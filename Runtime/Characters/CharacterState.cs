namespace SF.Character.Core
{
	public enum CharacterType
	{
		Player,
		Enemy,
		AI
	}
	public enum MovementState
	{
		Idle,
		Walking,
		Running,
		Jumping,
		Falling
	}
	public enum CharacterStatus
	{
		Alive,
		Dead
	}

	[System.Serializable]
	public class CharacterState
	{
		public MovementState MovementState;
		public CharacterStatus CharacterStatus;
	}
}
