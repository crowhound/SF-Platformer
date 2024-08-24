using UnityEngine;
using SF.Physics;
using SF.Physics.Collision;
using SF.Character.Core;
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
		public ContactFilter2D PlatformFilter;
		public GameObject StandingOnObject;


		[Header("Booleans")]
		public bool IsGrounded = false;
		protected bool _wasGroundedLastFrame = false;
		public bool IsRunning = false;
		public bool IsSwimming = false;
		public bool IsJumping = false;
		public bool IsFalling = false;
		public bool IsGliding = false;
		public bool IsCrouching = false;

		[Header("Slope Settings")]
		[SerializeField] private bool _useSlopes = false;
		public float SlopeLimit = 35;
		public float StandingOnSlopeAngle;
		public bool OnSlope = false;

		public Action OnGrounded;

		#region Components 
		protected BoxCollider2D _boxCollider;
		protected Vector2 _originalColliderSize;
		protected Vector2 _modifiedColliderSize;
		protected Vector2 _previousColliderSize;
		#endregion
		protected override void OnAwake()
		{
			_boxCollider = GetComponent<BoxCollider2D>();
			Bounds = _boxCollider.bounds;
			_originalColliderSize = _boxCollider.size;
		}
		protected override void OnStart()
		{
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
		protected virtual void GroundChecks()
		{
			// This will eventually also show colliding with other things than platforms.
			CollisionInfo.IsCollidingBelow = RaycastMultiple(Bounds.BottomLeft(), Bounds.BottomRight(), Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);

			if(IsJumping)
			{
				IsGrounded = false;
				return;
			}

			IsGrounded = RaycastMultiple(Bounds.BottomLeft(), Bounds.BottomRight() ,Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);

			// If grounded last frame, but grounded this frame call GlideReset
			if(!_wasGroundedLastFrame && IsGrounded)
			{
				if(_calculatedVelocity.y < 0)
                    _calculatedVelocity.y = 0;
				OnGrounded?.Invoke();
			}
		}
		protected virtual void CeilingChecks()
		{
			CollisionInfo.IsCollidingAbove = RaycastMultiple(Bounds.TopLeft(), Bounds.TopRight(), Vector2.up, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);
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
			bool hasHit = false;
			Vector2 startPosition;
			float stepPercent;

			for(int x = 0; x < numberOfRays; x++)
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				hasHit =  Physics2D.Raycast(startPosition, direction, distance, layerMask);

				if(hasHit)
					return true;
			}
			return hasHit;
		}

		public bool RaycastMultiple(Vector2 origin, Vector2 end, Vector2 direction, float distance, ContactFilter2D contactFilter2D, int numberOfRays = 4)
		{
			return RaycastMultiple(origin, end, direction, distance, contactFilter2D.layerMask, numberOfRays);
		}

		#endregion
		protected override void OnPreFixedUpdate()
		{
			Bounds = _boxCollider.bounds;
		}
		protected override void CalculateHorizontal()
		{
			CalculateSlope();

			if(Direction.x != 0)
			{
				// We only have to do a single clamp because than Direction.x takes care of it being negative or not when being multiplied.
				ReferenceSpeed = Mathf.Clamp(ReferenceSpeed,0,CurrentPhysics.GroundMaxSpeed);

				_calculatedVelocity.x = ReferenceSpeed * Direction.x;

				// Moving right
				if (Direction.x > 0 && CollisionInfo.IsCollidingRight)
					_calculatedVelocity.x = 0;
				// Moving left
				else if (Direction.x < 0 && CollisionInfo.IsCollidingLeft)
					_calculatedVelocity.x = 0;
            }
			else
			{
                _calculatedVelocity.x = Mathf.MoveTowards(_calculatedVelocity.x, 0, CurrentPhysics.GroundDeacceleration);
            }
		}
		protected override void CalculateVertical()
		{
			if(!IsGrounded)
			{
				_calculatedVelocity.y += (-1 * CurrentPhysics.GravityScale);
				_calculatedVelocity.y = Mathf.Clamp(_calculatedVelocity.y,
					-CurrentPhysics.TerminalVelocity,
					CurrentPhysics.TerminalVelocity);
			}
		}

		public virtual void CalculateSlope()
		{
			if(!_useSlopes)
				return;

			RaycastHit2D hit = Physics2D.Raycast(Bounds.BottomLeft(), Vector2.down,.25f);
            StandingOnSlopeAngle = Vector2.Angle(hit.normal,Vector2.up);

			OnSlope = StandingOnSlopeAngle > SlopeLimit;
			
			if(OnSlope)
				IsGrounded = true;
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
			// TODO: There are some places that set the values outside of this function. Find a way to make it where this function is the only needed one. Example IsJump in the Jumping Ability.

			// If our velocity is negative we are either falling/gliding.
			if(_calculatedVelocity.y < 0)
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
	}
}