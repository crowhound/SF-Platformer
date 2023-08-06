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
	public enum StatusState
	{
		Alive,
		Dead,
	}

	[System.Serializable]
	public struct CharacterState
	{
		

		public CharacterType CharacterType;
		public MovementState MovementState;
		public StatusState Status;
	}
}
