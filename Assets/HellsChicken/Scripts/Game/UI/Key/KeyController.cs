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
        [SerializeField] private Image keyImage;
        private const float SpaceBetweenKeys = 0.06f;
        
        private void OnEnable()
        {
            EventManager.StartListening("AddKey", AddKey);
            EventManager.StartListening("RemoveKey", RemoveKey);
        }
        
        private void Update()
        {
            for (int i = 0; i < _holdingKeys; i++)
            {
                var k = Instantiate(keyImage, transform, true);
                if (k.transform is RectTransform heartImageRect)
                {
                    heartImageRect.anchoredPosition = new Vector2(1000, 0); //TODO Vector2.zero
                    heartImageRect.sizeDelta = Vector2.zero;
                    heartImageRect.anchorMin += new Vector2(SpaceBetweenKeys, 0f) * i;
                    heartImageRect.anchorMax += new Vector2(SpaceBetweenKeys, 0f) * i;
                }

                if (i < _keyList.Count)
                {
                    Platform.Doors.KeyDoor.Key.KeyType keyType = _keyList[i];
                    switch (keyType)
                    {
                        case Platform.Doors.KeyDoor.Key.KeyType.Red:
                            k.color = Color.red;
                            break;
                        case Platform.Doors.KeyDoor.Key.KeyType.Yellow:
                            k.color = Color.yellow;
                            break;
                        case Platform.Doors.KeyDoor.Key.KeyType.Blue:
                            k.color = Color.blue;
                            break;
                    }
                }
                else
                {
                    k.color = Color.yellow; //
                }
            }
        }

        private void AddKey()
        {
            EventManager.StopListening("AddKey", AddKey);
            //Instantiate current key list
            _keyList = keyHolder.GetKeyList();
            _holdingKeys++;
            EventManager.StartListening("AddKey", AddKey);
        }
        
        private void RemoveKey()
        {
            EventManager.StopListening("RemoveKey", RemoveKey);
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
