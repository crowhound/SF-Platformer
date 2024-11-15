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
            _rigidbody2D.freezeRotation = true;
            OnInit();
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
            OnStart();
        }
        protected virtual void OnStart()
        {

        }
        private void FixedUpdate()
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

            _rigidbody2D.linearVelocity = _calculatedVelocity;
        }


        protected virtual void CalculateHorizontal()
        {
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
        protected virtual void GroundChecks()
        {
            // This will eventually also show colliding with other things than platforms.
            CollisionInfo.IsCollidingBelow = RaycastMultiple(Bounds.BottomLeft(), Bounds.BottomRight(), Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);
        }
        protected virtual void CeilingChecks()
        {
            CollisionInfo.IsCollidingAbove = RaycastMultiple(Bounds.TopLeft(), Bounds.TopRight(), Vector2.up, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);
        }
        protected virtual void SideCollisionChecks()
        {
            // Right Side
            CollisionInfo.IsCollidingRight = RaycastMultiple(Bounds.TopRight(), Bounds.MiddleRight(), Vector2.right, CollisionController.HoriztonalRayDistance, PlatformFilter, CollisionController.HoriztonalRayAmount);

            // Left Side
            CollisionInfo.IsCollidingLeft = RaycastMultiple(Bounds.TopLeft(), Bounds.MiddleLeft(), Vector2.left, CollisionController.HoriztonalRayDistance, PlatformFilter, CollisionController.HoriztonalRayAmount);
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
                hasHit = Physics2D.Raycast(startPosition, direction, distance, layerMask);

                if(hasHit)
                {

                    if(direction.x > 0)
                        CollisionInfo.RightHit = hasHit;
                    else
                        CollisionInfo.LeftHit = hasHit;

                    if(direction.y > 0)
                        CollisionInfo.CeilingHit = hasHit;
                    else
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
        public virtual void Reset()
        {
            if(_rigidbody2D == null)
                _rigidbody2D = GetComponent<Rigidbody2D>();

            _calculatedVelocity = Vector3.zero;
            _rigidbody2D.linearVelocity = Vector3.zero;
            _externalVelocity = Vector3.zero;
        }
    }
}
