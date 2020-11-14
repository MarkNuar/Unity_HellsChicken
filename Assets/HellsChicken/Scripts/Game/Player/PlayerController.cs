using System;
using System.Collections;
using System.Diagnostics;
using Cinemachine;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using HellsChicken.Scripts.Game.UI.Crosshair;
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
        [SerializeField] private float flamesCooldown = 2f;

        private float _gravity;

        private Vector3 _moveDirection;

        private Transform _transform;
        private CharacterController _characterController;
        private MeshRenderer _meshRenderer;
        public Animator anim;

        private Quaternion _leftRotation;
        private Quaternion _rightRotation;

        [SerializeField] private float initialEggVelocity= 6f;
        [SerializeField] private float deltaEggVelocity= 5f;
        [SerializeField] private float maxEggVelocity= 20f;
        [SerializeField] private GameObject eggPrefab;
        [SerializeField] private Transform eggThrowPoint;
        [SerializeField] private GameObject crosshairCanvas;
        private CrosshairImageController _crosshairImageController;
        [SerializeField] private GameObject playerCamera;
        private Target _target;
        private bool _isAiming;
        private bool _isWaitingForEggExplosion;
        private bool _isMoving;
        private bool _isGliding;
        private bool _isShootingFlames;
        private bool _isShootingEgg;
        private bool _canShoot;
        private bool _isDead;
        private bool _hasVibrated;
        [SerializeField] private float eggCooldown = 2;

        private Vector3 _lookDirection;

        [SerializeField] private float immunityDuration = 1.0f;
        private bool _isImmune;
        private bool _isLastHeart;


        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            _meshRenderer = gameObject.GetComponent<MeshRenderer>();
            _crosshairImageController = crosshairCanvas.transform.GetChild(0).GetComponent<CrosshairImageController>();
            
            _target = playerCamera.GetComponent<Target>();
            _isImmune = false;
            _isLastHeart = false;
            _isYMovementCorrected = false;
            _isAiming = false;
            _isMoving = false;
            _isGliding = false;
            _isWaitingForEggExplosion = false;
            _isShootingEgg = false;
            _isShootingFlames = false;
            _canShoot = true;
            _isDead = false;
            _hasVibrated = false;
            _moveDirection = Vector3.zero;
            _gravity = Physics.gravity.y;
            _rightRotation = transform.rotation;
            _leftRotation = _rightRotation * Quaternion.Euler(0, 180, 0);
        }

        private void OnEnable()
        {
            EventManager.StartListening("PlayerDeath", Death);
            EventManager.StartListening("LastHeart", LastHeart);
            EventManager.StartListening("StartImmunityCoroutine", StartImmunityCoroutine);
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
                _isShootingFlames = true;
                Debug.Log("Shoot flames");
                StartCoroutine(EnableFlames(flamesCooldown));
            }
        }

        private void ThrowEgg()
        {
            _isShootingEgg = true;
            float g = -_gravity;
            float angle = float.NaN;
            float v = initialEggVelocity; //initial throw egg force
            while (float.IsNaN(angle))
            {
                float v2 = v * v;
                float v4 = v2 * v2;
                var sourcePosition = eggThrowPoint.position;
                float x = _target.GetTarget().x - sourcePosition.x;
                if (Mathf.Abs(x) < 0.01f)
                {
                    angle = Mathf.PI / 2;
                }
                else
                {
                    float y = _target.GetTarget().y - sourcePosition.y;
                    float x2 = x * x;
                    float squareRoot = (float) Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2));
                    angle = Mathf.Atan((v2 - squareRoot) / (g * x));
                    
                    if (_lookDirection.x < 0f)
                        angle = Mathf.PI + angle;
                    
                    if (float.IsNaN(angle))
                    {
                        v = v + deltaEggVelocity;
                        if (v > maxEggVelocity)
                        {
                            v = maxEggVelocity;
                            if (_lookDirection.x > 0f)
                                angle = Mathf.PI / 4; //throw with the best angle for reaching maximum distance
                            else
                                angle = 3 * Mathf.PI / 4;
                        }
                    }
                }
            }
            GameObject egg = Instantiate(eggPrefab, eggThrowPoint.transform.position,
                Quaternion.identity);
            var baseEggVelocity = new Vector3(v * Mathf.Cos(angle), v * Mathf.Sin(angle), 0f);
            
            // //TODO: velocity correction if only the player is grounded
            // var playerVelocityCorrected = new Vector3(_moveDirection.x, _moveDirection.y, 0f);
            // if (_isYMovementCorrected)
            //     playerVelocityCorrected.y += _yMovementCorrection;
            // if (!IsGrounded())
            // {
            //     egg.GetComponent<Rigidbody>().velocity = baseEggVelocity;
            // }
            // else //grounded
            // {
            //     egg.GetComponent<Rigidbody>().velocity = baseEggVelocity + playerVelocityCorrected;
            // }
            
            egg.GetComponent<Rigidbody>().velocity = baseEggVelocity;
        }

        private IEnumerator EnableEggThrow(float time)
        {
            _isWaitingForEggExplosion = true;
            yield return new WaitForSeconds(time);
            _crosshairImageController.SetCrosshairToIdle();
            _isWaitingForEggExplosion = false;
            yield return null;
        }

        private void Update()
        {
            if (!_isDead)
            {
                _isShootingFlames = false;
                _isShootingEgg = false;
                _lookDirection = _target.GetTarget() - eggThrowPoint.position;

                //FIRE
                if (Input.GetButtonDown("Fire1"))
                    ShootFlames();

                //EGG
                if (!_isWaitingForEggExplosion)
                {
                    if (Input.GetButton("Fire2"))
                    {
                        _isAiming = true;
                        _crosshairImageController.SetCrosshairToAiming();

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
                        _crosshairImageController.SetCrosshairToWaiting();
                        StartCoroutine(EnableEggThrow(eggCooldown));
                        ThrowEgg();
                    }
                }

                //HORIZONTAL MOVEMENT
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

                //STICK TO THE PAVEMENT
                _isYMovementCorrected = false;
                if (IsGrounded() && IsFalling()
                ) //The falling check is made because when the character is on ground, it has a negative velocity
                {
                    _isYMovementCorrected = true;
                    _moveDirection.y = -_yMovementCorrection;
                    _isGliding = false;

                    if (_isMoving)
                        EventManager.TriggerEvent("footSteps");
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
                        _isGliding = true;
                        _moveDirection.y = -glidingSpeed;
                        EventManager.TriggerEvent("wingsFlap");
                    }
                    else
                        _isGliding = false;
                }

                //MOVEMENT CHECK
                if (_moveDirection.x != 0)
                    _isMoving = true;
                else
                    _isMoving = false;

                //MOVEMENT APPLICATION
                _characterController.Move(_moveDirection * Time.deltaTime);
                
                //ANIMATION
                anim.SetBool("isGrounded", IsGrounded());
                anim.SetBool("isMoving", _isMoving);
                anim.SetBool("isGliding", _isGliding);
                anim.SetBool("isShootingFlames", _isShootingFlames);
                anim.SetBool("isShootingEgg", _isShootingEgg);
            }

            //DEATH MANAGEMENT
            else if (_isDead && !_hasVibrated)
            {
                gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse(); 
                EventManager.TriggerEvent("chickenDeath");
                StartCoroutine(EnableDeath(1.2f));
                _hasVibrated = true;
            }
            
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

        private void OnControllerColliderHit(ControllerColliderHit hit){

            if (hit.gameObject.CompareTag("MovingPlatform")) {
                EventManager.TriggerEvent("platformCollide",hit.gameObject.name);
            }
            
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
            if (!_isDead)
            {
                if (!_isImmune)
                {
                    if (other.CompareTag("Enemy") || other.CompareTag("EnemyShot"))
                    {
                        EventManager.TriggerEvent("StartImmunityCoroutine");
                        EventManager.TriggerEvent("chickenDamage");
                    }

                    if (other.CompareTag("Lava"))
                    {
                        _isDead = true;
                    }
                }
            }
        }

        //Damaged by an Enemy or an EnemyShot, entering in contact with the enemy
        private void OnTriggerEnter(Collider other)
        {
            if (!_isDead)
            {
                if (!_isImmune)
                {
                    if (other.CompareTag("Enemy") || other.CompareTag("EnemyShot"))
                    {
                        EventManager.TriggerEvent("StartImmunityCoroutine");
                        EventManager.TriggerEvent("chickenDamage");
                    }

                    if (other.CompareTag("Lava"))
                    {
                        _isDead = true;
                    }
                }
            }
        }

        private IEnumerator ImmunityTimer(float time)
        {
            _isImmune = true;
            
            //TODO CHANGE LAYER OF THE PLAYER TO IMMUNEPLAYER
            gameObject.layer = LayerMask.NameToLayer("ImmunePlayer");
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
            gameObject.layer = LayerMask.NameToLayer("Player");
            yield return null;
        }

        private void FlashMesh()
        {
            //_meshRenderer.enabled = !_meshRenderer.enabled;
            //Use the next lines if you want it to be transparent
             var material = _meshRenderer.material;
             var temp = material.color;
             print(material.color.a);
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
        }

        private void StartImmunityCoroutine() {
            EventManager.StopListening("StartImmunityCoroutine", StartImmunityCoroutine);
            gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
            if (!_isLastHeart)
            {
                StartCoroutine(ImmunityTimer(immunityDuration));
            }
            EventManager.TriggerEvent("DecreasePlayerHealth");
            EventManager.StartListening("StartImmunityCoroutine", StartImmunityCoroutine);
        }

        private IEnumerator EnableDeath(float time)
        {
            yield return new WaitForSeconds(time);
            EventManager.TriggerEvent("KillPlayer");
            yield return null;
        }
    }
}
