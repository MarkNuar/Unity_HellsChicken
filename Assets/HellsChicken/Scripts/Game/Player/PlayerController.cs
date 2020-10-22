﻿using System;
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

        [SerializeField] private float walkSpeed = 5.0f;
        [SerializeField] private float jumpSpeed = 5.0f;
        [SerializeField] private float gravityScale = 2.0f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float lowJumpMultiplier = 2f;
        [SerializeField] private float glidingSpeed = 15f;
        [SerializeField] private ParticleSystem flameThrower;
        [SerializeField] private Transform firePosition;
         private Boolean _canShoot = true;
        [SerializeField] private float flamesCooldown = 2f;

        private float _gravity;

        private Vector3 _moveDirection;

        private Transform _transform;
        private CharacterController _characterController;
        
        private Quaternion _leftRotation;
        private Quaternion _rightRotation;
        private bool _lookingRight;

        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            // _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _moveDirection = Vector3.zero;
            _gravity = Physics.gravity.y;
            _rightRotation = transform.rotation;
            _leftRotation = _rightRotation * Quaternion.Euler(0, 180, 0); 
        }

        public void ShootFlames()
        {
            if (_canShoot)
            {

                ParticleSystem myFlamethrower =
                    Instantiate(flameThrower, firePosition.position, firePosition.rotation);
                (myFlamethrower).transform.parent = (_transform).transform;
                _canShoot = false;
                Debug.Log("Shoot flames");
                StartCoroutine(EnableFlames(flamesCooldown));
            }
        }

        public void StartEggAiming()
        {
            Debug.Log("Start Egg Aiming");
        }

        private void Update()
        {
            if(Input.GetButtonDown("Fire1"))
                ShootFlames();
            if(Input.GetButtonDown("Fire2"))
                StartEggAiming();

            _moveDirection.x = Input.GetAxis("Horizontal") * walkSpeed;
            _moveDirection.z = 0f;
            //CHARACTER ROTATION
            if (_moveDirection.x > 0.01f)
            {
                _lookingRight = true;
                _transform.rotation = _rightRotation;
            }
            else if (_moveDirection.x < -0.01f)
            {
                _lookingRight = false;
                _transform.rotation = _leftRotation;
            }
            //STICK TO THE PAVEMENT
            if (IsGrounded() && IsFalling()) //The falling check is made because when the character is on ground, it has a negative velocity
            {
                _moveDirection.y = -8f;
            }
            //JUMPING
            if (IsGrounded() && Input.GetButtonDown("Jump"))
            {
                _moveDirection.y = Mathf.Sqrt(jumpSpeed * -3.0f * _gravity * gravityScale);
            }
            //JUMP PROPORTIONAL TO BAR PRESSING
            if (!IsFalling() && !Input.GetButton("Jump"))
            {
                _moveDirection.y += _gravity * gravityScale * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
            //GRAVITY INCREASE WHEN FALLING
            if (!IsGrounded() && IsFalling())
            {
                _moveDirection.y += _gravity * gravityScale * (fallMultiplier - 1) * Time.deltaTime;
            }
            //GRAVITY APPLICATION
            _moveDirection.y += _gravity * gravityScale * Time.deltaTime;
            //GLIDING
            if (!IsGrounded() && IsFalling())
            {
                if (Input.GetButton("Jump")) //TODO
                {
                    _moveDirection.y = - glidingSpeed;
                }
            }
            //MOVEMENT APPLICATION
            _characterController.Move(_moveDirection * Time.deltaTime);
        }


        private bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

        private bool IsFalling()
        {
            return _moveDirection.y < 0f ;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (_characterController.collisionFlags != CollisionFlags.Above) return;
            if (Vector3.Dot(hit.normal, _moveDirection) < 0)
            {
               _moveDirection -= hit.normal * Vector3.Dot(hit.normal, _moveDirection);
            }
        }
        
        IEnumerator EnableFlames(float time)
        {
            yield return new WaitForSeconds(time);
            _canShoot = true;
            yield return null;
            //yield --:> Finché viene ritornata una wait, IEnumerator viene richiamato il frame successivo. 
            //Non appena viene ritornato null, si esce da IEnumerator.
        }
    }
    
}
