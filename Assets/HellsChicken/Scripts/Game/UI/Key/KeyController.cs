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
        
        [SerializeField] private Sprite key;
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
            _keys = new Image[numberOfKeys];
            
            for (var i = 0; i < numberOfKeys; i++)
            {
                var k = Instantiate(keyImage, transform, true);
                if (k.transform is RectTransform heartImageRect)
                {
                    heartImageRect.anchoredPosition = new Vector2(1000, 0);
                    heartImageRect.sizeDelta = Vector2.zero;
                    heartImageRect.anchorMin += new Vector2(SpaceBetweenKeys, 0f) * i;
                    heartImageRect.anchorMax += new Vector2(SpaceBetweenKeys, 0f) * i;
                }
                _keys[i] = k;
                _keys[i].color = Color.clear;
            }
        }
        
        private void AddKey()
        {
            EventManager.StopListening("AddKey", AddKey);
            //Instantiate current key list
            _keyList = keyHolder.GetKeyList();
            
            Platform.Doors.KeyDoor.Key.KeyType keyType = _keyList[_holdingKeys];
                
            _keys[_holdingKeys].sprite = key;
            switch (keyType)
            {
                case Platform.Doors.KeyDoor.Key.KeyType.Red:
                    _keys[_holdingKeys].color = Color.red;
                    break;
                case Platform.Doors.KeyDoor.Key.KeyType.Yellow:
                    _keys[_holdingKeys].color = Color.yellow;
                    break;
                case Platform.Doors.KeyDoor.Key.KeyType.Blue:
                    _keys[_holdingKeys].color = Color.blue;
                    break;
            }
            
            if (_holdingKeys < numberOfKeys) 
                _holdingKeys++;
            
            EventManager.StartListening("AddKey", AddKey);
        }
        
        private void RemoveKey()
        {
            EventManager.StopListening("RemoveKey", RemoveKey);
            if(_holdingKeys > 0) 
                _holdingKeys--;
            
            //Instantiate current key list
            _keyList = keyHolder.GetKeyList();
            EventManager.StartListening("RemoveKey", RemoveKey);
        }
        
        private void OnDisable()
        {
            EventManager.StartListening("AddKey", AddKey);
            EventManager.StartListening("RemoveKey", RemoveKey);
        }
    }
}
