using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

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
        healthCanvas.SetActive(false);
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
        Time.timeScale = 1f;
        _gameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1); //test if it works
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
