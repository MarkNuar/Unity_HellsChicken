using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform.Doors.KeyDoor
{
    public class KeyDoor : MonoBehaviour
    {
        [SerializeField] private Key.KeyType keyType;
        
        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private float sizeOfDoor = 5;
        [SerializeField] private float amountOfDoorInWall = 0.9f;

        private Vector3 _doorCloseTarget;
        private Vector3 _doorOpenTarget;

        private float _startTime;
        private float _totalDistanceToCover;
        private bool _isOpened;
        private bool _isButtonPressed;

        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();
        }
        
        // Update is called once per frame
        void Update()
        {
            OpenDoor();
        }

        public Key.KeyType GetKeyType()
        {
            return keyType;
        }
        
        void SetInitialReferences()
        {
            Vector3 position = transform.position;
            
            _doorCloseTarget = position;
            _doorOpenTarget = new Vector3(position.x, position.y + (sizeOfDoor * amountOfDoorInWall), position.z);
            _totalDistanceToCover = Vector3.Distance(_doorCloseTarget, _doorOpenTarget);
        }

        void OpenDoor()
        {
            if (!_isOpened && _isButtonPressed)
            {
                float distanceCovered = (Time.time - _startTime) * moveSpeed;
                float fractionOfJourney = distanceCovered / _totalDistanceToCover;
                transform.position = Vector3.Lerp(transform.position, _doorOpenTarget, fractionOfJourney);

                if (Mathf.Approximately(transform.position.y, _doorOpenTarget.y))
                {
                    Debug.Log("Door Opened");
                    _isOpened = true;
                }
            }
        }

        public bool IsOpened()
        {
            return _isOpened;
        }

        public void Open()
        {
            _startTime = Time.time;
            _isButtonPressed = true;
        }
    }
}
