using EventManagerNamespace;
using UnityEngine;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.UI.Health
{
    public class HealthController : MonoBehaviour
    {
        private int _health;
        [SerializeField] private int numberOfHearts;
        [SerializeField] private Sprite fullHeart;
        [SerializeField] private Sprite lostHeart;
        [SerializeField] private Image thighImage;
        private const float SpaceBetweenHearts = 0.06f;
        private Image[] _hearts;
        private bool _playerHasToBeKilled;
        
        private void OnEnable()
        {
            EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
            EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
            EventManager.StartListening("RefillPlayerHealth", RefillHealth);
            EventManager.StartListening("KillPlayer",KillPlayer);
        }

        private void Start ()
        {
            _playerHasToBeKilled = false;
            _health = numberOfHearts;
            _hearts = new Image[numberOfHearts];
            for (var i = 0; i < numberOfHearts; i++)
            {
                var heartImage = Instantiate(thighImage, transform, true);
                if (heartImage.transform is RectTransform heartImageRect)
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
        
            for (var i = 0; i < _health; i++)
            {
                _hearts[i].sprite = fullHeart;
            }

            for (var i = _health; i < numberOfHearts; i++)
            {
                _hearts[i].sprite = lostHeart;
            }
        
            if(_health == 1)
                EventManager.TriggerEvent("LastHeart");
            
            if (_health == 0 || _playerHasToBeKilled)
            {
                EventManager.TriggerEvent("PlayerDeath");
                _playerHasToBeKilled = false;
            }
        }
    
        private void IncreaseHealth()
        {
            EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
            if (_health < numberOfHearts)
            {
                _health++;
                //_hearts[_health].sprite = fullHeart;
            }
            EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
        }

        private void DecreaseHealth()
        {
            EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
            if (_health > 0)
            {
                _health--;
                //_hearts[_health].sprite = lostHeart;
            }
            EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
        }
    
        private void RefillHealth()
        {
            EventManager.StopListening("RefillPlayerHealth", RefillHealth);
            _health = numberOfHearts;
            // foreach (var heart in _hearts)
            // {
            //     heart.sprite = fullHeart;
            // }
            EventManager.StartListening("RefillPlayerHealth", RefillHealth);
        }

        private void KillPlayer()
        {
            EventManager.StopListening("KillPlayer",KillPlayer);
            _playerHasToBeKilled = true;
            EventManager.StartListening("KillPlayer",KillPlayer);
        }
    
        private void OnDisable()
        {
            EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
            EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
            EventManager.StopListening("RefillPlayerHealth", RefillHealth);
            EventManager.StopListening("KillPlayer",KillPlayer);
        }
    }
}
