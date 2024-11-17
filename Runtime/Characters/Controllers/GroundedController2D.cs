#if UNITY_EDITOR
// The two below namespaces are only used in the OnDrawGizmos
// at the time of typing this comment.
using System.Collections.Generic;
using System.Runtime.InteropServices;
#endif

using UnityEngine;
using SF.Physics;
using System;

#if SF_Utilities
using SF.Utilities;
#else
using SF.Platformer.Utilities;
#endif

namespace SF.Characters.Controllers
{
	[RequireComponent(typeof(BoxCollider2D))]
	public class GroundedController2D : Controller2D
	{

		/// <summary>
		/// Reference speed if used for passing in a value in horizontal calculatin based on running or not.
		/// </summary>
		[NonSerialized] public float ReferenceSpeed;

		[Header("Platform Settings")]
        [SerializeField] protected ContactFilter2D OneWayPlatformFilter;
        [SerializeField] public GameObject StandingOnObject { get; protected set; }


		[Header("Booleans")]
		public bool IsGrounded = false;
		protected bool _wasGroundedLastFrame = false;
		public bool IsRunning = false;
		public bool IsSwimming = false;
		public bool IsJumping = false;
		public bool IsFalling = false;
		public bool IsGliding = false;
		public bool IsCrouching = false;


		public bool IsClimbing
		{
			get { return _isClimbing; }
			set 
			{ 
				_isClimbing = value; 

				if(!_isClimbing)
				{
					CurrentPhysics.GravityScale = DefaultPhysics.GravityScale;
                    _character.CanTurnAround = true;
                }
				else
				{
					CurrentPhysics.GravityScale = 0;
					_character.CanTurnAround = false;
				}
			}
		}
		[SerializeField] private bool _isClimbing = false;
		[Header("Slope Settings")]
		[SerializeField] private bool _useSlopes = false;
		public float SlopeLimit = 55;
		public float SlopeSlipLimit = 35;
		[SerializeField] protected Vector2 _slopeNormal;
		public float StandingOnSlopeAngle;
		public bool OnSlope = false;
		protected Vector2 _slopeSideDirection;

		protected int OneWayFilterBitMask => PlatformFilter.layerMask & OneWayPlatformFilter.layerMask;
		public Action OnGrounded;

		protected Character2D _character;
		#region Components 
		protected Vector2 _originalColliderSize;
		protected Vector2 _modifiedColliderSize;
		protected Vector2 _previousColliderSize;
		#endregion
		protected override void OnAwake()
		{
			_character = GetComponent<Character2D>();
			_boxCollider = GetComponent<BoxCollider2D>();
			Bounds = _boxCollider.bounds;
			_originalColliderSize = _boxCollider.size;
		}
		protected override void OnStart()
		{
			CharacterState.StatusEffectChanged += OnStatusEffectChanged;
			DefaultPhysics.GroundSpeed = Mathf.Clamp(DefaultPhysics.GroundSpeed, 0, DefaultPhysics.GroundMaxSpeed);

			PlatformFilter.useLayerMask = true;

			CurrentPhysics = DefaultPhysics;
			ReferenceSpeed = CurrentPhysics.GroundSpeed;
		}
		#region Collision Calculations
		protected override void ColisionChecks()
		{
			_wasGroundedLastFrame = IsGrounded;
			GroundChecks();
			CeilingChecks();
			SideCollisionChecks();
			CheckOnCollisionActions();
		}
		protected override void GroundChecks()
		{
            // This will eventually also show colliding with other things than platforms.

            CollisionInfo.BelowHit = Physics2D.BoxCast(
                        Bounds.BottomCenter(),
                        new Vector2(Bounds.size.x, .02f),
                        0,
                        Vector2.down,
                        CollisionController.HoriztonalRayDistance,
                        PlatformFilter.layerMask
                    );
            CollisionInfo.IsCollidingBelow = CollisionInfo.BelowHit;

            if(IsJumping)
			{
				IsGrounded = false;
				return;
			}

			IsGrounded = CollisionInfo.IsCollidingBelow;

            if(IsGrounded)
				_calculatedVelocity.y = 0;

			// If grounded last frame, but grounded this frame call OnGrounded
			if(!_wasGroundedLastFrame && IsGrounded)
			{
				if(_calculatedVelocity.y < 0)
					_calculatedVelocity.y = 0;
				OnGrounded?.Invoke();
			}
		}
		protected override void SideCollisionChecks()
		{
            // Right Side
            CollisionInfo.IsCollidingRight = Physics2D.BoxCast(Bounds.MiddleRight(), new Vector2(.02f,Bounds.size.y), 0, Vector2.right, CollisionController.HoriztonalRayDistance, PlatformFilter.layerMask);

            // Left Side
            CollisionInfo.IsCollidingLeft = Physics2D.BoxCast(Bounds.MiddleLeft(), new Vector2(.02f, Bounds.size.y), 0, Vector2.left, CollisionController.HoriztonalRayDistance, PlatformFilter.layerMask);

			RaycastHit2D hit2D;

            if(Direction.x > 0) // Looking Right
				hit2D = Physics2D.BoxCast(Bounds.MiddleRight(), _boxCollider.size, 0, Vector2.right, CollisionController.HoriztonalRayDistance, PlatformFilter.layerMask);
			else
                hit2D = Physics2D.BoxCast(Bounds.MiddleLeft(), new Vector2(CollisionController.HoriztonalRayDistance, _boxCollider.size.y), 0, Vector2.left, CollisionController.HoriztonalRayDistance, PlatformFilter.layerMask);

			if(!hit2D)
                CollisionInfo.ClimbableSurfaceHit = new RaycastHit2D();
            else if(hit2D.collider.TryGetComponent(out ClimbableSurface climbableSurface))
				CollisionInfo.ClimbableSurfaceHit = hit2D;
		}

