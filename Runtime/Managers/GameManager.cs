using UnityEngine;

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

    public class GameManager : MonoBehaviour
    {
		public LivesManager LivesManager;

		private void OnEnable()
		{
			LivesManager.RegisterEventListeners();
		}
		private void OnDestroy()
		{
			LivesManager.DeregisterEventListeners();
		}
	}
}
