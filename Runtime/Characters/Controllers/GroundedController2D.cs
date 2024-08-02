using UnityEngine;
using SF.Physics;
using SF.Physics.Collision;
using SF.Character.Core;
using System;
using SF.Utilities;

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

		[Header("Slope Settings")]
		[SerializeField] private bool _useSlopes = false;
		public float SlopeLimit = 35;
		public float StandingOnSlopeAngle;
		public bool OnSlope = false;

		public Action OnGrounded;

		#region Components 
		protected BoxCollider2D _boxCollider;
		#endregion
		protected override void OnAwake()
		{
			_boxCollider = GetComponent<BoxCollider2D>();
			Bounds = _boxCollider.bounds;
		}
		protected override void OnStart()
		{
			DefaultPhysics.GroundSpeed = Mathf.Clamp(DefaultPhysics.GroundSpeed, 0, DefaultPhysics.GroundMaxSpeed);

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

			// If grounded last frame, but grounded this frame call OnGrounded
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
		protected override void CalculateMovementState()
		{

			if(_calculatedVelocity.y < 0)
			{
                IsFalling = true;
                IsJumping = false;
				CharacterState.MovementState = MovementState.Falling;
            }

			if(IsJumping)
				CharacterState.MovementState = MovementState.Jumping;

			if(IsGrounded)
			{
				IsFalling = false;
				CharacterState.MovementState = (Direction.x == 0) ? MovementState.Idle : MovementState.Walking;
			}
		}
	}
}