using UnityEngine;
using UnityEngine.InputSystem;

using SF.InputModule;

namespace SF.AbilityModule.Characters
{
    public class CrouchAbility : AbilityCore, IInputAbility
    {
        [SerializeField] private Vector2 _colliderResized = new Vector2(.5f,1.5f);

        protected override bool CheckAbilityRequirements()
        {
            if(!_isInitialized || !enabled || _controller2d == null)
                return false;

            // Only crouch if we are grounded.
            return _controller2d.IsGrounded;
        }

        private void OnInputCrouch(InputAction.CallbackContext context)
        {
            //TODO: Need to do some collider check to make sure the character is not being pushed into a ceiling or something else when uncrouching.

            if(!CheckAbilityRequirements()) return;

            _controller2d.IsCrouching = !_controller2d.IsCrouching;

            if(_controller2d.IsCrouching)
                _controller2d.ResizeCollider(_colliderResized);
            else
                _controller2d.ResetColliderSize();
        }

        private void OnEnable()
        {
            InputManager.Controls.Player.Enable();
            InputManager.Controls.Player.Crouch.performed += OnInputCrouch;
        }

        private void OnDisable()
        {
            if(InputManager.Instance == null) return;
            InputManager.Controls.Player.Crouch.performed -= OnInputCrouch;
        }
    }
}
