using SF.Managers;

using UnityEngine;

namespace SF.UI
{
    /// <summary>
    /// This is the in game menu that can be opened during playing. 
    /// For the main menu when loading up the game at the main screen look for the MainMenu scripts.
    /// </summary>
    public class GameMenu : MonoBehaviour
    {
        private Canvas _mainMenuCanvas;

        private void Awake()
        {
            _mainMenuCanvas = GetComponent<Canvas>();
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

        private void OnEnable()
        {
            GameManager.Instance.OnGameMenuOpen += OnGameMenuOpen;
            GameManager.Instance.OnGameMenuClose += OnGameMenuClosed;
        }

        private void OnDisable()
        {
            if(GameManager.Instance == null)
                return;

            GameManager.Instance.OnGameMenuOpen -= OnGameMenuOpen;
            GameManager.Instance.OnGameMenuClose -= OnGameMenuClosed;
        }
    }
}
