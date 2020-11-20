using UnityEngine;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        private static bool _gameIsPaused;

        public GameObject pauseMenuUI;
        public GameObject commandsMenuUI;

        public GameObject eggCrosshairCanvas;
        
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_gameIsPaused)
                {
                    Resume();
                }

                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            pauseMenuUI.SetActive(false);
            commandsMenuUI.SetActive(false);
            Time.timeScale = 1f;
            _gameIsPaused = false;
            eggCrosshairCanvas.SetActive(true);
        }
    
        void Pause()
        {
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _gameIsPaused = true;
            eggCrosshairCanvas.SetActive(false);
        
        }
    
        public void QuitGame()
        {
            Application.Quit();
        }

        public bool getGameIsPaused()
        {
            return _gameIsPaused;
        }
    }
}
