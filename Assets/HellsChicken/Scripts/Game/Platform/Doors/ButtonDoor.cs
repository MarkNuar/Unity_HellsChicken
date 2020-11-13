using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform.Doors
{
    public class ButtonDoor : MonoBehaviour
    {
        [SerializeField] private Transform door;

        [SerializeField] private float moveSpeed = 1;
        [SerializeField] private float sizeOfDoor = 5;
        [SerializeField] private float amountOfDoorInWall = 0.9f;

        private Vector3 _doorCloseTarget;
        private Vector3 _doorOpenTarget;

        private float _startTime;
        private float _totalDistanceToCover;
        private bool _isOpened;
        private bool _isButtonPressed;
        
        [SerializeField] private float timer = 5;
        private float _countdown;

        private float buttonHeight = 0.1f;


        // Start is called before the first frame update
        void Start()
        {
            _countdown = timer;
            SetInitialReferences();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_isOpened)
            {
                if (_isButtonPressed)
                { 
                    OpenDoor();
                }
            }
            else
            {
                _countdown -= Time.deltaTime;
                if (_countdown <= 0f)
                {
                    Debug.Log(_startTime);
                    transform.position += new Vector3(0, buttonHeight, 0);
                    _isButtonPressed = false;
                    CloseDoor();
                    _countdown = timer;
                }
            }
        }

        void SetInitialReferences()
        {
            Vector3 doorPosition = door.localPosition;
            
            _doorCloseTarget = doorPosition;
            _doorOpenTarget = new Vector3(doorPosition.x, doorPosition.y + (sizeOfDoor * amountOfDoorInWall), doorPosition.z);
            _totalDistanceToCover = Vector3.Distance(_doorCloseTarget, _doorOpenTarget);
        }

        void OpenDoor()
        {
            float distanceCovered = (Time.time - _startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / _totalDistanceToCover;
            door.localPosition = Vector3.Lerp(door.localPosition, _doorOpenTarget, fractionOfJourney);

            if (Mathf.Approximately(door.localPosition.y, _doorOpenTarget.y))
            { 
                Debug.Log("Door Opened");
                _isOpened = true;
            }
        }

        void CloseDoor()
        {
            float distanceCovered = (Time.time - _startTime) * moveSpeed;
            float fractionOfJourney = distanceCovered / _totalDistanceToCover;
            door.localPosition = Vector3.Lerp(door.localPosition, _doorCloseTarget, fractionOfJourney);

            if (Mathf.Approximately(door.localPosition.y, _doorCloseTarget.y))
            {
                Debug.Log("Door Closed");
                _isOpened = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                transform.position -= new Vector3(0, buttonHeight, 0);

                if (!_isOpened)
                {
                    _startTime = Time.time;
                    _isButtonPressed = true;
                }
            }
        }
        
    }
}
