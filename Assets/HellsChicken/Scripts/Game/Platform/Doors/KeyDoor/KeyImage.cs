using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HellsChicken.Scripts.Game.Platform.Doors.KeyDoor
{
    public class KeyImage : MonoBehaviour
    {
        [SerializeField] private KeyHolder keyHolder;
        
        [SerializeField] private Transform container;
        [SerializeField] private Transform keyTemplate;
        
        [SerializeField] private Image keyImage;

        private void Awake()
        {
            keyTemplate.gameObject.SetActive(false);
        }

        private void Start()
        {
            //keyHolder.OnKeysChanged += KeyHolder_OnKeysChanged;
        }

        private void KeyHolder_OnKeysChanged(object sender, EventArgs e)
        {
            UpdateVisual();
        }
        
        private void UpdateVisual()
        {
            //Instantiate current key list
            List<Key.KeyType> keyList = keyHolder.GetKeyList();
            for (int i = 0; i < keyList.Count; i++)
            {
                Key.KeyType keyType = keyList[i];
                
                keyTemplate.gameObject.SetActive(true);
                keyTemplate.position = new Vector2(50 * i + 20, 50);
                
                keyImage.GetComponent<Image>();
                switch (keyType)
                {
                    case Key.KeyType.Red:
                        keyImage.color = Color.red;
                        break;
                    case Key.KeyType.Yellow:
                        keyImage.color = Color.yellow;
                        break;
                    case Key.KeyType.Blue:
                        keyImage.color = Color.blue;
                        break;
                }
            }
        }

        private void Update()
        {
            //Clean up old keys
            foreach (Transform child in container)
            {
                if(child == keyTemplate && keyHolder.Opening())
                    child.gameObject.SetActive(false);
            }
        }
    }
}
