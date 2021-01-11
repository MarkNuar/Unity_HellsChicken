using System;
using System.Collections;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Audio;
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
        
        public GameObject transition;
        public float transitionTime = 1f;
        private Animator _transitionAnimator;
        private CanvasGroup _transitionGroup;
        
        private bool _pauseEnabled;
        
        private void Start()
        {
            _pauseEnabled = false;
            StartCoroutine(EnablePauseMenu());
            _transitionAnimator = transition.GetComponent<Animator>();
            _transitionGroup = transition.GetComponent<CanvasGroup>();
            _gameIsPaused = false;
        }

        private IEnumerator EnablePauseMenu()
        {
            yield return new WaitForSeconds(.5f);
            _pauseEnabled = true;
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && _pauseEnabled)
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
            _pauseEnabled = false;
            TimerUI.Instance.DestroyTimerUI(transitionTime - 0.05f);
            LevelManager.Instance.DestroyLevelManagerInstance();
            GameAudioManager.Instance.DestroyGameAudioManagerInstance(transitionTime - 0.05f);
            Time.timeScale = 1f;
            //EventManager.TriggerEvent("stopGameSoundtrack");
            EventManager.TriggerEvent("stopAllMusics");
            StartCoroutine(LoadSceneWithFading("MainMenu")); 
        }

        public static bool GetGameIsPaused()
        {
            return _gameIsPaused;
        }

        public void DisablePause()
        {
            StartCoroutine(DisablePauseMenu());
        }

        private IEnumerator DisablePauseMenu()
        {
            yield return new WaitForSeconds(0.5f);
            _pauseEnabled = false;
        }

        public void RestartLevel()
        {
            _pauseEnabled = false;
            var curLevel = LevelManager.Instance.levelNumber;
            TimerUI.Instance.DestroyTimerUI(transitionTime - 0.05f);
            LevelManager.Instance.DestroyLevelManagerInstance();
            GameAudioManager.Instance.DestroyGameAudioManagerInstance(transitionTime - 0.05f);
            Time.timeScale = 1f;
            EventManager.TriggerEvent("stopAllMusics");
            StartCoroutine(LoadSceneWithFading("Level_"+curLevel));
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
        
        private IEnumerator LoadSceneWithFading(String  sceneName)
        {
            _transitionGroup.blocksRaycasts = true; //disable other buttons
            _transitionAnimator.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}
