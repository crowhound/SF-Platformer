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
		Crouching,
		Walking,
		Running,
		Jumping,
		Falling, 
		Gliding,

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
