using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using Cinemachine;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

namespace HellsChicken.Scripts.Game.Player
{

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 5.0f;
        [SerializeField] private float jumpSpeed = 5.0f;
        private bool _isYMovementCorrected;
        private float _yMovementCorrection = 8.0f;
        [SerializeField] private float gravityScale = 2.0f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float lowJumpMultiplier = 2f;
        [SerializeField] private float glidingSpeed = 15f;
        [SerializeField] private ParticleSystem flameStream;
        [SerializeField] private Transform firePosition;
        private Boolean _canShoot = true;
        [SerializeField] private float flamesCooldown = 2f;

        private float _gravity;

        private Vector3 _moveDirection;

        private Transform _transform;
        private CharacterController _characterController;
        private MeshRenderer _meshRenderer;

        private Quaternion _leftRotation;
        private Quaternion _rightRotation;
        
        [SerializeField] private float eggThrowForce = 20f;
        [SerializeField] private GameObject eggPrefab;
        [SerializeField] private Transform eggThrowPoint;
        [SerializeField] private GameObject crosshair;
        private bool _isAiming;
        private bool _isWaitingForEggExplosion;
        private CrosshairSpriteController _crosshairSpriteController;
        
        private Vector3 _lookDirection;

        [SerializeField] private float immunityDuration = 1.0f;
        private bool _isImmune;
        private bool _isLastHeart;


        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _crosshairSpriteController = crosshair.GetComponent<CrosshairSpriteController>();
            _isImmune = false;
            _isLastHeart = false;
            _isYMovementCorrected = false;
            _isAiming = false;
            _isWaitingForEggExplosion = false;
            _moveDirection = Vector3.zero;
            _gravity = Physics.gravity.y;
            _rightRotation = transform.rotation;
            _leftRotation = _rightRotation * Quaternion.Euler(0, 180, 0);
        }

        private void OnEnable()
        {
            EventManager.StartListening("PlayerDeath", Death);
            EventManager.StartListening("LastHeart", LastHeart);
            EventManager.StartListening("EggExplosionNotification", EggExplosionNotification);
        }

        private void Start()
        {
            _characterController.enabled = false;
            if (GameManager.Instance)
                _transform.position = GameManager.Instance.GetCurrentCheckPointPos();
            _characterController.enabled = true;
        }

        private void ShootFlames()
        {
            if (_canShoot)
            {
                ParticleSystem myFlameStream = Instantiate(flameStream, firePosition.position, firePosition.rotation);
                (myFlameStream).transform.SetParent(_transform.transform);
                StartCoroutine(DetachFlames(myFlameStream));
                EventManager.TriggerEvent("flameThrower");
                _canShoot = false;
                Debug.Log("Shoot flames");
                StartCoroutine(EnableFlames(flamesCooldown));
            }
        }

        private void ThrowEgg()
        {
            float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
            float distance = _lookDirection.magnitude;
            Vector2 direction = _lookDirection / distance;
            direction.Normalize();
            GameObject egg = Instantiate(eggPrefab, eggThrowPoint.transform.position,
                Quaternion.Euler(0.0f, 0.0f, angle));
            var velocityCorrected = new Vector3(_moveDirection.x, _moveDirection.y, 0f);
            if (_isYMovementCorrected)
                velocityCorrected.y += _yMovementCorrection;
            egg.GetComponent<Rigidbody>().velocity = (direction * eggThrowForce) + (Vector2) velocityCorrected;
        }

        private void EggExplosionNotification()
        {
            EventManager.StopListening("EggExplosionNotification",EggExplosionNotification);
            _crosshairSpriteController.SetCrosshairToIdle();
            _isWaitingForEggExplosion = false;
            EventManager.StartListening("EggExplosionNotification",EggExplosionNotification);
        }

        private void Update()
        {
            _lookDirection = Target.GetTarget() - eggThrowPoint.position;

            //FIRE
            if (Input.GetButtonDown("Fire1"))
                ShootFlames();

            //EGG 
            //TODO avoid crosshair to disappear behind scene objects, by using a canvas...
            crosshair.transform.position = new Vector3(Target.GetTarget().x, Target.GetTarget().y, 0f); //crosshair always follows mouse
            if(!_isWaitingForEggExplosion)
            {
                if (Input.GetButton("Fire2"))
                {
                    _isAiming = true;
                    _crosshairSpriteController.SetCrosshairToAiming();
                    if (_lookDirection.x > 0.01f)
                    {
                        _transform.rotation = _rightRotation;
                    }
                    else if (_lookDirection.x < -0.01f)
                    {
                        _transform.rotation = _leftRotation;
                    }
                }
                if (Input.GetButtonUp("Fire2"))
                {
                    _isAiming = false;
                    _crosshairSpriteController.SetCrosshairToWaiting();
                    _isWaitingForEggExplosion = true;
                    ThrowEgg();
                }
            }
            
            //TODO SHOW EGG'S LAUNCH DIRECTION
            Debug.DrawLine(Target.GetTarget(), eggThrowPoint.position, Color.white,0.01f);
            
            
            _moveDirection.x = Input.GetAxis("Horizontal") * walkSpeed;
            //_moveDirection.z = Input.GetAxis("Vertical") * 2; //just for fun, z movement
            _moveDirection.z = 0f;

            //CHARACTER ROTATION
            if (_moveDirection.x > 0.01f && !_isAiming)
            {
                _transform.rotation = _rightRotation;
            }
            else if (_moveDirection.x < -0.01f && !_isAiming)
            {
                _transform.rotation = _leftRotation;
            }

            _isYMovementCorrected = false;
            //STICK TO THE PAVEMENT
            if (IsGrounded() && IsFalling()) //The falling check is made because when the character is on ground, it has a negative velocity
            {
                _isYMovementCorrected = true;
                _moveDirection.y = -_yMovementCorrection;
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
                if (Input.GetButton("Jump")) //TODO apply some variation to the velocity while gliding
                {
                    _moveDirection.y = -glidingSpeed;
                }
            }

            //MOVEMENT APPLICATION
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        private void LateUpdate()
        {
            //Constraint the Z position of the playerbody.
            ZConstraint();
        }

        private void ZConstraint()
        {
            if (_transform.position.z == 0) return;
            _characterController.enabled = false;
            var position = _transform.position;
            position = new Vector3(position.x,position.y, 0f);
            _transform.position = position;
            _characterController.enabled = true;
        }

        private bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

        private bool IsFalling()
        {
            return _moveDirection.y < 0f;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            //Stop going up when the character controller collides with something over it
            if (_characterController.collisionFlags != CollisionFlags.Above) return;
            if (Vector3.Dot(hit.normal, _moveDirection) < 0)
            {
                _moveDirection -= hit.normal * Vector3.Dot(hit.normal, _moveDirection);
            }
        }
        
        //Damaged by an Enemy or an EnemyShot, staying on the enemy
        private void OnTriggerStay(Collider other)
        {
            if (!_isImmune)
            {
                if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("EnemyShot"))
                {
                    // Debug.LogError("Hih by an enemy OnTriggerEnter");
                    gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                    if (!_isLastHeart)
                    {
                        StartCoroutine(ImmunityTimer(immunityDuration));
                    }
                    EventManager.TriggerEvent("DecreasePlayerHealth");
                }
            }
        }
        
        //Damaged by an Enemy or an EnemyShot, entering in contact with the enemy
        private void OnTriggerEnter(Collider other)
        {
            if (!_isImmune)
            {
                if (other.transform.CompareTag("Enemy") || other.transform.CompareTag("EnemyShot"))
                {
                    // Debug.LogError("Hih by an enemy OnTriggerEnter");
                    gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                    if (!_isLastHeart)
                    {
                        StartCoroutine(ImmunityTimer(immunityDuration));
                    }
                    EventManager.TriggerEvent("DecreasePlayerHealth");
                }
            }
        }

        private IEnumerator ImmunityTimer(float time)
        {
            _isImmune = true;
            InvokeRepeating(nameof(FlashMesh), 0f, 0.2f);
            //Debug.Log("Transparent");
            yield return new WaitForSeconds(time);
            //Debug.Log("Not Transparent Anymore");
            _isImmune = false;
            CancelInvoke();
            //_meshRenderer.enabled = true;
            var material = _meshRenderer.material;
            var temp = material.color;
            temp.a = 1.0f;
            material.color = temp;
            yield return null;
        }

        private void FlashMesh()
        {
            //_meshRenderer.enabled = !_meshRenderer.enabled;
            //Use the next lines if you want it to be transparent
             var material = _meshRenderer.material;
             var temp = material.color;
             temp.a = temp.a > 0.5f ? 0.3f : 1.0f;
             material.color = temp;
        }

        private void LastHeart()
        {
            EventManager.StopListening("LastHeart", LastHeart);
            _isLastHeart = true;
            EventManager.StartListening("LastHeart", LastHeart);
        }

        private void Death()
        {
            //TODO: if death, it should only respawn player and destroyed objects
            // EventManager.TriggerEvent("RefillPlayerHealth");
            // _characterController.enabled = false;
            // if (GameManager.Instance)
            //     _transform.position = GameManager.Instance.GetCurrentCheckPointPos();
            // _characterController.enabled = true;
            //If player dies, reload the entire scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private IEnumerator EnableFlames(float time)
        {
            yield return new WaitForSeconds(time);
            _canShoot = true;
            yield return null;
            //yield --:> Finché viene ritornata una wait, IEnumerator viene richiamato il frame successivo.
            //Non appena viene ritornato null, si esce da IEnumerator.
        }

        private IEnumerator DetachFlames(ParticleSystem temp)
        {
            yield return new WaitForSecondsRealtime(temp.main.duration);
            temp.transform.parent = null;
            var mainModule = temp.main;
            mainModule.simulationSpeed = 3f;
            yield return null;
        }

        private void OnDisable()
        {
            EventManager.StopListening("PlayerDeath", Death);
            EventManager.StopListening("LastHeart", LastHeart);
            EventManager.StopListening("EggExplosionNotification",EggExplosionNotification);
        }
    }
}
    
