using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform.Doors.KeyDoor
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private KeyType keyType;

        public enum KeyType
        {
            Red,
            Yellow,
            Blue
        }

        public KeyType GetKeyType()
        {
            return keyType;
        }
    }
}