		#endregion
        protected void OnStatusEffectChanged(StatusEffect statusEffect)
        {
			if(statusEffect == StatusEffect.Beserk)
				GetComponent<SpriteRenderer>().color = Color.red;
        }
        protected override void CalculateHorizontal()
		{
			if(IsClimbing)
			{
                _calculatedVelocity.x = 0;
                return;
			}

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
					_calculatedVelocity.x = 0;
			}
			else
			{
				_calculatedVelocity.x = Mathf.MoveTowards(_calculatedVelocity.x, 0, CurrentPhysics.GroundDeacceleration);
			}
		}
		protected override void CalculateVertical()
		{
			if(IsClimbing)
			{
				_calculatedVelocity.y = Direction.y * CurrentPhysics.ClimbSpeed.y;
			}

			if(!IsGrounded && !IsClimbing)
			{
				_calculatedVelocity.y += (-1 * CurrentPhysics.GravityScale);
				_calculatedVelocity.y = Mathf.Clamp(_calculatedVelocity.y,
					-CurrentPhysics.TerminalVelocity,
					CurrentPhysics.MaxUpForce);
			}

		}

		public virtual void CalculateSlope()
		{
			if(!_useSlopes)
				return;

			if(Direction.x > 0)
				_slopeNormal = Physics2D.Raycast(Bounds.BottomRight(), Vector2.down, .25f).normal;
			else
				_slopeNormal = Physics2D.Raycast(Bounds.BottomLeft(), Vector2.down, .25f).normal;

			StandingOnSlopeAngle = Vector2.Angle(_slopeNormal, Vector2.up);

			OnSlope = StandingOnSlopeAngle > 5;

			if(OnSlope)
			{
				IsGrounded = true;
				// TODO: Make the ability to walk up slopes.
				_calculatedVelocity = Vector3.ProjectOnPlane(_calculatedVelocity, _slopeNormal);
			}
		}

