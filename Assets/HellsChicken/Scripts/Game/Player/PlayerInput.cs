
using UnityEngine;


namespace HellsChicken.Scripts.Game.Player
{
    [RequireComponent(typeof(PlayerController))]

    public class PlayerInput : MonoBehaviour
    {
        private PlayerController _playerController;
        
        private float _horizontalMovement; //this will be 1 for right, -1 for left
        private bool _jump;
        private bool _glide;
        private bool _shootFlames;
        private bool _startEggAiming;
        private void Awake()
        {
            _playerController = gameObject.GetComponent<PlayerController>();
        }

        private void OnEnable()
        {

        }

        private void OnDisable()
        {
            
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        
        // Update is called once per frame
        void Update()
        {
            //_horizontalMovement = _newInputSystem.Walking.Move.ReadValue<float>();
            _horizontalMovement = Input.GetAxisRaw("Horizontal");
            if (Input.GetButtonDown("Jump"))
                _jump = true;
            if (Input.GetButton("Jump"))
                _glide = true;
            if (Input.GetButtonDown("Fire1"))
                _shootFlames = true;
            if (Input.GetButtonDown("Fire2"))
                _startEggAiming = true;
            //TODO: END EGG AIMING.
            
            //todo : check with get button up if we can detect end of gliding. 
        }
    
        private void FixedUpdate()
        {
            _playerController.MoveHorizontally(_horizontalMovement);
            if (_jump)
            {
                _playerController.Jump();
                _jump = false;
            }
            if (_glide)
            {
                _playerController.Glide();
                _glide = false;
            }
            if (_shootFlames)
            {
                _playerController.ShootFlames();
                _shootFlames = false;
            }
            if (_startEggAiming)
            {
                _playerController.StartEggAiming();
                _startEggAiming = false;
            }
        }
    }
}

