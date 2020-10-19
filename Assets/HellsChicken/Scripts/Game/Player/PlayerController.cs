using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace HellsChicken.Scripts.Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float _walkSpeed = 5.0f;
        [SerializeField] private float _jumpSpeed = 5.0f;
        [SerializeField] private float _gravityScale = 2.0f;

        private const float Gravity = 9.81f;

        private float _horizontalMovement;
        private bool _jump = false;
        private bool _glide;
        private bool _isGliding;
        
        
        private Vector3 _moveDirection;

        private Transform _transform;
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            _moveDirection = Vector3.zero;
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
            if (!_characterController.isGrounded)
            {
                _isGliding = true;
            }
            else
            {
                _isGliding = false;
            }
        }
        
        public void ShootFlames()
        {
            Debug.Log("Shoot flames");
        }

        public void EnterEggAiming()
        {
            Debug.Log("Enter Egg Aiming");
        }
        
        private void FixedUpdate()
        {
            _moveDirection.x = _horizontalMovement * _walkSpeed;
            _moveDirection.z = 0f;
            if (_characterController.isGrounded)
            {
                _moveDirection.y = -10f;
                if (_jump)
                {
                    _moveDirection.y = _jumpSpeed;
                    _jump = false;
                }
            }
            /*else
            {
                //gliding
                if (_isGliding)
                {
                    
                }
            }*/
            _moveDirection.y -= _gravityScale * Gravity * Time.fixedDeltaTime;
            //Faster discesa ??
            if (_moveDirection.y < 0f)
            {
                _moveDirection.y -= 1f * _gravityScale * Gravity * Time.fixedDeltaTime;
            }
            _characterController.Move(_moveDirection * Time.fixedDeltaTime);
        }

        /*
        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Debug.DrawLine(hit.normal, hit.normal * 2, Color.white, 0.5f);
            if (!_characterController.isGrounded && hit.normal.y < -Mathf.Epsilon)
            {
                _moveDirection.y = 0f;
            }
            //hit.normal
            //TODO
            /*if (!_characterController.isGrounded)
            {
                
            }
        }*/
    }
}