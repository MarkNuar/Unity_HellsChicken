using System;
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
            _keys = new Image[numberOfKeys];
            for (int i = 0; i < numberOfKeys; i++)
            {
                var k = Instantiate(keyImage, transform, true);
                if (k.transform is RectTransform heartImageRect)
                {
                    heartImageRect.anchoredPosition = new Vector2(1000, 0); //TODO Vector2.zero
                    heartImageRect.sizeDelta = Vector2.zero;
                    heartImageRect.anchorMin += new Vector2(SpaceBetweenKeys, 0f) * i;
                    heartImageRect.anchorMax += new Vector2(SpaceBetweenKeys, 0f) * i;
                }
                _keys[i] = keyImage;
            }
        }

        private void Update()
        {
            for (int i = 0; i < _holdingKeys; i++)
            {
                Platform.Doors.KeyDoor.Key.KeyType keyType = _keyList[i];
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
                        break;
                }
            }

            for (int i = _holdingKeys; i < numberOfKeys; i++)
            {
                _keys[i].sprite = emptyKey;
            }
        }

        private void AddKey()
        {
            EventManager.StopListening("AddKey", AddKey);
            
            //Instantiate current key list
            _keyList = keyHolder.GetKeyList();
            if (_holdingKeys < numberOfKeys) 
                _holdingKeys++;
            
            EventManager.StartListening("AddKey", AddKey);
        }
        
        private void RemoveKey()
        {
            EventManager.StopListening("RemoveKey", RemoveKey);
            
            //Instantiate current key list
            _keyList = keyHolder.GetKeyList();
            if (_holdingKeys > 0)
                _holdingKeys--;
            
            EventManager.StartListening("RemoveKey", RemoveKey);
        }
        
        private void OnDisable()
        {
            EventManager.StopListening("AddKey", AddKey);
            EventManager.StopListening("RemoveKey", RemoveKey);
        }
    }
}
