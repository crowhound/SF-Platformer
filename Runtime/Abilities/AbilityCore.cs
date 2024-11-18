using UnityEngine;

using SF.Characters;
using SF.Characters.Controllers;

namespace SF.AbilityModule
{
	/// <summary>
	/// Abilities contain the data for what actions can do and how they do them.
	/// </summary>
    public abstract class AbilityCore : MonoBehaviour, IAbility
    {
		public MovementState BlockingMovementStates;
		public CharacterStatus BlockingCharacterStatus = CharacterStatus.Dead;

		public bool IsEnabled = true;
		public bool InitOnStart = true;
		public bool DoOnUpdate = false;
		[SerializeField] protected bool _isInitialized = false;
		


		protected GroundedController2D _controller2d;


		public virtual void Initialize(Controller2D controller2D = null)
		{
			if (_isInitialized)
				return;

			_controller2d = controller2D as GroundedController2D;

			OnInitialize();

			_isInitialized = true;
		}
		protected virtual void OnInitialize()
		{

		}
		public void PreUpdate() 
		{ 
			OnPreUpdate();
		}
		protected virtual void OnPreUpdate()
		{

		}
		public void UpdateAbility() 
		{
			OnUpdate();
		}
		protected virtual void OnUpdate()
		{

		}
		public void PostUpdate() 
		{
			OnPostUpdate();
		}
		protected virtual void OnPostUpdate()
		{

		}

		protected virtual void OnAbilityInteruption()
		{

		}

		/// <summary>
		/// Is there any state for the controller, character or ability that blocks the start of the ability.
		/// </summary>
		/// <returns></returns>
		protected bool CanStartAbility()
		{
            if(!_isInitialized || !IsEnabled || _controller2d == null)
                return false;

            // If we are in a blocking movement state or blocking movement status don't start ability.
            if((_controller2d.CharacterState.MovementState & BlockingMovementStates) > 0
                || (_controller2d.CharacterState.CharacterStatus & BlockingCharacterStatus) > 0)
                return false;

			return CheckAbilityRequirements();
        }

		/// <summary>
		///		Override this to create custom ability checking to make sure the ability can actually be used.
		/// </summary>
		/// <returns></returns>
		protected virtual bool CheckAbilityRequirements()
		{
			return true;
		}
	}
}
