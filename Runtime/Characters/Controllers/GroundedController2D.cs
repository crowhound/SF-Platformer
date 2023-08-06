using UnityEngine;
using SF.Physics;
using SF.Physics.Collision;
using SF.Character.Core;

namespace SF.Characters.Controllers
{
    public class GroundedController2D : Controller2D
    {
		[Header("States")]
		public CharacterState CharacterState;

		[Header("Physics Properties")]
		public MovementProperties DefaultPhysics = new(5);
		public MovementProperties CurrentPhysics;

		[Header("Collision Settings")]
		public ContactFilter2D PlatformFilter;
		public GameObject StandingOnObject;
		public CollisionController CollisionController = new(0.01f);
		protected RaycastHit2D GroundedHit;
		protected RaycastHit2D CeilingHit;
		protected RaycastHit2D RightHit;
		protected RaycastHit2D LeftHit;
		public bool IsCollidingRight = false;
		public bool IsCollidingLeft = false;
		public bool IsCollidingAbove = false;
		[Header("Booleans")]
		public bool IsGrounded = false;
		public bool IsSwimming = false;
		public bool IsJumping = false;
		#region Components
		protected BoxCollider2D _boxCollider;
		#endregion
		protected override void OnStart()
		{
			if(DefaultPhysics.GroundMaxSpeed < DefaultPhysics.GroundSpeed)
				DefaultPhysics.GroundMaxSpeed = DefaultPhysics.GroundSpeed;

			CurrentPhysics = DefaultPhysics;
		}
		#region Collision Calculations
		protected override void ColisionChecks()
		{
			GroundChecks();
			SideCollisionChecks();
		}
		protected virtual void GroundChecks()
		{
			if(IsJumping)
			{
				IsGrounded = false;
				return;
			}
			//GroundedHit = Physics2D.Raycast(_boundsData.BottomCenter, Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter.layerMask);
			IsGrounded = RaycastMultiple(_boundsData.BottomLeft, _boundsData.BottomRight ,Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);

			//IsGrounded = (GroundedHit) ? true: false;
			//StandingOnObject = (GroundedHit) ? GroundedHit.collider.gameObject : null;
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

		/* Custom Raycast hit data I will create for 2D Raycasting.
		public HitData2D RaycastMultiple(Vector2 origin, Vector2 end, float distance, ContactFilter2D contactFilter2D)
		{
			return RaycastMultiple(origin, end, distance, contactFilter2D.layerMask);
		}*/

		protected virtual void RightCollisionChecks()
		{

		}
		protected virtual void SideCollisionChecks()
		{
			//Right Side
			RightHit = Physics2D.Raycast(_boundsData.MiddleRight,
				Vector2.right, 
				CollisionController.HoriztonalRayDistance,
				PlatformFilter.layerMask);

			IsCollidingRight = RightHit.collider != null;

			LeftHit = Physics2D.Raycast(_boundsData.MiddleLeft,
				Vector2.left,
				CollisionController.HoriztonalRayDistance,
				PlatformFilter.layerMask);

			IsCollidingLeft = LeftHit.collider != null;
		}
		#endregion
		protected override void CalculateHorizontal()
		{

			if(Direction.x != 0)
			{
				_calculatedVelocity.x = CurrentPhysics.GroundSpeed * Direction.x;

				_calculatedVelocity.x = Direction.x > 0
					? (_calculatedVelocity.x > CurrentPhysics.GroundMaxSpeed) // Moving Right
						? CurrentPhysics.GroundMaxSpeed
						: _calculatedVelocity.x
					: (_calculatedVelocity.x <  -1 * CurrentPhysics.GroundMaxSpeed) // Moving Left
						? -1 * CurrentPhysics.GroundMaxSpeed
						: _calculatedVelocity.x;

				_calculatedVelocity.x = Direction.x > 0
					? IsCollidingRight // Moving Right
						? 0
						: _calculatedVelocity.x
					: IsCollidingLeft // Moving Left
						? 0
						: _calculatedVelocity.x;
			}
			else
			{
				_calculatedVelocity.x = 0;
				_rigidbody2D.velocityX = 0;
			}	
		}
		protected override void CalculateVertical()
		{
			if(!IsGrounded)
			{
				_calculatedVelocity.y += (-1 * CurrentPhysics.GravityScale);

				if(_calculatedVelocity.y < 0)
				{
					_calculatedVelocity.y = (_calculatedVelocity.y < (-1 * CurrentPhysics.TerminalVelocity)
						? -1 * CurrentPhysics.TerminalVelocity
						: _calculatedVelocity.y);
				}
				else if(_calculatedVelocity.y > 0)
				{
					_calculatedVelocity.y = (_calculatedVelocity.y > (CurrentPhysics.MaxUpForce)
						? CurrentPhysics.MaxUpForce
						: _calculatedVelocity.y);
				}
			}
			else // IsGrounded
			{
				_calculatedVelocity.y = (IsJumping) ? _calculatedVelocity.y : 0;
				_rigidbody2D.velocityY = (IsJumping) ? _rigidbody2D.velocityY : 0;
			}
		}
		public virtual void UpdatePhysics(MovementProperties movementProperties)
		{
			CurrentPhysics = movementProperties;
		}
		protected override void CalculateMovementState()
		{
			if(IsJumping)
			{
				CharacterState.MovementState = (_calculatedVelocity.y > 0) ? MovementState.Jumping : MovementState.Falling;
				if(CharacterState.MovementState == MovementState.Falling)
					IsJumping = false;
			}

			if(IsGrounded)
			{
				CharacterState.MovementState = (Direction.x == 0) ? MovementState.Idle : MovementState.Walking;
			}
		}
	}
}
