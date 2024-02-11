using System;
using SF.Abilities.Characters;
using SF.AbilityModule;
using SF.Characters.Controllers;
using SF.InputModule;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SF.Abilities.CharacterModule
{
    public class JumpAbility : AbilityCore, IInputAbility
    {
        public JumpingPhysics JumpingPhysics;
        protected override void OnInitialize()
        {
            _controller2d.OnGrounded += ResetJumps;
        }
        private void OnInputJump(InputAction.CallbackContext context)
		{
			if (IsEnabled == false || JumpingPhysics.JumpsRemaining < 1) return;

            JumpingPhysics.JumpsRemaining--;

			_controller2d.IsJumping = true;
            _controller2d.IsFalling = false;
            _controller2d.SetVerticalVelocity(JumpingPhysics.JumpHeight);
		}

		private void ResetJumps()
        {
            JumpingPhysics.ResetJumps();
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
