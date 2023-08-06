using UnityEngine;

namespace SF.Abilities.Characters
{
    public class AbilityCore : MonoBehaviour
    {
		public GameObject User;
		public virtual void Initialize()
		{
			OnInitialize();
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
	}
}
