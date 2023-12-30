namespace SF.AbilityModule
{
	/// <summary>
	/// Abilities contain the data for what actions can do and how they do them.
	/// </summary>
	[System.Serializable]
    public class AbilityCore : IAbility
    {
		public AbilityCore()
		{
			
		}
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
