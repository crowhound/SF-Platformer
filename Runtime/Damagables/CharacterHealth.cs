using SF.Characters.Controllers;
using SF.Characters;
using UnityEngine;

namespace SF.SpawnModule
{
    public class CharacterHealth : Health, IDamagable
    {
        [Header("Animation Setting")]

        [Tooltip("If you want to force an animation state when this object is damaged than set this string to the name of the animation state.")]
        public const string HitAnimationName = "Damaged";
        public readonly int HitAnimationHash = Animator.StringToHash(HitAnimationName);

        public const string DeathAnimationName = "Death";
        public readonly int DeathAnimationHash = Animator.StringToHash(DeathAnimationName);

        public float HitAnimationDuration = 0.3f;

        protected Controller2D _controller;
        protected Character2D _character2D;

        protected void Awake()
        {
            _controller = GetComponent<Controller2D>();
            _character2D = GetComponent<Character2D>();
        }

        protected override void Kill()
        {
            base.Kill();

            if(_controller != null)
                _controller.CharacterState.CharacterStatus = CharacterStatus.Dead;

            if(_character2D != null && !string.IsNullOrEmpty(DeathAnimationName))
                _character2D.SetAnimationState(DeathAnimationName);
        }

        protected override void Respawn()
        {
            if(_controller != null)
            {
                _controller.Reset();
                _controller.CharacterState.CharacterStatus = CharacterStatus.Alive;
            }

            base.Respawn();
        }

        public override void TakeDamage(int damage)
        {
            if(_character2D != null && !string.IsNullOrEmpty(HitAnimationName))
                _character2D.SetAnimationState(HitAnimationName, HitAnimationDuration);

            base.TakeDamage(damage);
        }
    }
}
