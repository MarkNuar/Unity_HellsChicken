using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform.Doors.KeyDoor
{
    public class KeyHolder : MonoBehaviour
    {
        private List<Key.KeyType> _keyList;
        private bool _open;

        private void Awake()
        {
            _keyList = new List<Key.KeyType>();
        }

        public List<Key.KeyType> GetKeyList()
        {
            return _keyList; 
        }

        private void AddKey(Key.KeyType keyType)
        {
            _keyList.Add(keyType);
            EventManager.TriggerEvent("AddKey");
        }

        private void RemoveKey(Key.KeyType keyType)
        {
            _keyList.Remove(keyType);
            EventManager.TriggerEvent("RemoveKey");
        }

        private bool ContainsKey(Key.KeyType keyType)
        {
            return _keyList.Contains(keyType);
        }

        private void OnTriggerEnter(Collider other)
        {
            Key key = other.GetComponent<Key>();
            if (key != null)
            {
                AddKey(key.GetKeyType());
                Destroy(key.gameObject);
            }

            KeyDoor keyDoor = other.GetComponent<KeyDoor>();
            
            if (keyDoor == null) 
                return;
            
            if (ContainsKey(keyDoor.GetKeyType()))
            {
                if (!keyDoor.IsOpened())
                {
                    RemoveKey(keyDoor.GetKeyType());
                    keyDoor.Open();
                    _open = true;
                }
            }

        }

        public bool Opening()
        {
            return _open;
        }
    }
}
