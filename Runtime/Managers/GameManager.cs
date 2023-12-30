using UnityEngine;
using UnityEngine.Events;

namespace SF
{
	/// <summary>
	/// The current state that is controlling the games input and actions. 
	/// </summary>
	public enum GameControlState
	{
		Player,
		SceneChanging,
		Cutscenes,
		CameraTransition,
		TransformTransition // Player being moved within a scene, but has no control over the player. Think teleporting.
	}

	public class GameController
	{
		#region Unity Events
		public UnityEvent OnGameControlChange;
		#endregion

		public static GameControlState CurentControlState { get; private set; }

		public static GameControlState PreviousControlState{ get; private set; }

		public static void ChangeControlState(GameControlState gameControlState)
		{
			if (gameControlState == CurentControlState) return;

			PreviousControlState = CurentControlState;
			CurentControlState = gameControlState;
		}
	}

    public class GameManager : MonoBehaviour
    {
		public GameController GameController;
	}
}
