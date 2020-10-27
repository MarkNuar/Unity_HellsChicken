using EventManagerNamespace;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int numberOfHearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite lostHeart;
    [SerializeField] private Image heartImage;
    private const float SpaceBetweenHearts = 0.06f;
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
            var heartImage = Instantiate(this.heartImage, transform, true);
            RectTransform heartImageRect = heartImage.transform as RectTransform;
            if (!(heartImageRect is null))
            {
                heartImageRect.anchoredPosition = Vector2.zero;
                heartImageRect.sizeDelta = Vector2.zero;
                heartImageRect.anchorMin += new Vector2(SpaceBetweenHearts, 0f) * i;
                heartImageRect.anchorMax += new Vector2(SpaceBetweenHearts, 0f) * i;
            }
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