		protected override void Move()
		{
			//CalculateSlope();

			base.Move();
		}
		public virtual void UpdatePhysics(MovementProperties movementProperties)
		{
			CurrentPhysics.GroundSpeed = movementProperties.GroundSpeed;
			CurrentPhysics.GroundAcceleration = movementProperties.GroundAcceleration;
			CurrentPhysics.GroundMaxSpeed = movementProperties.GroundMaxSpeed;

			CurrentPhysics.GravityScale = movementProperties.GravityScale;
			CurrentPhysics.TerminalVelocity = movementProperties.TerminalVelocity;
			CurrentPhysics.MaxUpForce = movementProperties.MaxUpForce;
		}
		/// <summary>
		/// Calculates the current movement state that the player is currently in.
		/// </summary>
		/// <remarks>
		/// This needs to be moved into the Controller2d parent class.
		/// </remarks>
		protected override void CalculateMovementState()
		{

			if(CharacterState.CharacterStatus == CharacterStatus.Dead )
				return;

			// TODO: There are some places that set the values outside of this function. Find a way to make it where this function is the only needed one. Example IsJump in the Jumping Ability.

			if(IsClimbing)
			{
				if(_calculatedVelocity.y != 0)
					CharacterState.MovementState = MovementState.Climbing;
				else
					CharacterState.MovementState = MovementState.ClimbingIdle;
			}

			// If our velocity is negative we are either falling/gliding.
			if(_calculatedVelocity.y < 0 && !IsClimbing)
			{
				if(IsGliding)
					CharacterState.MovementState = MovementState.Gliding;
				else
				{
					// Need to remove the crouch check when I get the collider calculation more accurate on resizing.
					if(!IsCrouching)
						CharacterState.MovementState = MovementState.Falling;
				}
                IsFalling = true;
                IsJumping = false;
            }

			if(IsJumping)
				CharacterState.MovementState = MovementState.Jumping;

			if(IsGrounded)
			{
                IsFalling = false;

                if(IsCrouching)
				{
					CharacterState.MovementState = MovementState.Crouching;
					return;
				}
				
				CharacterState.MovementState = (Direction.x == 0) ? MovementState.Idle : MovementState.Walking;
			}
		}

		protected void LowerToGround()
		{
			RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down);
			transform.position = hit.point + new Vector2(0, CollisionController.VerticalRayDistance);
		}

		public virtual void ResizeCollider(Vector2 newSize)
		{
			// Need to keep track of the previous size if the collider was already resized once before, but wasn't reset to the default collider size.
			_previousColliderSize = _boxCollider.size;
			_modifiedColliderSize = newSize;
            _boxCollider.size = newSize;

            LowerToGround();
		}

        public void ResetColliderSize()
		{
			_boxCollider.size = _originalColliderSize;

			//TODO: Do checks if colliding on the sides or ceiling to make sure the default collider size doesn't click through them.

			// Put character above ground.
			if(IsGrounded)
			{
				transform.position += new Vector3(0, CollisionController.VerticalRayDistance, 0);

			}
		}

#if UNITY_EDITOR

        private readonly List<Vector3> _listOfPoints = new();

        public void OnDrawGizmos()
        {
            _listOfPoints.Clear();
            _boxCollider = (_boxCollider == null) ? GetComponent<BoxCollider2D>() : _boxCollider;
            Bounds = _boxCollider.bounds;

            Vector2 startPosition;
            float stepPercent;
            int numberOfRays = CollisionController.VerticalRayAmount;
            Vector2 origin = Bounds.BottomLeft();
            Vector2 end = Bounds.BottomRight();

            for(int x = 0; x < numberOfRays; x++) // Down
            {
                stepPercent = (float)x / (float)(numberOfRays - 1);
                startPosition = Vector2.Lerp(origin, end, stepPercent);
                if(x == 0)
                    startPosition += new Vector2(CollisionController.RayOffset, 0);
                if(x == numberOfRays - 1)
                    startPosition -= new Vector2(CollisionController.RayOffset, 0);
                _listOfPoints.Add(startPosition);
                _listOfPoints.Add(startPosition - new Vector2(0, CollisionController.VerticalRayDistance));
            }

            numberOfRays = CollisionController.HoriztonalRayAmount;
            origin = Bounds.TopRight();
            end = Bounds.BottomRight();

            for(int x = 0; x < numberOfRays; x++) // Right
            {
                stepPercent = (float)x / (float)(numberOfRays - 1);
                startPosition = Vector2.Lerp(origin, end, stepPercent);
                _listOfPoints.Add(startPosition);
                _listOfPoints.Add(startPosition + new Vector2(CollisionController.HoriztonalRayDistance, 0));
            }

            for(int x = 0; x < numberOfRays; x++) // Left
            {
                stepPercent = (float)x / (float)(numberOfRays - 1);
                startPosition = Vector2.Lerp(Bounds.BottomLeft(), Bounds.TopLeft(), stepPercent);
                _listOfPoints.Add(startPosition);
                _listOfPoints.Add(startPosition - new Vector2(CollisionController.HoriztonalRayDistance, 0));
            }

            ReadOnlySpan<Vector3> pointsAsSpan = CollectionsMarshal.AsSpan(_listOfPoints);
            Gizmos.DrawLineList(pointsAsSpan);

            if(CollisionInfo.ClimbableSurfaceHit)
                Gizmos.DrawWireSphere(CollisionInfo.ClimbableSurfaceHit.point, .25f);

        }
#endif
    }
}