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

		#region Common Components
		protected SpriteRenderer _spriteRend;
		protected Animator _animator;
		protected Controller2D _controller;
		#endregion

		#region Character Abilities
		//public AbilityController AbilityController;
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
			// Need to debug the 40 bytes being allocated here. 
			CharacterState.MovementState = _controller switch
			{
				PlayerController playerController =>
					CharacterState.MovementState = playerController.CharacterState.MovementState,
				GroundedController2D groundedController =>
					CharacterState.MovementState = groundedController.CharacterState.MovementState,
				_ => MovementState.Idle
			};
			_animationHash = Animator.StringToHash(CharacterState.MovementState.ToString());

			if(_animator.HasState(0, _animationHash))
			{
				_animator.CrossFade(_animationHash, 0);
			}
		}


		private void OnInputMove(InputAction.CallbackContext context)
		{
			Vector2 input = context.ReadValue<Vector2>();
			_spriteRend.flipX = input.x <= 0;
		}

		private void OnEnable()
		{
			InputManager.Controls.Player.Enable();
			InputManager.Controls.Player.Move.performed += OnInputMove;
		}
		private void OnDisable()
		{
			if(InputManager.Instance == null) return;
			InputManager.Controls.Player.Move.performed -= OnInputMove;
		}
	}
}
