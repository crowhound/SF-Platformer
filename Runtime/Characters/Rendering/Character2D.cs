using System;

using SF.Abilities.Characters;
using SF.Character.Core;
using SF.Characters.Controllers;
using SF.InputModule;

using UnityEngine;
using UnityEngine.InputSystem;

namespace SF.Characters
{
	[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
    public class Character2D : MonoBehaviour
    {
		public enum CharacterTypes { Player, Ally, Enemy, NPC}
		public CharacterTypes CharacterType = CharacterTypes.Player;
		public CharacterState CharacterState;

		public bool FacingRight = true;
		#region Common Components
		protected SpriteRenderer _spriteRend;
		protected Animator _animator;
		protected Controller2D _controller;
		#endregion

		private int _animationHash;

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
		protected virtual void UpdateAnimator()
		{
			SetAnimations();
		}
		protected virtual void SetAnimations()
		{
			CharacterState.MovementState = _controller.CharacterState.MovementState;

			_animationHash = Animator.StringToHash(CharacterState.MovementState.ToString());

			if(_animator.HasState(0, _animationHash))
			{
				_animator.CrossFade(_animationHash, 0);
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
