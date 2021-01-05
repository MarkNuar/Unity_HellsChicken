﻿using System;
using System.Collections;
using EventManagerNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class EndMenu : MonoBehaviour
    {
        public GameObject eggCrosshairCanvas;
        public GameObject healthCanvas;
        public GameObject feedbackMenuUI;
        public GameObject endLevelMenuUI;
        public GameObject backGround;
    
        public TMP_InputField tmpInputField;
        private const string VideoGameName = "Hell's Chicken";

        private static bool _gameIsPaused;
        
        public Animator transition;
        public float transitionTime = 1f;

        private void Start()
        {
            _gameIsPaused = false;
        }

        public void Pause()
        {
            backGround.SetActive(true);
            endLevelMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _gameIsPaused = true;
            Cursor.visible = true;
            eggCrosshairCanvas.SetActive(false);
            //healthCanvas.SetActive(false);
        }
    
        public static bool GetGameIsPaused()
        {
            return _gameIsPaused;
        }
    
        public void QuitGame()
        {
            Application.Quit();
        }

        public void MainMenu()
        {
            LevelManager.Instance.DestroyLevelManagerInstance();
            GameAudioManager.Instance.DestroyGameAudioManagerInstance(transitionTime - 0.05f);
            Time.timeScale = 1f;
            EventManager.TriggerEvent("stopGameSoundtrack");
            StartCoroutine(LoadSceneWithFading("MainMenu")); 
        }

        public void NextLevel()
        {
            var nextLevel = LevelManager.Instance.levelNumber + 1;
            LevelManager.Instance.DestroyLevelManagerInstance();
            GameAudioManager.Instance.DestroyGameAudioManagerInstance(transitionTime - 0.05f);
            Time.timeScale = 1f;
            EventManager.TriggerEvent("stopGameSoundtrack");
            StartCoroutine(LoadSceneWithFading("Level_"+nextLevel));
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

        private IEnumerator LoadSceneWithFading(String sceneName)
        {
            transition.SetTrigger("Start");
            //EventManager.TriggerEvent("fadeOutMusic");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}
