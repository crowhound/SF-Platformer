using SF.Events;
using SF.InputModule;
using SF.Managers;

using UnityEngine;
using UnityEngine.UI;

namespace SF.UI
{
    /// <summary>
    /// This is the in game menu that can be opened during playing. 
    /// For the main menu when loading up the game at the main screen look for the MainMenu scripts.
    /// </summary>
    public class GameMenu : MonoBehaviour, EventListener<GameMenuEvent>
    {
        private Canvas _mainMenuCanvas;
        
        [SerializeField] private Button _exitGameButton;

        private void Awake()
        {
            _mainMenuCanvas = GetComponent<Canvas>();
        }

        private void OnExitGameBtnClicked()
        {
            ApplicationEvent.Trigger(ApplicationEventTypes.ExitApplication);
        }

        private void OnGameMenuOpen()
        {
            if(_mainMenuCanvas == null)
            {             
                Debug.LogWarning("Failed to open the in game menu. There was no canvas assigned for the Game Menu script", this.gameObject);
                return;
            }

            _mainMenuCanvas.enabled = true;
        }
        private void OnGameMenuClosed()
        {
            if(_mainMenuCanvas == null)
            {
                Debug.LogWarning("Failed to close the in game menu. There was no canvas assigned for the Game Menu script", this.gameObject);
                return;
            }

            _mainMenuCanvas.enabled = false;
        }

        public void OnEvent(GameMenuEvent eventType)
        {
            switch(eventType.EventType)
            {
                case GameMenuEventTypes.OpenGameMenu:
                    {
                        OnGameMenuOpen();
                        break;
                    }
                case GameMenuEventTypes.CloseGameMenu:
                    {
                        OnGameMenuClosed();
                        break;
                    }
            }
        }

        private void OnEnable()
        {
            if(_exitGameButton != null)
                _exitGameButton.onClick.AddListener(OnExitGameBtnClicked);

            this.EventStartListening<GameMenuEvent>();
        }

        private void OnDisable()
        {
            if(_exitGameButton != null)
                _exitGameButton.onClick.RemoveAllListeners();

            this.EventStopListening<GameMenuEvent>();
        }
    }
}
