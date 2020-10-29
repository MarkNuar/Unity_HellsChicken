using EventManagerNamespace;
using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{
    private int _health;
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
        _health = numberOfHearts;
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
        if (_health > numberOfHearts)
            _health = numberOfHearts;
        if(_health == 1)
            EventManager.TriggerEvent("LastHeart");
        if (_health == 0)
        {
            //Debug.Log("Ready to respawn at "+ GameManager.Instance.GetCurrentCheckPointPos());
            EventManager.TriggerEvent("PlayerDeath");
        }
    }
    
    private void IncreaseHealth()
    {
        EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
        if (_health < numberOfHearts)
        {
            _health++;
            _hearts[_health].sprite = fullHeart;
        }
        EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
    }

    private void DecreaseHealth()
    {
        EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
        if (_health > 0)
        {
            _health--;
            _hearts[_health].sprite = lostHeart;
        }
        EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
    }
    private void OnDisable()
    {
        EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
        EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
    }

    private void RefillHealth()
    {
        _health = numberOfHearts;
        foreach (var heart in _hearts)
        {
            heart.sprite = fullHeart;
        }
    }
    
}
