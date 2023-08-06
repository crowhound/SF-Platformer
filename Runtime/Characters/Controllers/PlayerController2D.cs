using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using UnityEngine;
using UnityEngine.InputSystem;

using SF.InputSystem;

namespace SF.Characters.Controllers
{
	[RequireComponent(typeof(BoxCollider2D))]
    public class PlayerController : GroundedController2D
    {
		public float JumpInput;

		#region Life cycles
		protected override void OnAwake()
		{
			_boxCollider = GetComponent<BoxCollider2D>();
		}
		protected override void OnPreFixedUpdate()
		{
			_boundsData.Bounds = _boxCollider.bounds;
			JumpInput = InputManager.Controls.Player.Jump.ReadValue<float>();
		}

		private void OnEnable()
		{
			InputManager.Controls.Player.Enable();
			InputManager.Controls.Player.Move.performed += OnInputMove;
			InputManager.Controls.Player.Move.canceled += OnInputMove;
			InputManager.Controls.Player.Jump.performed += OnInputJump;
		}
		private void OnDisable()
		{
			if(InputManager.Instance == null) return;

			InputManager.Controls.Player.Move.performed -= OnInputMove;
			InputManager.Controls.Player.Move.canceled -= OnInputMove;
			InputManager.Controls.Player.Jump.performed -= OnInputJump;
		}
		#endregion
		public void UpdateBounds()
		{
			if(_boxCollider != null)
				_boundsData.UpdateBounds(_boxCollider);
		}
		#region Input Actions
		private void OnInputMove(InputAction.CallbackContext context)
		{
			Vector2 input = context.ReadValue<Vector2>();

			Direction.x = input.x != 0 ? input.x : 0;
		}
		private void OnInputJump(InputAction.CallbackContext context)
		{
			if(!IsGrounded) return;

			IsJumping = true;
			SetVerticalVelocity(CurrentPhysics.JumpHeight);
		}
		#endregion
		
#if UNITY_EDITOR
		public void OnDrawGizmos()
		{
			if(!IsDebugModeActivated) return;

			_boxCollider = (_boxCollider == null) ? GetComponent<BoxCollider2D>() : _boxCollider;
			_boundsData.Bounds = _boxCollider.bounds;
			Vector2 startPosition;
			float stepPercent;
			int numberOfRays = CollisionController.VerticalRayAmount;
			Vector2 origin = _boundsData.BottomLeft;
			Vector2 end = _boundsData.BottomRight;
			List<Vector3> listOfPoints = new();

			for(int x = 0; x < numberOfRays; x++) // Down
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				listOfPoints.Add(startPosition);
				listOfPoints.Add(startPosition - new Vector2(0, CollisionController.VerticalRayDistance));
			}

			numberOfRays = CollisionController.HoriztonalRayAmount;
			origin = _boundsData.TopRight; 
			end = _boundsData.BottomRight;

			for(int x = 0; x < numberOfRays; x++) // Right
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				listOfPoints.Add(startPosition);
				listOfPoints.Add(startPosition + new Vector2(CollisionController.HoriztonalRayDistance,0));
			}

			ReadOnlySpan<Vector3> pointsAsSpan = CollectionsMarshal.AsSpan(listOfPoints);
			Gizmos.DrawLineList(pointsAsSpan);
		}
#endif
	}
}