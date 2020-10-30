using System;
using System.Collections;
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

        private float throwForce = 20f;

        [SerializeField] GameObject eggPrefab;
        [SerializeField] Transform eggThrowPoint;
        [SerializeField] GameObject crosshair;

        private float timer = 2f;
        private float _countdown;
        private bool _isAiming;

        private Vector3 _lookDirection;


        [SerializeField] private float immunityDuration = 1.0f;
        private bool _isImmune;
        private bool _isLastHeart;


        void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _isImmune = false;
            _isLastHeart = false;
            _moveDirection = Vector3.zero;
            _gravity = Physics.gravity.y;
            _rightRotation = transform.rotation;
            _leftRotation = _rightRotation * Quaternion.Euler(0, 180, 0);
        }

        private void OnEnable()
        {
            EventManager.StartListening("PlayerDeath", Death);
            EventManager.StartListening("LastHeart", LastHeart);
        }

        // Use this for initialization
        void Start()
        {
            crosshair.transform.localScale = new Vector3(0, 0, 0);
            _characterController.enabled = false;
            if (GameManager.Instance)
                _transform.position = GameManager.Instance.GetCurrentCheckPointPos();
            _characterController.enabled = true;
        }

        void ShootFlames()
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

        void ThrowEgg()
        {
            float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
            float distance = _lookDirection.magnitude;
            Vector2 direction = _lookDirection / distance;
            direction.Normalize();
            GameObject egg = Instantiate(eggPrefab, eggThrowPoint.transform.position,
                Quaternion.Euler(0.0f, 0.0f, angle));
            egg.GetComponent<Rigidbody>().velocity = direction * throwForce;

            //TODO vettore forza + vettore movimento

            _countdown = timer;
        }

        void Update()
        {
            _lookDirection = Target.GetTarget() - eggThrowPoint.position;
            _countdown -= Time.deltaTime;

            //FIRE
            if (Input.GetButtonDown("Fire1"))
                ShootFlames();

            //EGG
            if (_countdown <= 0f)
            {
                if (Input.GetButton("Fire2"))
                {
                    _isAiming = true;

                    crosshair.transform.localScale = new Vector3(0.25f, 0.25f, 1);
                    crosshair.transform.position = new Vector2(Target.GetTarget().x, Target.GetTarget().y);

                    if (_lookDirection.x > 0.01f)
                    {
                        _transform.rotation = _rightRotation;
                    }
                    else if (_lookDirection.x < -0.01f)
                    {
                        _transform.rotation = _leftRotation;
                    }

                    //TODO traiettoria

                }

                if (Input.GetButtonUp("Fire2"))
                {
                    ThrowEgg();
                    crosshair.transform.localScale = new Vector3(0, 0, 0);
                    _isAiming = false;

                }
            }

            //Debug.DrawLine(eggThrowPoint.position,Target.GetTarget());

            _moveDirection.x = Input.GetAxis("Horizontal") * walkSpeed;
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

            //STICK TO THE PAVEMENT
            if (IsGrounded() && IsFalling()
            ) //The falling check is made because when the character is on ground, it has a negative velocity
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
                    _moveDirection.y = -glidingSpeed;
                }
            }

            //MOVEMENT APPLICATION
            _characterController.Move(_moveDirection * Time.deltaTime);
        }

        bool IsGrounded()
        {
            return _characterController.isGrounded;
        }

        bool IsFalling()
        {
            return _moveDirection.y < 0f;
        }

        void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (!_isImmune)
            {
                if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("EnemyShot"))
                {
                    Debug.LogError("Hih by an enemy");
                    gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                    if (!_isLastHeart)
                    {
                        StartCoroutine(ImmunityTimer(immunityDuration));
                    }
            
                    EventManager.TriggerEvent("DecreasePlayerHealth");
                }
            
                // else if (hit.transform.CompareTag("Magma"))
                // {
                //     //TODO DIE IMMEDIATLY
                // }
            }

            if (_characterController.collisionFlags != CollisionFlags.Above) return;
            if (Vector3.Dot(hit.normal, _moveDirection) < 0)
            {
                _moveDirection -= hit.normal * Vector3.Dot(hit.normal, _moveDirection);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_isImmune)
            {
                if (other.collider.CompareTag("Enemy") || other.collider.CompareTag("EnemyShot"))
                {
                    Debug.LogError("Hih by an enemy");
                    if (!_isLastHeart)
                    {
                        gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                        StartCoroutine(ImmunityTimer(immunityDuration));
                    }
                    EventManager.TriggerEvent("DecreasePlayerHealth");
                }

                // else if (hit.transform.CompareTag("Magma"))
                // {
                //     //TODO DIE IMMEDIATLY
                // }
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
            _meshRenderer.enabled = true;
            yield return null;
        }

        private void FlashMesh()
        {
            _meshRenderer.enabled = !_meshRenderer.enabled;
        }

        private void LastHeart()
        {
            EventManager.StopListening("LastHeart", LastHeart);
            _isLastHeart = true;
            EventManager.StartListening("LastHeart", LastHeart);
        }

        private void Death()
        {
            //If player dies, reload the entire scene.
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        IEnumerator EnableFlames(float time)
        {
            yield return new WaitForSeconds(time);
            _canShoot = true;
            yield return null;
            //yield --:> Finché viene ritornata una wait, IEnumerator viene richiamato il frame successivo.
            //Non appena viene ritornato null, si esce da IEnumerator.
        }

        IEnumerator DetachFlames(ParticleSystem temp)
        {

            yield return new WaitForSecondsRealtime(temp.main.duration);
            temp.transform.parent = null;
            var mainModule = temp.main;
            mainModule.simulationSpeed = 3f;
            yield return null;
        }
    }
}
    
