﻿using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.UI.Menu
{
   public class MainMenu : MonoBehaviour
   {
      public TMP_InputField tmpInputField;
      private const string VideoGameName = "Hell's Chicken";
      public Animator transition;
      public float transitionTime = 1f;

      public List<Button> levels;
      private int _levelToBeCompleted;
      public void Start()
      {
         EventManager.TriggerEvent("menuSoundtrack");
         _levelToBeCompleted = GameManager.Instance.GetLevelToBeCompleted();
         for (var i = 0; i < levels.Count; i++)
         {
            levels[i].interactable = i < _levelToBeCompleted;
         }
         
      }

      public void PlayGame()
      {
         StartCoroutine(LoadSceneWithFading(SceneManager.GetActiveScene().buildIndex + 1));
      }
      

      public void PlayLevel(int levelIndex)
      {
         StartCoroutine(LoadSceneWithFading(SceneManager.GetActiveScene().buildIndex + levelIndex));
      }

      public void QuitGame()
      {
         Application.Quit();
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

       IEnumerator LoadSceneWithFading(int index)
       {
          transition.SetTrigger("Start");
          EventManager.TriggerEvent("fadeOutMusic");
          yield return new WaitForSeconds(transitionTime);
          SceneManager.LoadScene(index);
       }
   }
}
