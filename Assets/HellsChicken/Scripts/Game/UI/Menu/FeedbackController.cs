using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class FeedbackController : MonoBehaviour 
    {
        private const string VideoGameName = "Hell's Chicken";
        [SerializeField] private Text feedbackText;
    
        public void SendFeedback()
        {
            var feedback = feedbackText.text;
            StartCoroutine(PostFeedback(VideoGameName, feedback));
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