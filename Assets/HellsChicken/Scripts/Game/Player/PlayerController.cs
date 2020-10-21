using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace HellsChicken.Scripts.Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float _walkSpeed = 5.0f;
        [SerializeField] private float _jumpSpeed = 5.0f;
        [SerializeField] private float _gravityScale = 2.0f;
        [SerializeField] private float _fallingGravityIncrease = 1.0f;
        [SerializeField] private float _glidingUpGravityIncrease = 5.0f;
        //deprecated
        //[SerializeField] private float _glidingDownGravityDecrease = 0.1f;

        [SerializeField] private float _maxVerticalSpeed = 40f;
        [SerializeField] private float _glidingDescentFixedSpeed = 15f;
        [SerializeField] private Transform _firePosition;
        [SerializeField] private GameObject _fireStreamPrefab;
        
        private float _gravity;

        private float _horizontalMovement;
        private bool _jump = false;
        private bool _glide;

        private Vector3 _moveDirection;

        private Transform _transform;
        private CharacterController _characterController;
        private MeshRenderer _meshRenderer;

        [SerializeField] private Material _glideMaterial;
        [SerializeField] private Material _normalMaterial;

        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _moveDirection = Vector3.zero;
            _gravity = - Physics.gravity.y;
        }

        public void MoveHorizontally(float horizontalMovement)
        {
            _horizontalMovement = horizontalMovement;
        }
        public void Jump()
        {
            _jump = _characterController.isGrounded;
        }
        public void Glide()
        {
            _glide = IsFalling() && !_characterController.isGrounded;
        }
        public void ShootFlames()
        {
            Instantiate(_fireStreamPrefab, _firePosition.position, _firePosition.rotation);
            Debug.Log("Shoot flames");
        }
        
        public void StartEggAiming()
        {
            Debug.Log("Start Egg Aiming");
        }
        
        private void FixedUpdate()
        {
            
            _moveDirection.x = _horizontalMovement * _walkSpeed;
            _moveDirection.z = 0f;
            if (_characterController.isGrounded)
            {
                //If I'm on ground, then I stop gliding
                _glide = false;
                _moveDirection.y = -10f;
                if (_jump)
                {
                    _moveDirection.y = _jumpSpeed;
                    _jump = false;
                }
            }
            else //not grounded, so falling or gliding
            {
                //falling
                if (!_glide)
                {
                    _moveDirection.y -= _gravityScale * _gravity;
                    //Faster fall when going down
                    if (_moveDirection.y < 0f)
                    {
                        _moveDirection.y -= _fallingGravityIncrease * _gravityScale * _gravity;
                    }
                }
                //gliding
                else
                {
                    if (_moveDirection.y > 0)
                    {
                        //quick reduction of gravity if the player is going up
                        _moveDirection.y -= _glidingUpGravityIncrease * _gravityScale * _gravity;
                    }
                    else
                    {
                        //now that there is no upward movement, the player float down slowly.
                        _moveDirection.y = -_glidingDescentFixedSpeed;// * Random.Range(.5f, 2f);
                        //_moveDirection.y -= _glidingDownGravityDecrease * _gravityScale * _gravity * Time.fixedDeltaTime;
                    }
                    _glide = false;
                }
            }
            //Clamping _moveDirection.y at _maxVerticalSpeed
            if (Math.Abs(_moveDirection.y) > _maxVerticalSpeed)
                _moveDirection.y = Math.Sign(_moveDirection.y) * _maxVerticalSpeed;
            _characterController.Move(_moveDirection * Time.fixedDeltaTime);
            
            
            
            
            //TODO VERSION WITHOUT PLAYER INPUT, IT ONLY INCREASES COMPLEXITY
            //TODO add controls fetch in the Update function!!
            /*
            _moveDirection.x = Input.GetAxis("Horizontal") * _walkSpeed;
            _moveDirection.z = 0f;
            if (_characterController.isGrounded)
            {
                _moveDirection.y = -10f;
                if (Input.GetButtonDown("Jump"))
                {
                    _moveDirection.y = _jumpSpeed;
                    //CALL ANIMATOR FOR JUMP ANIMATION
                }
            }
            else //not grounded, so falling or gliding
            {
                if (IsFalling())
                {
                    if (!Input.GetButton("Jump"))
                    {
                        _moveDirection.y -= _fallingGravityIncrease * _gravityScale * _gravity;
                    }
                    else
                    {
                        _moveDirection.y = -_glidingDescentFixedSpeed;
                    }
                }
                else
                {
                    _moveDirection.y -= _gravityScale * _gravity;
                }
            }
            //Clamping _moveDirection.y at _maxVerticalSpeed
            if (Math.Abs(_moveDirection.y) > _maxVerticalSpeed)
                _moveDirection.y = Math.Sign(_moveDirection.y)* _maxVerticalSpeed;
            _characterController.Move(_moveDirection * Time.fixedDeltaTime);
            */
        }

        public bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

        public bool IsGliding()
        {
            return _glide;
        }
        private bool IsFalling()
        {
            return _moveDirection.y < 0;
        }
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_characterController.collisionFlags != CollisionFlags.Above) return;
            if (Vector3.Dot(hit.normal, _moveDirection) < 0)
            {
               _moveDirection -= hit.normal * Vector3.Dot( hit.normal, _moveDirection ) * _moveDirection.y;
            }
        }
    }
}