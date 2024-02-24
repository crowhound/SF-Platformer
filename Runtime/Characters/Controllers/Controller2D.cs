using System;

using UnityEngine;

using SF.Physics.Helpers;
using SF.Physics.Collision;

using SF.Character.Core;


namespace SF.Characters.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
	public class Controller2D : MonoBehaviour
	{
		public bool IsDebugModeActivated;

		[Header("States")]
		public CharacterState CharacterState;

		public Vector2 Direction
		{
			get { return _direction; }
			set 
			{ 
				_direction = value;
				OnDirectionChanged?.Invoke(this, _direction);
			}
		}
		private Vector2 _direction;
		public EventHandler<Vector2> OnDirectionChanged;

		[field:SerializeField] public Vector2 Velocity { get; protected set; }
		protected Vector2 _calculatedVelocity;
		protected Vector2 _previousVelocity;
		protected Vector2 _controllerVelocity;
		
		#region Components 
		protected BoundsData _boundsData;
		protected Rigidbody2D _rigidbody2D;
		#endregion

		public CollisionInfo CollisionInfo;
		public CollisionController CollisionController = new(0.05f, 0.02f, 3, 4);

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
			OnStart();
		}
		protected virtual void OnStart()
		{

		}
		private void FixedUpdate()
		{
			OnPreFixedUpdate();
			_previousVelocity = _calculatedVelocity;

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
		}
		protected virtual void OnPreFixedUpdate()
		{
		}
		#endregion
		#region Movement Calculations
		protected virtual void Move()
		{
			_rigidbody2D.velocity = _calculatedVelocity;
			/*
			  The old way of doing it. Converted over to use per velocity to match the standard of non kinematic rigidbodies being moved with velocity. 

			 Will need to keep an eye on things that change the vertical velocity through code and not through Unity's Gravity physic step. 
			 
			_rigidbody2D.MovePosition( (Vector2)transform.position + _calculatedVelocity * Time.fixedDeltaTime);
			*/
			Velocity = _calculatedVelocity;
		}
		protected virtual void ColisionChecks()
		{

		}
		protected virtual void CalculateHorizontal()
		{
		}
		protected virtual void CalculateVertical()
		{
		}
		public virtual void AddVelocity(Vector2 velocity)
		{
			_controllerVelocity += velocity;
		}
		public virtual void AddHorizontalVelocity(float horizontalVelocity)
		{
			_controllerVelocity.x += horizontalVelocity;
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
		protected virtual void Reset()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_rigidbody2D.freezeRotation = true;
		}
	}
}
