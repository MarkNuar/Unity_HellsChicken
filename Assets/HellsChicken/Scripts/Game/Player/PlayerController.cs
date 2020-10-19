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

        private float _gravity = 9.81f;
        
        private float _horizontalMovement;
        private bool _jump = false;

        private Vector3 _moveDirection;
        private Vector3 _playerVelocity;
        
        private CharacterController _characterController;

        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _moveDirection = Vector3.zero;
        }

        public void MoveHorizontally(float horizontalMovement)
        {
            _horizontalMovement = horizontalMovement;
        }

        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                _jump = true;
            }
            else
            {
                _jump = false;
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
                if (_jump)
                {
                    _moveDirection.y = _jumpSpeed;
                    _jump = false;
                }
            }
            _moveDirection.y -= _gravityScale * _gravity * Time.fixedDeltaTime;
            //Faster discesa ??
            if (_moveDirection.y < 0f)
            {
                _moveDirection.y -= 0.8f * _gravityScale * _gravity * Time.fixedDeltaTime;
            }
            _characterController.Move(_moveDirection * Time.fixedDeltaTime);
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            
            if (!_characterController.isGrounded)
            {
                _moveDirection.y = 0;
            }
        }
    }
}