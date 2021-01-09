using System;
using System.Collections;
using EventManagerNamespace;
using HellsChicken.Scripts.Game;
using HellsChicken.Scripts.Game.UI.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using TMPro;

public class EndGameMenu : MonoBehaviour
{
    
    public TMP_InputField tmpInputField;
    private const string VideoGameName = "Hell's Chicken";
    
    public GameObject transition;
    public float transitionTime = 1f;
    //TODO
    private Animator _transitionAnimator;
    private CanvasGroup _transitionGroup;
    
    // Start is called before the first frame update
    void Start()
    {
        _transitionAnimator = transition.GetComponent<Animator>();
        _transitionGroup = transition.GetComponent<CanvasGroup>();
        EventManager.TriggerEvent("menuSoundtrack");
    }
    
        
    public void MainMenu()
    {
        StartCoroutine(LoadSceneWithFading("MainMenu")); 
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
        EventManager.TriggerEvent("fadeOutMusic");
        _transitionAnimator.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}
