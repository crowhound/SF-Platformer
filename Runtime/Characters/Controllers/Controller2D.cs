using SF.Physics.Helpers;

using UnityEngine;

namespace SF.Characters.Controllers
{
    [RequireComponent(typeof(Rigidbody2D))]
	public class Controller2D : MonoBehaviour
	{
		public bool IsDebugModeActivated;

		public Vector2 Direction;
		[field:SerializeField] public Vector2 Velocity { get; protected set; }
		protected Vector2 _calculatedVelocity;
		protected Vector2 _previousVelocity;
		protected Vector2 _controllerVelocity;
		#region Components 
		protected BoundsData _boundsData;
		protected Rigidbody2D _rigidbody2D;
		#endregion

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
			_rigidbody2D.MovePosition( (Vector2)transform.position + _calculatedVelocity * Time.fixedDeltaTime);
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
		protected virtual void AddForce(Vector2 force)
		{
			_calculatedVelocity += force;
		}
		protected virtual void AddHorizontalForce(float horizontalForce)
		{
			_calculatedVelocity.x += horizontalForce;
		}
		protected virtual void AddVerticalForce(float verticalForce)
		{
			_controllerVelocity.y += verticalForce;
		}
		protected virtual void SetHorizontalVelocity(float horizontalForce)
		{
			_rigidbody2D.velocityX = horizontalForce;
		}
		protected virtual void SetVerticalVelocity(float verticalForce)
		{
			_calculatedVelocity.y = verticalForce;
		}
		protected virtual void CalculateMovementState()
		{

		}
		#endregion

		protected virtual void Reset()
		{
			_rigidbody2D = GetComponent<Rigidbody2D>();
			_rigidbody2D.freezeRotation = true;
		}
	}
}
