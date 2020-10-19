using HellsChicken.Scripts.Game.Input;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player
{
    [RequireComponent(typeof(PlayerController))]

    public class PlayerInput : MonoBehaviour
    {
        private PlayerController _playerController;
        private NewInputSystem _playerInputSystem;
        private float _horizontalMovement; //this will be 1 for right, -1 for left
        private bool _jump;

        private void Awake()
        {
            _playerController = gameObject.GetComponent<PlayerController>();
            _playerInputSystem = new NewInputSystem();
        }

        private void OnEnable()
        {
            _playerInputSystem.Enable();
        }

        private void OnDisable()
        {
            _playerInputSystem.Disable();
        }

        // Start is called before the first frame update
        void Start()
        {
            //Whenever the Jump is performed, the funtion Jump form the player controller is executed
            _playerInputSystem.Walking.Jump.performed += _ => _playerController.Jump();
        }

        // Update is called once per frame
        void Update()
        {
            //get input from the input system
            _horizontalMovement = _playerInputSystem.Walking.Move.ReadValue<float>();
        }
    
        private void FixedUpdate()
        {
            _playerController.MoveHorizontally(_horizontalMovement);
        }
    
    }
}

