using SF.Character.Core;
using SF.Characters.Controllers;

using UnityEngine;

namespace SF.Characters
{
	[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class Character2D : MonoBehaviour
    {
		public enum CharacterTypes { Player, Ally, Enemy, NPC}
		public CharacterTypes CharacterType = CharacterTypes.Player;
		public CharacterState CharacterState => _controller.CharacterState;

		public bool FacingRight = true;
		#region Common Components
		private SpriteRenderer _spriteRend;
		private Animator _animator;
		private Controller2D _controller;
		#endregion

		private int AnimationHash => Animator.StringToHash(CharacterState.MovementState.ToString());

		#region Lifecycle Functions  
		private void Awake()
		{
			_animator = GetComponent<Animator>();
			_spriteRend = GetComponent<SpriteRenderer>();
			_controller = GetComponent<Controller2D>();
			Init();
		}
		#endregion
		private void Init()
		{
			OnInit();
		}
		
		protected virtual void OnInit()
		{
			_controller.OnDirectionChanged += OnDirectionChanged;
		}

		private void LateUpdate()
		{
			UpdateAnimator(); 
		}
		private void UpdateAnimator()
		{
			SetAnimations();
		}
		private void SetAnimations()
		{
			if(_animator.HasState(0, AnimationHash))
			{
				_animator.CrossFade(AnimationHash, 0);
			}
		}

		private void SpriteFlip(Vector2 direction)
		{
			direction *= (FacingRight) ? 1 : -1;
			_spriteRend.flipX = direction.x <= 0;
		}

		private void OnDirectionChanged(object sender, Vector2 direction)
		{
			SpriteFlip(direction);
		}
	}
}
