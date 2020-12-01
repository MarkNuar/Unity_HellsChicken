using System.Collections.Generic;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Platform.Doors.KeyDoor;
using UnityEngine;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.UI.Key
{
    public class KeyController : MonoBehaviour
    {
        [SerializeField] private KeyHolder keyHolder;
        private List<Platform.Doors.KeyDoor.Key.KeyType> _keyList = new List<Platform.Doors.KeyDoor.Key.KeyType>();

        private int _holdingKeys;
        [SerializeField] private int numberOfKeys = 3;
        [SerializeField] private Sprite emptyKey;
        [SerializeField] private Sprite blueKey;
        [SerializeField] private Sprite redKey;
        [SerializeField] private Sprite yellowKey;
        [SerializeField] private Image keyImage;
        private const float SpaceBetweenKeys = 0.06f;
        private Image[] _keys;
        
        private void OnEnable()
        {
            EventManager.StartListening("AddKey", AddKey);
            EventManager.StartListening("RemoveKey", RemoveKey);
        }

        private void Start()
        {
            _holdingKeys = 0; //at the beginning no keys held
            _keys = new Image[numberOfKeys];
            for (var i = 0; i < numberOfKeys; i++)
            {
                var k = Instantiate(keyImage, transform, true);
                if (k.transform is RectTransform heartImageRect)
                {
                    heartImageRect.anchoredPosition = Vector2.zero; //TODO Vector2.zero
                    heartImageRect.sizeDelta = Vector2.zero;
                    heartImageRect.anchorMin -= new Vector2(SpaceBetweenKeys, 0f) * i;
                    heartImageRect.anchorMax -= new Vector2(SpaceBetweenKeys, 0f) * i;
                }
                _keys[i] = k;
            }
            // swap array, since images are stored from right to left
            // var tempKeys = _keys;
            // for (var i = 0; i < numberOfKeys; i++)
            // {
            //     _keys[i] = tempKeys[(numberOfKeys - 1) - i];
            // }
        }

        private void Updating()
        {
            for (var i = 0; i <_holdingKeys; i++)
            {
                SetTransparency(false,_keys[i]);
                var keyType = _keyList[i];
                switch (keyType)
                {
                    case Platform.Doors.KeyDoor.Key.KeyType.Red:
                        _keys[i].sprite = redKey;
                        break;
                    case Platform.Doors.KeyDoor.Key.KeyType.Yellow:
                        _keys[i].sprite = yellowKey;
                        break;
                    case Platform.Doors.KeyDoor.Key.KeyType.Blue:
                        _keys[i].sprite = blueKey;
                        Debug.Log("ciao");
                        break;
                    default:
                        _keys[i].sprite = emptyKey;
                        break;
                }
            }
            
            for (var i = _holdingKeys; i < numberOfKeys; i++)
            {
                SetTransparency(true,_keys[i]);
                //_keys[i].sprite = emptyKey;
            }
        }

        private static void SetTransparency(bool transparent, Image image)
        {
            float alpha = transparent ? 0 : 1;
            var color = image.color;
            color.a = alpha;
            image.color = color;
        }

        private void AddKey()
        {
            EventManager.StopListening("AddKey", AddKey);
            
            _keyList = keyHolder.GetKeyList();
            if (_holdingKeys < numberOfKeys) 
                _holdingKeys++;
            Updating();
            
            EventManager.StartListening("AddKey", AddKey);
        }
        
        private void RemoveKey()
        {
            EventManager.StopListening("RemoveKey", RemoveKey);
            
            _keyList = keyHolder.GetKeyList();
            if (_holdingKeys > 0)
                _holdingKeys--;
            Updating();
            
            EventManager.StartListening("RemoveKey", RemoveKey);
        }
        
        private void OnDisable()
        {
            EventManager.StopListening("AddKey", AddKey);
            EventManager.StopListening("RemoveKey", RemoveKey);
        }
    }
}
