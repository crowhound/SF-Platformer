using System;
using SF.Abilities.Characters;
using SF.AbilityModule;
using SF.Characters.Controllers;
using UnityEngine;

namespace SF.Abilities.CharacterModule
{
    public class JumpAbility : CharacterAbility
    {
        [NonSerialized] private GroundedController2D _controller;
        public JumpAbility(GameObject owner, GroundedController2D controller)
        {
            _owner = owner;

            if (controller == null)
                LoggingSystem.LogNullObject(_owner, typeof(Controller2D));

            _controller = controller;
        }

        public void Use()
        {
            if(!_controller.IsGrounded || IsEnabled == false) return;

			_controller.IsJumping = true;
            _controller.SetVerticalVelocity(_controller.CurrentPhysics.JumpHeight);
        }
    }
}
