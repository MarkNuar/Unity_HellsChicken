using EventManagerNamespace;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int numberOfHearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite lostHeart;
    [SerializeField] [Range(10f,100f)] private float heartIconSize = 40f;
    [SerializeField] [Range(0.01f,0.1f)] private float spaceBetweenHearts = 0.1f;
    
    private Image[] _hearts;
    private void OnEnable()
    {
        EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
        EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
    }

    private void Start ()
    {
        _hearts = new Image[numberOfHearts];
        for (var i = 0; i < numberOfHearts; i++)
        {
            GameObject heartObject = new GameObject("Heart");
            Image heartImage = heartObject.AddComponent<Image>();
            heartImage.transform.SetParent(transform);
            heartImage.sprite = fullHeart;
            heartImage.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            heartImage.GetComponent<RectTransform>().sizeDelta = new Vector2(heartIconSize,heartIconSize);
            heartImage.GetComponent<RectTransform>().pivot = new Vector2(0,1);
            heartImage.GetComponent<RectTransform>().anchorMin = new Vector2(spaceBetweenHearts * i,1);
            heartImage.GetComponent<RectTransform>().anchorMax = new Vector2(spaceBetweenHearts * i,1);
            _hearts[i] = heartImage;
        }
    }
    
    private void Update()
    {
        if (health > numberOfHearts)
            health = numberOfHearts;
        if(health==0)
            EventManager.TriggerEvent("PlayerDeath");
    }

    private void OnDisable()
    {
        EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
        EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
    }

    private void IncreaseHealth()
    {
        EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
        if (health < numberOfHearts)
        {
            health++;
            _hearts[health].sprite = fullHeart;
        }
        
        EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
    }

    private void DecreaseHealth()
    {
        EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
        if (health > 0)
        {
            health--;
            _hearts[health].sprite = lostHeart;
        }
        EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
    }
    
}
