using System;
using SF.Abilities.Characters;
using SF.Characters.Controllers;
using UnityEngine;

namespace SF.Abilities.CharacterModule
{
    [Serializable]
    public class JumpAbility : CharacterAbility
    {
        [NonSerialized] private GroundedController2D _controller;
        public JumpAbility(GameObject owner, GroundedController2D controller)
        {
            _owner = owner;

            if (controller == null)
                LoggingSystem.LogNullObject(_owner, typeof(Controller2D));

            _controller = controller;
            _controller.CurrentPhysics.ResetJumps();
            _controller.OnGrounded += ResetJumps;
        }
        public void Use()
        {
            if (IsEnabled == false || _controller.CurrentPhysics.JumpsRemaining < 1) return;

            _controller.CurrentPhysics.JumpsRemaining--;

			_controller.IsJumping = true;
            _controller.IsFalling = false;
            _controller.SetVerticalVelocity(_controller.CurrentPhysics.JumpHeight);
        }
        private void ResetJumps()
        {
            _controller.CurrentPhysics.ResetJumps();
        }
    }
}
