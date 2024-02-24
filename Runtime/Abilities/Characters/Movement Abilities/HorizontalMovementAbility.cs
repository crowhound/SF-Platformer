using UnityEngine;
using UnityEngine.InputSystem;
using SF.AbilityModule;
using SF.InputModule;
namespace SF.Abilities.Characters
{
    public class HorizontalMovementAbility : AbilityCore, IInputAbility
    {
        #region Input Actions
        private void OnInputMove(InputAction.CallbackContext context)
		{
			Vector2 input = context.ReadValue<Vector2>();

			float xDirection = input.x != 0 ? input.x : 0;
			_controller2d.Direction = new Vector2(xDirection,0);
		}
        private void OnMoveInputRun(InputAction.CallbackContext context)
        {
			_controller2d.IsRunning = context.ReadValue<float>() > 0;
			_controller2d.ReferenceSpeed = _controller2d.IsRunning
			? _controller2d.CurrentPhysics.GroundRunningSpeed
			: _controller2d.CurrentPhysics.GroundSpeed;
        }
        #endregion Input Actions
        private void OnEnable()
		{
			InputManager.Controls.Player.Enable();
			InputManager.Controls.Player.Move.performed += OnInputMove;
			InputManager.Controls.Player.Move.canceled += OnInputMove;

            InputManager.Controls.Player.Running.performed += OnMoveInputRun;
			InputManager.Controls.Player.Running.canceled += OnMoveInputRun;
		}

        private void OnDisable()
		{
			if(InputManager.Instance == null) return;

			InputManager.Controls.Player.Move.performed -= OnInputMove;
			InputManager.Controls.Player.Move.canceled -= OnInputMove;

            InputManager.Controls.Player.Running.performed -= OnMoveInputRun;
			InputManager.Controls.Player.Running.canceled -= OnMoveInputRun;
		}
	}
}