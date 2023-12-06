using UnityEngine;
using SF.Physics;
using SF.Physics.Collision;
using SF.Character.Core;

namespace SF.Characters.Controllers
{
	[System.Serializable]
	public struct CollisionInfo
	{
		public RaycastHit2D GroundedHit;
		public RaycastHit2D CeilingHit;
		public RaycastHit2D RightHit;
		public RaycastHit2D LeftHit;

		public bool IsCollidingRight;
		public bool IsCollidingLeft;
		public bool IsCollidingAbove;
		public bool IsCollidingBelow;
	}

	[RequireComponent(typeof(BoxCollider2D))]
	public class GroundedController2D : Controller2D
    {
		[Header("States")]
		public CharacterState CharacterState;

		[Header("Physics Properties")]
		public MovementProperties DefaultPhysics = new(5);
		public MovementProperties CurrentPhysics = new(0);

		[Header("Collision Settings")]
		public ContactFilter2D PlatformFilter;
		public GameObject StandingOnObject;

		protected CollisionInfo CollisionInfo;
		public CollisionController CollisionController = new(0.05f,0.02f,3,4);
		

		[Header("Booleans")]
		public bool IsGrounded = false;
		public bool IsSwimming = false;
		public bool IsJumping = false;
		#region Components 
		protected BoxCollider2D _boxCollider;
		#endregion
		protected override void OnAwake()
		{
			_boxCollider = GetComponent<BoxCollider2D>();
			_boundsData.Bounds = _boxCollider.bounds;
		}
		protected override void OnStart()
		{
			if(DefaultPhysics.GroundSpeed > DefaultPhysics.GroundMaxSpeed)
				DefaultPhysics.GroundSpeed = DefaultPhysics.GroundMaxSpeed;
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
			CollisionInfo.RightHit = Physics2D.Raycast(_boundsData.MiddleRight,
				Vector2.right, 
				CollisionController.HoriztonalRayDistance,
				PlatformFilter.layerMask);

			CollisionInfo.IsCollidingRight = CollisionInfo.RightHit.collider != null;

			CollisionInfo.LeftHit = Physics2D.Raycast(_boundsData.MiddleLeft,
				Vector2.left,
				CollisionController.HoriztonalRayDistance,
				PlatformFilter.layerMask);

			CollisionInfo.IsCollidingLeft = CollisionInfo.LeftHit.collider != null;
		}
		#endregion
		protected override void OnPreFixedUpdate()
		{
			_boundsData.Bounds = _boxCollider.bounds;
		}
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
					? CollisionInfo.IsCollidingRight // Moving Right
						? 0
						: _calculatedVelocity.x
					: CollisionInfo.IsCollidingLeft // Moving Left
						? 0
						: _calculatedVelocity.x;
			}
			else if(_controllerVelocity.x == 0)
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
				_calculatedVelocity.y = IsJumping ? _calculatedVelocity.y : 0;
				_rigidbody2D.velocityY = IsJumping ? _rigidbody2D.velocityY : 0;
			}
		}
		public virtual void UpdatePhysics(MovementProperties movementProperties)
		{
			CurrentPhysics.GroundSpeed = movementProperties.GroundSpeed;
			CurrentPhysics.GroundAcceleration = movementProperties.GroundAcceleration;
			CurrentPhysics.GroundMaxSpeed = movementProperties.GroundMaxSpeed;
			
			CurrentPhysics.GravityScale = movementProperties.GravityScale;
			CurrentPhysics.GravityAcceleration = movementProperties.GravityAcceleration;
			CurrentPhysics.TerminalVelocity = movementProperties.TerminalVelocity;
			CurrentPhysics.MaxUpForce = movementProperties.MaxUpForce;
			
			CurrentPhysics.JumpHeight = movementProperties.JumpHeight;

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
