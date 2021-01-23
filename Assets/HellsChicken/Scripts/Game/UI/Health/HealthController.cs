using EventManagerNamespace;
using HellsChicken.Scripts.Game.UI.Menu;
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
        public Animator transition;

        public PauseMenu pauseMenuRef;
        private void OnEnable()
        {
            EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
            EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
            EventManager.StartListening("RefillPlayerHealth", RefillHealth);
            EventManager.StartListening("KillPlayer",KillPlayer);
        }

        private void Start ()
        {
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
    
        private void Updating()
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
            if(_health > 1)
                EventManager.TriggerEvent("NotLastHeart");
            if(_health == 1)
                EventManager.TriggerEvent("LastHeart");
            
            if (_health == 0)
            {
                pauseMenuRef.DisablePause();
                EventManager.TriggerEvent("chickenDeath");
                EventManager.TriggerEvent("PlayerDeath");
                transition.SetTrigger("Start");
            }
        }
    
        private void IncreaseHealth()
        {
            EventManager.StopListening("IncreasePlayerHealth",IncreaseHealth);
            if (_health < numberOfHearts)
                _health++;
            Updating();
            EventManager.StartListening("IncreasePlayerHealth",IncreaseHealth);
        }

        private void DecreaseHealth()
        {
            EventManager.StopListening("DecreasePlayerHealth",DecreaseHealth);
            if (_health > 1)
                EventManager.TriggerEvent("chickenDamage");
            if (_health > 0)
                _health--;
            Updating();
            EventManager.StartListening("DecreasePlayerHealth",DecreaseHealth);
        }
    
        private void RefillHealth()
        {
            EventManager.StopListening("RefillPlayerHealth", RefillHealth);
            _health = numberOfHearts;
            Updating();
            EventManager.StartListening("RefillPlayerHealth", RefillHealth);
        }

        private void KillPlayer()
        {
            EventManager.StopListening("KillPlayer",KillPlayer);
            _health = 0;
            Updating();
            transition.SetTrigger("Start");
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
