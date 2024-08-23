using SF.Characters.Controllers;
using UnityEngine;

namespace SF.AbilityModule
{
	/// <summary>
	/// Abilities contain the data for what actions can do and how they do them.
	/// </summary>
    public class AbilityCore : MonoBehaviour, IAbility
    {
		public bool IsEnabled = true;
		public bool InitOnStart = true;
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

		/// <summary>
		///		Override this to create custom ability checking to make sure the ability can actually be used.
		/// </summary>
		/// <returns></returns>
		protected virtual bool CheckAbilityRequirements()
		{
			if(!_isInitialized || !IsEnabled) 
				return false;

			return true;
		}
	}
}
