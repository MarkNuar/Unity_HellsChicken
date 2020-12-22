using System.Collections;
using EventManagerNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class PauseMenu : MonoBehaviour
    {
        private static bool _gameIsPaused;

        public GameObject backGround;
        public GameObject pauseMenuUI;
        public GameObject commandsMenuUI;
        public GameObject feedbackMenuUI;
        public GameObject settingsMenuUI;

        public GameObject eggCrosshairCanvas;

        public TMP_InputField tmpInputField;
        private const string VideoGameName = "Hell's Chicken";
        
        private void Start()
        {
            _gameIsPaused = false;
        }

        // Update is called once per frame
        private void Update()
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
            backGround.SetActive(false);
            pauseMenuUI.SetActive(false);
            commandsMenuUI.SetActive(false);
            feedbackMenuUI.SetActive(false);
            settingsMenuUI.SetActive(false);
            Time.timeScale = 1f;
            _gameIsPaused = false;
            Cursor.visible = false;
            eggCrosshairCanvas.SetActive(true);
        }

        private void Pause()
        {
            backGround.SetActive(true);
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _gameIsPaused = true;
            Cursor.visible = true;
            eggCrosshairCanvas.SetActive(false);
        }
    
        public void QuitGame()
        {
            Application.Quit();
        }
        
        public void MainMenu()
        {
            LevelManager.Instance.DestroyLevelManagerInstance();
            GameAudioManager.Instance.DestroyGameAudioManagerInstance();
            Time.timeScale = 1f;
            _gameIsPaused = false;
            EventManager.TriggerEvent("stopGameSoundtrack");
            SceneManager.LoadScene(0); //test if it works
        }

        public static bool GetGameIsPaused()
        {
            return _gameIsPaused;
        }
        
        public void SendFeedback()
        {
            var feedback = tmpInputField.text;
            if (feedback.Length > 0)
            {
                StartCoroutine(PostFeedback(VideoGameName, feedback));
                tmpInputField.text = "";
            }
        }

        public void CloseFeedbackPanel()
        {
            tmpInputField.text = "";
        }
      
        private static IEnumerator PostFeedback(string videoGameName, string feedback) 
        {
            // https://docs.google.com/forms/d/e/1FAIpQLSdyQkpRLzqRzADYlLhlGJHwhbKZvKJILo6vGmMfSePJQqlZxA/viewform?usp=pp_url&entry.631493581=Simple+Game&entry.1313960569=Very%0AGood!

            const string url = "https://docs.google.com/forms/d/e/1FAIpQLSdyQkpRLzqRzADYlLhlGJHwhbKZvKJILo6vGmMfSePJQqlZxA/formResponse";
        
            var form = new WWWForm();

            form.AddField("entry.631493581", videoGameName);
            form.AddField("entry.1313960569", feedback);

            var www = UnityWebRequest.Post(url, form);

            yield return www.SendWebRequest();

            print(www.error);

            Debug.Log(www.isNetworkError ? www.error : "Form upload complete!");

            // at the end go back to the main menu
            //MenuManager.Instance.OpenMainMenu();
        }
    }
}
