using SF.Characters.Controllers;

namespace SF.AIModule
{
	[System.Serializable]
	public abstract class AIActionBase : IAIAction
	{
		public string Name;
		public bool IsEnabled = true;
		protected Controller2D Controller2D;

		public abstract void Init();
		public abstract void DoAction();
	}
}
