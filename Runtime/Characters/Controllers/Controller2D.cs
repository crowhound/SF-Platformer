using System;

using UnityEngine;

using SF.Physics;

#if SF_Utilities
using SF.Utilities;
#else
using SF.Platformer.Utilities;
#endif

namespace SF.Characters.Controllers
{
    /// <summary>
    /// A physics controller used to add custom physics logic to any object. 
    /// This physics controller adds the ability to invoke events when colliding on per direction basis by
    /// using the <see cref="CollisionController"/> 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class Controller2D : MonoBehaviour, IForceReciever
    {
        /// <summary>
		/// Reference speed if used for passing in a value in horizontal calculatin based on running or not.
		/// </summary>
		[NonSerialized] public float ReferenceSpeed;

        public float DistanceToGround;

        [Header("Physics Properties")]
        public MovementProperties DefaultPhysics = new(new Vector2(5, 5));
        public MovementProperties CurrentPhysics = new(new Vector2(5, 5));

        public CharacterState CharacterState;
        public ContactFilter2D PlatformFilter;


        public Vector2 Direction
        {
            get { return _direction; }
            set
            {
                _direction = value;
                OnDirectionChanged?.Invoke(this, _direction);
            }
        }
        [SerializeField] private Vector2 _direction;
        public EventHandler<Vector2> OnDirectionChanged;

        #region Components 
        protected BoxCollider2D _boxCollider;
        protected Rigidbody2D _rigidbody2D;
        #endregion

        #region
        [NonSerialized] public Bounds Bounds;
        #endregion
        [Header("Collision Data")]
        public CollisionInfo CollisionInfo;
        public CollisionController CollisionController = new(0.05f, 0.02f, 3, 4);

        /// <summary>
        /// The overall velocity to be added this frame.
        /// </summary>
        protected Vector2 _calculatedVelocity;
        /// <summary>
        /// Velocity adding through external physics forces such as gravity and interactable objects.
        /// </summary>
        protected Vector2 _externalVelocity;

        #region Lifecycle Methods
        private void Awake()
        {
            Init();
            OnAwake();
        }
        /// <summary>
        /// This runs before OnAwake code to make sure things needing Initialized are
        /// ready before it is called and needed. This can be called externally if
        /// the Controller ever needs reset. Think spawning a character.
        /// </summary>
        public void Init()
        {
            // I think I should add the collider assignment here.
            // Even flying enemies need colliders to hurt the player. 
            _rigidbody2D = _rigidbody2D != null ? _rigidbody2D : GetComponent<Rigidbody2D>();
            _boxCollider = _boxCollider != null ? _boxCollider : GetComponent<BoxCollider2D>();
            SetComponentSetting();
            OnInit();
        }

        private void SetComponentSetting()
        {
            if(_boxCollider != null)
                _boxCollider.isTrigger = false;

            if(_rigidbody2D != null)
            {
                _rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
                _rigidbody2D.freezeRotation = true;
            }
        }

        protected virtual void OnInit()
        {

        }
        protected virtual void OnAwake()
        {
            _rigidbody2D.gravityScale = 0;
        }
        private void Start()
        {
            // Need to check why this is in twice. Gravity Scale is being set also in OnAwake.
            _rigidbody2D.gravityScale = 0;

            CharacterState.StatusEffectChanged += OnStatusEffectChanged;
            DefaultPhysics.GroundSpeed = Mathf.Clamp(DefaultPhysics.GroundSpeed, 0, DefaultPhysics.GroundMaxSpeed);

            PlatformFilter.useLayerMask = true;

            CurrentPhysics = DefaultPhysics;
            ReferenceSpeed = CurrentPhysics.GroundSpeed;

            OnStart();
        }
        protected virtual void OnStart()
        {
        }

        private void Update()
        {
            Bounds = _boxCollider.bounds;

            OnPreFixedUpdate();

            // Set the bools for what sides there was a collision on last frame.
            CollisionInfo.WasCollidingRight = CollisionInfo.IsCollidingRight;
            CollisionInfo.WasCollidingLeft = CollisionInfo.IsCollidingLeft;
            CollisionInfo.WasCollidingAbove = CollisionInfo.IsCollidingAbove;
            CollisionInfo.WasCollidingBelow = CollisionInfo.IsCollidingBelow;

            ColisionChecks();
            CalculateHorizontal();
            CalculateVertical();
            Move();
        }
        private void LateUpdate()
        {
            CalculateMovementState();
            OnLateUpdate();
        }

        protected virtual void OnLateUpdate()
        {

        }
        protected virtual void OnPreFixedUpdate()
        {
        }
        #endregion
        #region Movement Calculations
        protected virtual void Move()
        {
            if(CharacterState.CharacterStatus == CharacterStatus.Dead)
                _calculatedVelocity = Vector2.zero;

            if(_externalVelocity != Vector2.zero)
            {
                _calculatedVelocity = _externalVelocity;
                _externalVelocity = Vector2.zero;
            }

            //Vector2 newPosition = (Vector2)transform.position + _calculatedVelocity * Time.deltaTime; 
            //_rigidbody2D.MovePosition(newPosition);

            transform.Translate(_calculatedVelocity * Time.deltaTime);

            if(CollisionInfo.BelowHit)
            {
                if(transform.position.y < CollisionInfo.BelowHit.point.y)
                    transform.position = CollisionInfo.BelowHit.point + new Vector2(0, 0.25f);
            }

        }


        protected virtual void CalculateHorizontal()
        {
            if(Direction.x != 0)
            {
                // We only have to do a single clamp because than Direction.x takes care of it being negative or not when being multiplied.
                ReferenceSpeed = Mathf.Clamp(ReferenceSpeed, 0, CurrentPhysics.GroundMaxSpeed);

                // TODO: When turning around erase previously directional velocity.
                // If it is kept the player could slide in the previous direction for a second before running the new direction on smaller ground acceleration values.
                _calculatedVelocity.x = Mathf.MoveTowards(_calculatedVelocity.x, ReferenceSpeed * Direction.x, CurrentPhysics.GroundAcceleration);

                // Moving right
                if(Direction.x > 0 && CollisionInfo.IsCollidingRight)
                    _calculatedVelocity.x = 0;
                // Moving left
                else if(Direction.x < 0 && CollisionInfo.IsCollidingLeft)
                {

                    _calculatedVelocity.x = 0;
                }
            }
            else
            {
                _calculatedVelocity.x = Mathf.MoveTowards(_calculatedVelocity.x, 0, CurrentPhysics.GroundDeacceleration);
            }


        }

        protected virtual void CalculateVertical()
        {
        }
        public void SetExternalVelocity(Vector2 force)
        {
            _externalVelocity = force;
        }
        public virtual void AddVelocity(Vector2 velocity)
        {
            _externalVelocity += velocity;
        }
        public virtual void AddHorizontalVelocity(float horizontalVelocity)
        {
            _externalVelocity.x += horizontalVelocity;
        }
        public virtual void AddVerticalVelocity(float verticalVelocity)
        {
            _calculatedVelocity.y += verticalVelocity;
        }
        public virtual void SetHorizontalVelocity(float horizontalVelocity)
        {
            _calculatedVelocity.x = horizontalVelocity;
        }
        public virtual void SetVerticalVelocity(float verticalVelocity)
        {
            // Need to compare this to _rigidbody2D.velocityY to see which one feels better. 
            _calculatedVelocity.y = verticalVelocity;
        }
        protected virtual void CalculateMovementState()
        {

        }
        #endregion

        #region Collision Calculations
        protected virtual void ColisionChecks()
        {
            GroundChecks();
            CeilingChecks();
            SideCollisionChecks();
            CheckOnCollisionActions();
        }

        protected RaycastHit2D DebugBoxCast(Vector2 origin, 
            Vector2 size,
            float angle,
            Vector2 direction, 
            float distance, 
            LayerMask layerMask)
        {
#if UNITY_EDITOR
            Debug.DrawLine(origin, origin + (direction * distance));
#endif
            return Physics2D.BoxCast(origin, size,angle,direction, distance, layerMask);
        }

        protected RaycastHit2D DebugRayCast(Vector2 origin, Vector2 direction, float distance, LayerMask layerMask)
        {
#if UNITY_EDITOR

            Debug.DrawLine(origin, origin + (direction * distance));
#endif
            return Physics2D.Raycast(origin, direction, distance, layerMask);
        }


        protected virtual void GroundChecks()
        {
            // This will eventually also show colliding with other things than platforms.
            CollisionInfo.IsCollidingBelow = RaycastMultiple(Bounds.BottomLeft(), Bounds.BottomRight(), Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);
        }
        protected virtual void CeilingChecks()
        {
            CollisionInfo.IsCollidingAbove = RaycastMultiple(Bounds.TopLeft(), Bounds.TopRight(), Vector2.up, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);

            if(CollisionInfo.IsCollidingAbove)
            {
                // If colliding above reset the vertical velocity if it is above zero
                // This prevents that hanging feeling when touching a ceiling.

                if(_calculatedVelocity.y > 0)
                    _calculatedVelocity.y = 0;
            }
        }
        protected virtual void SideCollisionChecks()
        {
            // Right Side
            CollisionInfo.IsCollidingRight = RaycastMultiple(Bounds.TopRight(), Bounds.BottomRight(), Vector2.right, CollisionController.HoriztonalRayDistance, PlatformFilter, CollisionController.HoriztonalRayAmount);

            // Left Side
            CollisionInfo.IsCollidingLeft = RaycastMultiple(Bounds.TopLeft(), Bounds.BottomLeft(), Vector2.left, CollisionController.HoriztonalRayDistance, PlatformFilter, CollisionController.HoriztonalRayAmount);
        }

        public bool RaycastMultiple(Vector2 origin, Vector2 end, Vector2 direction, float distance, LayerMask layerMask, int numberOfRays = 4)
        {
            RaycastHit2D hasHit;
            Vector2 startPosition;
            float stepPercent;
            for(int x = 0; x < numberOfRays; x++)
            {
                stepPercent = (float)x / (float)(numberOfRays - 1);
                startPosition = Vector2.Lerp(origin, end, stepPercent);
                hasHit = DebugRayCast(startPosition, direction, distance, layerMask);

                if(hasHit)
                {
                    if(direction.x > 0 && direction.y == 0 )
                        CollisionInfo.RightHit = hasHit;
                    else if(direction.x < 0 && direction.y == 0)
                        CollisionInfo.LeftHit = hasHit;

                    if(direction.y > 0)
                        CollisionInfo.CeilingHit = hasHit;
                    else if(direction.y < 0)
                        CollisionInfo.BelowHit = hasHit;
                    return true;
                }
            }

            return false;
        }

        public bool RaycastMultiple(Vector2 origin, Vector2 end, Vector2 direction, float distance, ContactFilter2D contactFilter2D, int numberOfRays = 4)
        {
            return RaycastMultiple(origin, end, direction, distance, contactFilter2D.layerMask, numberOfRays);
        }

        #endregion

        /// <summary>
        /// Checks to see what sides might have a new collision that was started the current frame. If a new collision is detected on the side invoke the action related to that sides collisions.
        /// </summary>
        protected virtual void CheckOnCollisionActions()
        {
            // If we were not colliding on a side with anything last frame, but is now Invoke the OnCollisionActions.

            // Right Side
            if(!CollisionInfo.WasCollidingRight && CollisionInfo.IsCollidingRight)
                CollisionInfo.OnCollidedRight?.Invoke();

            // Left Side
            if(!CollisionInfo.WasCollidingLeft && CollisionInfo.IsCollidingLeft)
                CollisionInfo.OnCollidedLeft?.Invoke();

            // Above Side
            if(!CollisionInfo.WasCollidingAbove && CollisionInfo.IsCollidingAbove)
                CollisionInfo.OnCollidedAbove?.Invoke();

            //Below Side
            if(!CollisionInfo.WasCollidingBelow && CollisionInfo.IsCollidingBelow)
                CollisionInfo.OnCollidedBelow?.Invoke();
        }
        public void ChangeDirection()
        {
            Direction *= -1;
        }
        
        protected void OnStatusEffectChanged(StatusEffect statusEffect)
        {
            if(statusEffect == StatusEffect.Beserk)
                GetComponent<SpriteRenderer>().color = Color.red;
        }


        /// <summary>
        /// Corects the posiution if the character clips or goes through an object due to moving to fast during a frame.
        /// </summary>
        protected virtual void PositionCorection()
        {
            var raycastHit2D = Physics2D.Raycast(
                    transform.position,Vector2.down, 
                    3,
                    PlatformFilter.layerMask
                );
            if(raycastHit2D)
                DistanceToGround = raycastHit2D.distance - (Bounds.size.y / 2);
            else 
                DistanceToGround = 0;
        }

        public virtual void Reset()
        {
            if(_rigidbody2D == null)
                _rigidbody2D = GetComponent<Rigidbody2D>();

            /* This un childs characters from attached platforms like
             * moving, climables, and so forth on death to prevent being linked to them if dying while on one. */
            transform.parent = null;

            _calculatedVelocity = Vector3.zero;
            _rigidbody2D.linearVelocity = Vector3.zero;
            _externalVelocity = Vector3.zero;
        }
    }
}
