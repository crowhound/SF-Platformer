using System;

using UnityEngine;

namespace SF.Managers
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

	/// <summary>
	/// The current play state of the game loop that describes what type of logic loop is being updated.
	/// </summary>
	public enum GamePlayState
	{
		Playing = 0,
		Paused = 1,
		MainMenu = 2
	}

    [DefaultExecutionOrder(-1)]
    public class GameManager : MonoBehaviour
    {
		public GameControlState ControlState;
		public GamePlayState PlayState;

		public LivesManager LivesManager;

        public const string ManagerObjName = "Game Wide Managers";
        public const string ManagerObjTag = "Game Manager";

        public Action OnGameMenuOpen;
        public Action OnGameMenuClose;

        private static GameManager _instance;
        public static GameManager Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindFirstObjectByType<GameManager>();

                    // If no AudioManager was found in the scene make one than set it as the instance for the AudioManager.
                    if(_instance == null)
                    {
                        GameObject go = GameObject.FindGameObjectWithTag("ManagerObjTag");

                        if(go == null)
                        {
                            go = new GameObject(ManagerObjName, typeof(GameManager));
                            Instantiate(go);
                        }

                        _instance = go.GetComponent<GameManager>();
                    }
                }

                return _instance;
            }
            set
            {
                if(_instance == null)
                    _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
            Application.targetFrameRate = 60;
        }

        public void ToggleGameMenu()
        {
            if(PlayState == GamePlayState.Playing)
                GameMenuOpen();
            else
                GameMenuClose();
        }
        private void GameMenuOpen()
        {
            PlayState = GamePlayState.MainMenu;
            OnGameMenuOpen?.Invoke();
        }

        private void GameMenuClose()
        {
            PlayState = GamePlayState.Playing;
            OnGameMenuClose?.Invoke();
        }

        private void OnEnable()
		{
			LivesManager.RegisterEventListeners();
		}
        private void OnDisable ()
		{
			LivesManager.DeregisterEventListeners();
		}
	}
}
