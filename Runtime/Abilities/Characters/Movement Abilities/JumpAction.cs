using System;
using UnityEngine;

using SF.Characters.Controllers;

namespace SF.AbilityModule.Characters
{
    public class JumpAction : CharacterAbility
    {
        public JumpingPhysics JumpingPhysics;
        [NonSerialized] private GroundedController2D _controller;
        public JumpAction(GameObject owner, GroundedController2D controller)
        {
            _owner = owner;

            if (controller == null)
                LoggingSystem.LogNullObject(_owner, typeof(Controller2D));

            _controller = controller;
            JumpingPhysics.ResetJumps();
            _controller.OnGrounded += ResetJumps;
        }
        public void Use()
        {
            if (IsEnabled == false || JumpingPhysics.JumpsRemaining < 1) return;

           JumpingPhysics.JumpsRemaining--;

			_controller.IsJumping = true;
            _controller.IsFalling = false;
            _controller.SetVerticalVelocity(JumpingPhysics.JumpHeight);
        }
        private void ResetJumps()
        {
            JumpingPhysics.ResetJumps();
        }
    }
}
