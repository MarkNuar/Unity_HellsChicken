
using UnityEngine;


namespace HellsChicken.Scripts.Game.Player
{
    [RequireComponent(typeof(PlayerController))]

    public class PlayerInput : MonoBehaviour
    {
        private PlayerController _playerController;
        private NewInputSystem _newInputSystem;
        private float _horizontalMovement; //this will be 1 for right, -1 for left
        
        private void Awake()
        {
            _playerController = gameObject.GetComponent<PlayerController>();
            _newInputSystem = new NewInputSystem();
        }

        private void OnEnable()
        {
            _newInputSystem.Enable();
        }

        private void OnDisable()
        {
            _newInputSystem.Disable();
        }

        // Start is called before the first frame update
        void Start()
        {
            //Whenever the Jump is performed, the function Jump form the player controller is executed
            _newInputSystem.Walking.Jump.performed += _ => _playerController.Jump();
            _newInputSystem.Walking.EnterEggAiming.performed += _ => _playerController.EnterEggAiming();
            _newInputSystem.Walking.ShootFlame.performed += _ => _playerController.ShootFlames();
        }

        // Update is called once per frame
        void Update()
        {
            //get input from the input system
            
            _horizontalMovement = _newInputSystem.Walking.Move.ReadValue<float>();
        }
    
        private void FixedUpdate()
        {
            _playerController.MoveHorizontally(_horizontalMovement);
        }
    
    }
}

