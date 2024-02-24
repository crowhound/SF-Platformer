using UnityEngine;
using SF.Physics;
using SF.Physics.Collision;
using SF.Character.Core;
using System;

namespace SF.Characters.Controllers
{


	[RequireComponent(typeof(BoxCollider2D))]
	public class GroundedController2D : Controller2D
    {
		[Header("Physics Properties")]
		public MovementProperties DefaultPhysics = new(5);
		public MovementProperties CurrentPhysics = new(0);
		/// <summary>
		/// Reference speed if used for passing in a value in horizontal calculatin based on running or not.
		/// </summary>
		[NonSerialized] public float ReferenceSpeed;

		[Header("Collision Settings")]
		public ContactFilter2D PlatformFilter;
		public GameObject StandingOnObject;


		[Header("Booleans")]
		public bool IsGrounded = false;
		protected bool _wasGroundedLastFrame = false;
		public bool IsRunning = false;
		public bool IsSwimming = false;
		public bool IsJumping = false;
		public bool IsFalling = false;


		public Action OnGrounded;

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
			CollisionInfo.IsCollidingBelow = RaycastMultiple(_boundsData.BottomLeft, _boundsData.BottomRight, Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);

			if(IsJumping)
			{
				IsGrounded = false;
				return;
			}
			IsGrounded = RaycastMultiple(_boundsData.BottomLeft, _boundsData.BottomRight ,Vector2.down, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);

			

			//StandingOnObject = (GroundedHit) ? GroundedHit.collider.gameObject : null;

			// If grounded last frame, but grounded this frame call OnGrounded
			if(!_wasGroundedLastFrame && IsGrounded)
			{
				OnGrounded?.Invoke();
			}
		}
		protected virtual void CeilingChecks()
		{
			CollisionInfo.IsCollidingAbove = RaycastMultiple(_boundsData.TopLeft, _boundsData.TopRight, Vector2.up, CollisionController.VerticalRayDistance, PlatformFilter, CollisionController.VerticalRayAmount);
		}

		protected virtual void SideCollisionChecks()
		{
			// Right Side
			CollisionInfo.IsCollidingRight = RaycastMultiple(_boundsData.TopRight, _boundsData.BottomRight, Vector2.right, CollisionController.HoriztonalRayDistance, PlatformFilter, CollisionController.HoriztonalRayAmount);

			// Left Side
			CollisionInfo.IsCollidingLeft = RaycastMultiple(_boundsData.TopLeft, _boundsData.BottomLeft, Vector2.left, CollisionController.HoriztonalRayDistance, PlatformFilter, CollisionController.HoriztonalRayAmount);
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
			_boundsData.Bounds = _boxCollider.bounds;
		}
		protected override void CalculateHorizontal()
		{
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
		}
		protected override void CalculateMovementState()
		{
			if(IsJumping)
			{
				CharacterState.MovementState = (_calculatedVelocity.y > 0) 
					? MovementState.Jumping 
					: MovementState.Falling;

				if (CharacterState.MovementState == MovementState.Falling)
				{
					IsFalling = true;
					IsJumping = false;
				}
			}

			if(IsGrounded)
			{
				IsFalling = false;
				CharacterState.MovementState = (Direction.x == 0) ? MovementState.Idle : MovementState.Walking;
			}
		}
	}
}