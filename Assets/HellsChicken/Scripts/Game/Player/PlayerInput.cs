
using UnityEngine;
using UnityEngine.InputSystem.Interactions;


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
            _newInputSystem.Walking.JumpAndGlide.performed += ctx =>
            {
                if (_playerController.IsGrounded())
                {
                    _playerController.Jump();
                }
                else if (!_playerController.IsGliding())
                {
                    _playerController.StartGliding();
                }
                else
                {
                    _playerController.StopGliding();
                }
            };
            
            //TODO: DISTINGUISH BETWEEN PRESS AND RELEASE
            //ON RELEASE GET THE MOUSE POSITION TO COMPUTE THE VECTOR 2 DIRECTION. 
            _newInputSystem.Walking.EggAiming.performed += _ => _playerController.StartEggAiming();
            _newInputSystem.Walking.ShootFlame.performed += _ => _playerController.ShootFlames();
        }

        
        // Update is called once per frame
        void Update()
        {
            _horizontalMovement = _newInputSystem.Walking.Move.ReadValue<float>();
        }
    
        private void FixedUpdate()
        {
            _playerController.MoveHorizontally(_horizontalMovement);
        }
    
    }
}

