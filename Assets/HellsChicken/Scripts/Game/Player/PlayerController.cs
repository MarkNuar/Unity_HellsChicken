using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
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
        
        private const float Gravity = 9.81f;

        private float _horizontalMovement;
        private bool _jump = false;
        private bool _isGliding;
        
        
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
            _isGliding = false;
        }

        public void MoveHorizontally(float horizontalMovement)
        {
            _horizontalMovement = horizontalMovement;
        }

        public void Jump()
        {
            //_jump = _characterController.isGrounded;
            _jump = true;
        }
        
        public void StartGliding()
        {
            _meshRenderer.material = _glideMaterial;
            _isGliding = true;
        }

        public void StopGliding()
        {
            _meshRenderer.material = _normalMaterial;
            _isGliding = false;
        }
        public void ShootFlames()
        {
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
                StopGliding();
                _moveDirection.y = -10f;
                if (_jump)
                {
                    _moveDirection.y = _jumpSpeed;
                    _jump = false;
                }
            }
            else //not grounded, so maybe jumping or gliding
            {
                //falling
                if (!_isGliding)
                {
                    _moveDirection.y -= _gravityScale * Gravity * Time.fixedDeltaTime;
                    //Faster fall when going down
                    if (_moveDirection.y < 0f)
                    {
                        _moveDirection.y -= _fallingGravityIncrease * _gravityScale * Gravity * Time.fixedDeltaTime;
                    }
                }
                //gliding
                else
                {
                    if (_moveDirection.y > 0)
                    {
                        //quick reduction of gravity if the player is going up
                        _moveDirection.y -= _glidingUpGravityIncrease * _gravityScale * Gravity * Time.fixedDeltaTime;
                    }
                    else
                    {
                        //now that there is no upward movement, the player float down slowly.
                        //TODO: check if better with or without gravity acceleration
                        _moveDirection.y = -_glidingDescentFixedSpeed;// * Random.Range(.5f, 2f);
                        //_moveDirection.y -= _glidingDownGravityDecrease * _gravityScale * Gravity * Time.fixedDeltaTime;
                    }
                }
            }
            //Clamping _moveDirection.y at _maxVerticalSpeed
            if (Math.Abs(_moveDirection.y) > _maxVerticalSpeed)
                _moveDirection.y = Math.Sign(_moveDirection.y)* _maxVerticalSpeed;
            _characterController.Move(_moveDirection * Time.fixedDeltaTime);
        }

        public bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

        public bool IsGliding()
        {
            return _isGliding;
        }
        
        /*
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //TODO
            //Idea: if collided, then do not consider collision until ground is touched?
            if (_characterController.collisionFlags != CollisionFlags.Above) return;
            if (Vector3.Dot(hit.normal, _moveDirection) < 0)
            {
                _moveDirection -= hit.normal * Vector3.Dot( hit.normal, _moveDirection ) * _moveDirection.y;
            }
            /*
            //Debug.DrawLine(hit.normal, hit.normal * 2, Color.white, 0.5f);
            if (!_characterController.isGrounded && hit.normal.y < -.5f)
            {
                Debug.DrawLine(hit.normal, hit.normal * 2, Color.white, 0.5f);
                _moveDirection.y = -(float) Math.Sqrt(Math.Abs(_moveDirection.y));
            }
            //hit.normal
            //TODO
            
        }*/
    }
}