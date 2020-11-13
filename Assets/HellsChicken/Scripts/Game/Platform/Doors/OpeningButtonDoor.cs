using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform.Doors
{
    public class OpeningButtonDoor : MonoBehaviour
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
        private float _timer = 5;

        private float buttonHeight = 0.1f;


        // Start is called before the first frame update
        void Start()
        {
            SetInitialReferences();
        }

        // Update is called once per frame
        void Update()
        {
            OpenDoor();
            CloseDoor();

            /*
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            */
        }

        void SetInitialReferences()
        {
            _doorCloseTarget = door.localPosition;

            _doorOpenTarget = new Vector3(door.localPosition.x, door.localPosition.y + (sizeOfDoor * amountOfDoorInWall), door.localPosition.z);

            _totalDistanceToCover = Vector3.Distance(_doorCloseTarget, _doorOpenTarget);
        }

        void OpenDoor()
        {
            if (!_isOpened && _isButtonPressed)
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
        }

        void CloseDoor()
        {
            if (_isOpened && _timer <= 0)
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

        /*
        private void OnTriggerExit(Collider other)
        {
            if (other.transform.CompareTag("Player"))
            {
                transform.position += new Vector3(0, buttonHeight, 0);
                _timer = 5;
            }
        }
        */
    }
}
