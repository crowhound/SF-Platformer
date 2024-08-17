using SF.AbilityModule;
using SF.InputModule;

using UnityEngine;
using UnityEngine.InputSystem;

namespace SF.Abilities.CharacterModule
{
    public class JumpAbility : AbilityCore, IInputAbility
    {
        [Header("Jumping Phjysics")]
        public float JumpHeight = 12;
        public int JumpAmount = 1;
        public int JumpsRemaining;

        protected override void OnInitialize()
        {
            _controller2d.OnGrounded += ResetJumps;
        }
        private void OnInputJump(InputAction.CallbackContext context)
		{
			if (IsEnabled == false || JumpsRemaining < 1) return;

            JumpsRemaining--;

			_controller2d.IsJumping = true;
            _controller2d.IsFalling = false;
            _controller2d.SetVerticalVelocity(JumpHeight);
		}

        public void ResetJumps()
        {
            JumpsRemaining = JumpAmount;
        }

        private void OnEnable()
		{
			InputManager.Controls.Player.Enable();
			InputManager.Controls.Player.Jump.performed += OnInputJump;
		}

        private void OnDisable()
		{
			if(InputManager.Instance == null) return;

			InputManager.Controls.Player.Jump.performed -= OnInputJump;
			_controller2d.OnGrounded -= ResetJumps;
		}
    }
}
