using UnityEngine;
using UnityEngine.InputSystem;

using SF.InputModule;

namespace SF.AbilityModule.Characters
{
    public class JumpAbility : AbilityCore, IInputAbility
    {
        [Header("Jumping Physics")]
        public float JumpHeight = 12;
        public float RunningJumpMultiplier = 1.1f;
        private float CalculatedJumpHeight;
        public int JumpAmount = 1;
        public int JumpsRemaining;

        public bool CanJumpInfinitely = false;

        [Header("Jumping SFX")]
        [SerializeField] private AudioClip _jumpSFX;

        protected override void OnInitialize()
        {
            _controller2d.OnGrounded += ResetJumps;
        }

        protected override bool CheckAbilityRequirements()
        {
            if(!_isInitialized || !enabled || _controller2d == null)
                return false;

            // If we are currently gliding don't jump. 
            // Due note we could add the ability to jump in mid glide for more movement customization. Maybe a boolean in the Glide called CanGlideJump.
            if(_controller2d.IsGliding)
                return false;


            if(JumpsRemaining < 1 && !CanJumpInfinitely)
                return false;

            return true;
        }

        private void OnInputJump(InputAction.CallbackContext context)
		{
            if(!CheckAbilityRequirements()) return;

            CalculatedJumpHeight = _controller2d.IsRunning
                ? JumpHeight * RunningJumpMultiplier
                : JumpHeight;

            JumpsRemaining--;

			_controller2d.IsJumping = true;
            _controller2d.IsFalling = false;
            _controller2d.IsClimbing = false;

            if(_jumpSFX != null)
                AudioManager.Instance.PlayOneShot(_jumpSFX);

            // TODO: Only add the running height bonus to the first jump.
            _controller2d.SetVerticalVelocity(CalculatedJumpHeight);
		}

        public void ResetJumps()
        {
            JumpsRemaining = JumpAmount;
        }

        private void OnEnable()
		{
			InputManager.Controls.Player.Enable();
			InputManager.Controls.Player.Jump.performed += OnInputJump;
            
            // Have to check for null becuase you can have OnEnable run sometimes before initialization from the ability controller.
            if(_controller2d != null)
                _controller2d.OnGrounded += ResetJumps;
		}

        private void OnDisable()
		{
			if(InputManager.Controls == null) return;

			InputManager.Controls.Player.Jump.performed -= OnInputJump;
			_controller2d.OnGrounded -= ResetJumps;
		}
    }
}
