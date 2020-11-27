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
        private bool _haveKey;

        // Start is called before the first frame update
        private void Start()
        {
            SetInitialReferences();
        }

        private void SetInitialReferences()
        {
            Vector3 doorPosition = transform.position;
            
            _doorCloseTarget = doorPosition;
            _doorOpenTarget = new Vector3(doorPosition.x, doorPosition.y + (sizeOfDoor * amountOfDoorInWall), doorPosition.z);
            _totalDistanceToCover = Vector3.Distance(_doorCloseTarget, _doorOpenTarget);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!_isOpened && _haveKey) 
                OpenDoor();
        }

        private void OpenDoor()
        {
            float distanceCovered = (Time.time - _startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / _totalDistanceToCover;
            transform.position = Vector3.Lerp(transform.position, _doorOpenTarget, fractionOfJourney);

            if (Mathf.Approximately(transform.position.y, _doorOpenTarget.y)) 
                _isOpened = true;
        }

        public Key.KeyType GetKeyType()
        {
            return keyType;
        }

        public bool IsOpened()
        {
            return _isOpened;
        }

        public void Open()
        {
            _startTime = Time.time;
            _haveKey = true;
        }
    }
}
