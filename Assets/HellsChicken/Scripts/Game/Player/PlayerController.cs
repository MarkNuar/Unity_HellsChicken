using System;
using System.Collections;
using Cinemachine;
using Cinemachine.Utility;
using DigitalRuby.LightningBolt;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using HellsChicken.Scripts.Game.UI.Crosshair;
using HellsChicken.Scripts.Game.UI.Health;
using HellsChicken.Scripts.Game.UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using Debug = UnityEngine.Debug;

namespace HellsChicken.Scripts.Game.Player
{

    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float walkSpeed = 5.0f;
        [SerializeField] private float jumpSpeed = 5.0f;
        [SerializeField] private float maxSpeedVectorMagnitude = 100f;
        private bool _isYMovementCorrected;
        [SerializeField] private float yMovementCorrection = 8.0f;
        [SerializeField] private float gravityScale = 2.0f;
        [SerializeField] private float fallMultiplier = 2.5f;
        [SerializeField] private float lowJumpMultiplier = 2f;
        [SerializeField] private float glidingSpeed = 15f;
        //[SerializeField] private float glidingFrequency = 20f;
        [SerializeField] private float glidingAmplitude = 2f;
        [SerializeField] private ParticleSystem flameStream;
        [SerializeField] private ParticleSystem landingAnimationPrefab;
        [SerializeField] private Transform firePosition;
        [SerializeField] private Transform landingAnimationSpawnPoint;
        [SerializeField] private float flamesCooldown = 2f;
        [SerializeField] private MeshRenderer[] eyesMeshRenderer;
        private float _gravity;

        private Vector3 _moveDirection;

        private Transform _transform;
        private CharacterController _characterController;
        private SkinnedMeshRenderer _skinnedMeshRenderer;
        //private CapsuleCollider _capsuleCollider;
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

        private float _slopeAngle = 0f;
        private float _prevSlopeAngle = 0f;
        private bool _wasSlidingOnPrevFame;
        private bool _isSliding;
        private Vector3 _hitNormal;
        [SerializeField] private float slideFriction = 0.3f;
        private readonly RaycastHit[] _feetHitPoints = new RaycastHit[5];
        [SerializeField] private LayerMask slideMask;

        private bool _isAiming;
        private bool _isWaitingForEggExplosion;
        private bool _isMoving;
        private bool _isGliding;
        private bool _isShootingFlames;
        private bool _isShootingEgg;
        private bool _canShoot;
        private bool _isDead;
        private bool _hasVibrated;
        private bool _hasLanded;
        [SerializeField] private float eggCooldown = 2;


        private bool _wasGlidingOnPrevFrame = false;
        private float _glidingStartingTime;

        private bool _isFloating;
        public float windBreaking = 20f;
        public float windPushing = 20f;
        //public float maxWindVelocity = 20f;

        private Transform _cachedPlayerParent;
        
        
        //TODO level 2 initial movement
        private bool _isAutomaticallyMoved;
        [SerializeField] private float automaticMovementDuration = 1.5f;

        public Vector3 getPredictedPosition() {
            Vector3 result = new Vector3(transform.position.x,transform.position.y,0);
            if (!_isMoving)
                return _transform.position;
            else {
                //result.x = transform.position.x + _lookDirection.normalized.x * walkSpeed * 5;
                //result.y = transform.position.y + _lookDirection.normalized.y * walkSpeed * 2;
                //result.y =
            }

            return result;
        }

        private Vector3 _lookDirection;

        [SerializeField] private float immunityDuration = 1.0f;
        private bool _isImmune;
        private bool _isLastHeart;

        //dissolveScript
        private DissolveController _dissolve;

        private void Awake()
        {
            _characterController = gameObject.GetComponent<CharacterController>();
            _transform = gameObject.GetComponent<Transform>();
            _skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            _crosshairImageController = crosshairCanvas.transform.GetChild(0).GetComponent<CrosshairImageController>();
            _target = playerCamera.GetComponent<Target>();

            _wasSlidingOnPrevFame = false;
            _isSliding = false;
            _hasLanded = true;
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
            _isFloating = false;
            _cachedPlayerParent = _transform.parent;
            _moveDirection = Vector3.zero;
            _gravity = Physics.gravity.y;
            _rightRotation = transform.rotation;
            _leftRotation = _rightRotation * Quaternion.Euler(0, 180, 0);
            _dissolve = GetComponent<DissolveController>();
        }

        private void OnEnable()
        {
            EventManager.StartListening("PlayerDeath", Death);
            EventManager.StartListening("LastHeart", LastHeart);
            EventManager.StartListening("NotLastHeart", NotLastHeart);
            EventManager.StartListening("StartImmunityCoroutine", StartImmunityCoroutine);
        }

        private void Start()
        {
            //TODO
            if (LevelManager.Instance.isCurrentCkptTheFirst && LevelManager.Instance.levelNumber != 1)
            {
                _isAutomaticallyMoved = true;
                StartCoroutine(AutomaticMovementTimer(automaticMovementDuration));
            }
            else
                _isAutomaticallyMoved = false;
            
            _cachedPlayerParent = _transform.parent;
            _characterController.enabled = false;
            if (LevelManager.Instance)
                _transform.position = LevelManager.Instance.GetCurrentCheckPointPos();
            EventManager.TriggerEvent("chickenSpawnSound");
            _characterController.enabled = true;
        }

        private IEnumerator AutomaticMovementTimer(float time)
        {
            yield return new WaitForSeconds(time);
            _isAutomaticallyMoved = false;
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
                StartCoroutine(EnableFlames(flamesCooldown));
            }
        }

        private void ThrowEgg()
        {
            _isShootingEgg = true;
            float g = -_gravity;
            float angle = float.NaN;
            float v = initialEggVelocity; //initial throw egg force
            EventManager.TriggerEvent("eggThrowSound");

            while (float.IsNaN(angle))
            {
                float v2 = v * v;
                float v4 = v2 * v2;
                Vector3 sourcePosition = eggThrowPoint.position;
                float x = _target.GetTarget().x - sourcePosition.x;

                if (Mathf.Abs(x) < 0.01f)
                {
                    angle = Mathf.PI / 2;
                }
                else
                {
                    float y = _target.GetTarget().y - sourcePosition.y;
                    float x2 = x * x;
                    float squareRoot = Mathf.Sqrt(v4 - g * (g * x2 + 2 * y * v2));
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

            GameObject egg = Instantiate(eggPrefab, eggThrowPoint.transform.position, Quaternion.identity);
            Vector3 baseEggVelocity = new Vector3(v * Mathf.Cos(angle), v * Mathf.Sin(angle), 0f);

            if (!IsGrounded())
            {
                egg.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(baseEggVelocity, maxEggVelocity);
            }
            else
            {
                egg.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(baseEggVelocity + new Vector3(GetVelocityCorrected().x,0f,0f), maxEggVelocity);
            }
            //egg.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(baseEggVelocity + new Vector3(GetVelocityCorrected().x,0f,0f), maxEggVelocity);
            //egg.GetComponent<Rigidbody>().velocity = Vector3.ClampMagnitude(baseEggVelocity + GetVelocityCorrected(),maxEggVelocity);
            //egg.GetComponent<Rigidbody>().velocity = baseEggVelocity;
        }

        private IEnumerator EnableEggThrow(float time)
        {
            _isWaitingForEggExplosion = true;
            yield return new WaitForSeconds(time);
            _crosshairImageController.SetToIdle();
            _isWaitingForEggExplosion = false;
            yield return null;
        }

        private Vector3 GetVelocityCorrected()
        {
            var playerVelocityCorrected = new Vector3(_moveDirection.x, _moveDirection.y, 0f);
            if (_isYMovementCorrected)
                playerVelocityCorrected.y = 0f;
            return playerVelocityCorrected;
        }

        private void Update()
        {
            if (!_isDead && !PauseMenu.GetGameIsPaused() && !EndMenu.GetGameIsPaused())
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
                        _crosshairImageController.StartAiming();

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
                        _crosshairImageController.StartCooldownAnimation(eggCooldown);
                        StartCoroutine(EnableEggThrow(eggCooldown));
                        ThrowEgg();
                    }
                }

                //SLIDE CHECK
                _wasSlidingOnPrevFame = _isSliding;
                _prevSlopeAngle = _slopeAngle; // slide check call will eventually change the slope angle
                _isSliding = GroundCheck();


                //HORIZONTAL MOVEMENT CACHING
                var cachedHorizontalMovement = Input.GetAxis("Horizontal") * walkSpeed;
                _moveDirection.z = 0f;

                //CHARACTER ROTATION
                if (cachedHorizontalMovement > 0.01f && !_isAiming)
                {
                    _transform.rotation = _rightRotation;
                }
                else if (cachedHorizontalMovement < -0.01f && !_isAiming)
                {
                    _transform.rotation = _leftRotation;
                }

                // Debug.DrawLine(Vector3.zero,_hitNormal,Color.green);
                // Debug.Log(_isSliding);


                // AUTOMATIC RIGHT MOVEMENT AT THE BEGINNING OF THE LEVEL
                if (_isAutomaticallyMoved)
                {
                    _moveDirection.x = walkSpeed*0.6f;
                    _isYMovementCorrected = true;
                    _moveDirection.y = -yMovementCorrection;
                    _isGliding = false;
                    if (_isMoving)
                        EventManager.TriggerEvent("footsteps");
                }
                // SLIDING
                else if (_isSliding)
                {
                    //FIRST FRAME SLIDING OR SLOPE ANGLE CHANGE
                    var slopeAngleChanged = Mathf.Abs(_slopeAngle - _prevSlopeAngle) > 5f; // check if the slide direction changed while sliding
                    if (!_wasSlidingOnPrevFame || slopeAngleChanged) // reset the sliding at the beginning and when the slide direction changes.
                    {
                        _moveDirection = Vector3.Project(_moveDirection, Quaternion.Euler(0, 0, 90) * _hitNormal) * (1-slideFriction);
                    }

                    //GRAVITY APPLICATION
                    var gravityVector = new Vector3(0, _gravity * gravityScale * fallMultiplier, 0);
                    _moveDirection += Vector3.Project(gravityVector, Quaternion.Euler(0, 0, 90) * _hitNormal) * ((1-slideFriction) * Time.deltaTime);

                    //HORIZONTAL MOVEMENT
                    if (Mathf.Sign(cachedHorizontalMovement * _hitNormal.x) < 0.1f) // in case we are moving while sliding and heading towards the wall
                        cachedHorizontalMovement = 0f;
                    _moveDirection.x += cachedHorizontalMovement;

                    //GLIDING WHILE SLIDING
                    if (Input.GetButton("Jump")) //TODO apply some variation to the velocity while gliding
                    {
                        _isGliding = true;
                        var glideVector = new Vector3(0, -glidingSpeed, 0);
                        _moveDirection = Vector3.Project(glideVector, Quaternion.Euler(0, 0, 90) * _hitNormal);
                        _moveDirection.x += cachedHorizontalMovement;
                        EventManager.TriggerEvent("wingsFlap");
                    }
                    else
                    {
                        _isGliding = false;
                    }
                }
                // FLOATING
                else if (_isFloating)
                {
                    //HORIZONTAL MOVEMENT APPLICATION
                    _moveDirection.x = cachedHorizontalMovement;

                    //VERTICAL MOVEMENT BY WIND
                    if (IsFalling() && !IsGrounded()) //wind against our fall
                        _moveDirection.y += windBreaking * ((Mathf.Abs(_moveDirection.y)+2)/maxSpeedVectorMagnitude) * Time.deltaTime; //acceleration up
                    else //wind with our fall
                        _moveDirection.y += windPushing * Time.deltaTime;

                    if (_moveDirection.y > maxSpeedVectorMagnitude)
                        _moveDirection.y = maxSpeedVectorMagnitude;
                }
                // NORMAL MOVEMENT
                else
                {
                    //RESUME NORMAL SPEED AFTER SLIDING
                    if (_wasSlidingOnPrevFame)
                    {
                        _moveDirection = Vector3.Project(GetVelocityCorrected(), Vector3.down); //project velocity back on y axes
                    }

                    //HORIZONTAL MOVEMENT APPLICATION
                    _moveDirection.x = cachedHorizontalMovement;

                    //STICK TO THE PAVEMENT
                    _isYMovementCorrected = false;
                    if (IsGrounded() && IsFalling()) //The falling check is made because when the character is on ground, it has a negative velocity
                    {
                        _isYMovementCorrected = true;
                        _moveDirection.y = -yMovementCorrection;
                        _isGliding = false;

                        if (_isMoving)
                            EventManager.TriggerEvent("footsteps");
                    }

                    if (!_isMoving || !IsGrounded())
                        EventManager.TriggerEvent("stopFootsteps");

                    //JUMPING
                    if (IsGrounded() && Input.GetButtonDown("Jump"))
                    {
                        _moveDirection.y = Mathf.Sqrt(jumpSpeed * -3.0f * _gravity * gravityScale);
                        EventManager.TriggerEvent("chickenJumpSound");
                        _hasLanded = false;
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
                    _wasGlidingOnPrevFrame = _isGliding;
                    if (!IsGrounded() && IsFalling())
                    {
                        if (Input.GetButton("Jump")) //TODO apply some variation to the velocity while gliding
                        {
                            _isGliding = true;
                            if (!_wasGlidingOnPrevFrame && _isGliding)
                                _glidingStartingTime = Time.time;
                            _moveDirection.y = -glidingSpeed - Mathf.Sin((Time.time - _glidingStartingTime) * 4 * 2 * Mathf.PI) * glidingAmplitude;;
                            EventManager.TriggerEvent("wingsFlap");
                        }
                        else
                        {
                            _isGliding = false;
                        }
                    }
                }

                //MOVEMENT CHECK
                _isMoving = _moveDirection.x != 0;

                //CLAMP PLAYER VELOCITY TO MAX MAGNITUDE
                _moveDirection = Vector3.ClampMagnitude(_moveDirection, maxSpeedVectorMagnitude);
                Debug.DrawLine(Vector3.zero,_moveDirection, Color.magenta);


                //MOVEMENT APPLICATION
                _characterController.Move(_moveDirection * Time.deltaTime);

                //ANIMATION
                anim.SetBool("isTrulyFalling", IsTrulyFalling());
                anim.SetBool("isGrounded", IsGrounded());
                anim.SetBool("isMoving", _isMoving);
                anim.SetBool("isGliding", _isGliding);
                anim.SetBool("isShootingFlames", _isShootingFlames);
                anim.SetBool("isShootingEgg", _isShootingEgg);
                anim.SetBool("isFloating", _isFloating);

            }

            //DEATH MANAGEMENT
            else if (_isDead && !_hasVibrated)
            {
                gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                EventManager.TriggerEvent("chickenDeath");
                //StartCoroutine(EnableDeath(1.2f));
                EventManager.TriggerEvent("KillPlayer");
                _hasVibrated = true;
            }

        }

        private void LateUpdate()
        {
            //Constraint the Z position of the player body.
            ZConstraint();
        }

        private void ZConstraint()
        {
            if (_transform.position.z == 0) return;
            _characterController.enabled = false;
            Vector3 position = _transform.position;
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

        private bool IsTrulyFalling()
        {
            return GetVelocityCorrected().y < 0f;
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // if (hit.gameObject.CompareTag("MovingPlatform"))
            // {
            //     EventManager.TriggerEvent("platformCollide",hit.gameObject.name);
            // }

            //Stop going up when the character controller collides with something over it
            if (_characterController.collisionFlags != CollisionFlags.Above)
                return;
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
                    if (other.CompareTag("Enemy") || other.CompareTag("EnemyShot") || other.CompareTag("Shield"))
                    {
                        EventManager.TriggerEvent("StartImmunityCoroutine");
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
                    if (other.CompareTag("Enemy") || other.CompareTag("EnemyShot") || other.CompareTag("Shield"))
                    {
                        EventManager.TriggerEvent("StartImmunityCoroutine");
                    }

                    if (other.CompareTag("Lava"))
                    {
                        _isDead = true;
                    }

                    if (other.CompareTag("Ground") && !_hasLanded)
                    {
                        _hasLanded = true;
                        Instantiate(landingAnimationPrefab, landingAnimationSpawnPoint.position, landingAnimationSpawnPoint.rotation);
                        EventManager.TriggerEvent("chickenLand");
                    }
                }

                if (other.CompareTag("Wind"))
                {
                    _isFloating = true;
                }

                if (other.CompareTag("MovingPlatform"))
                {
                    if (other.transform.position.y < _transform.position.y)
                    {
                        _cachedPlayerParent = _transform.parent;
                        _transform.parent = other.transform;
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Wind"))
            {
                _isFloating = false;
            }

            if (other.CompareTag("MovingPlatform"))
            {
                _transform.parent = _cachedPlayerParent;
            }
        }


        //If true, it sets _hitNormal and _slopeAngle variables to the current values.
        private bool GroundCheck()
        {
            if (!IsFalling())
            {
                _hitNormal = Vector3.zero;
                _slopeAngle = 0f;
                return false;
            }
            Vector3 normalSum = Vector3.zero;
            CharacterController cc = _characterController;
            float carRad = cc.radius;
            Vector3 ccc = cc.center + transform.position;
            var skinWidth = 0.5f;
            Vector3 sourcePoint = new Vector3(ccc.x, ccc.y - (cc.height / 2 - carRad) + skinWidth / 2, ccc.z);
            var numberOfHits = Physics.SphereCastNonAlloc(sourcePoint, carRad, Vector3.down, _feetHitPoints, skinWidth, slideMask);
            if (numberOfHits == 0)
            {
                _hitNormal = Vector3.zero;
                _slopeAngle = 0f;
                return false;
            }
            for(var i = 0; i < numberOfHits; i++)
            {
                if (!_feetHitPoints[i].collider.CompareTag("SlipperyGround"))
                {
                    _hitNormal = Vector3.zero;
                    _slopeAngle = 0f;
                    return false;
                }
                normalSum += _feetHitPoints[i].normal;
            }
            //good but bad, okay for now
            _hitNormal = Vector3.ProjectOnPlane(normalSum.normalized, new Vector3(0, 0, 1)).normalized;
            _slopeAngle = Vector3.Angle(Vector3.up, _hitNormal);
            return true;
        }

        private IEnumerator ImmunityTimer(float time)
        {
            _isImmune = true;
            var material = _skinnedMeshRenderer.material;
            material.SetInt("alphaAnimation",1);

            foreach (var mesh in eyesMeshRenderer)
            {
                mesh. material.SetInt("alphaAnimation",1);
            }

            gameObject.layer = LayerMask.NameToLayer("ImmunePlayer");
            InvokeRepeating(nameof(FlashMesh), 0f, 0.2f);
            //Debug.Log("Transparent");
            yield return new WaitForSeconds(time);
            //Debug.Log("Not Transparent Anymore");
            _isImmune = false;
            CancelInvoke();
            //_meshRenderer.enabled = true;
            material.SetFloat("alphaValue",1.0f);

            foreach (var mesh in eyesMeshRenderer)
            {
                mesh.material.SetFloat("alphaValue", 1.0f);
            }

            material.SetInt("alphaAnimation",0);
            foreach (var mesh in eyesMeshRenderer)
            {
                mesh. material.SetInt("alphaAnimation",0);
            }

            gameObject.layer = LayerMask.NameToLayer("Player");
            yield return null;
        }

        private void FlashMesh()
        {
            //_meshRenderer.enabled = !_meshRenderer.enabled;
            //Use the next lines if you want it to be transparent
             var material = _skinnedMeshRenderer.material;
             material.SetFloat("alphaValue", material.GetFloat("alphaValue") == 1.0f ? -0.1f : 1.0f);

             foreach (var mesh in eyesMeshRenderer)
             {
                 mesh.material.SetFloat("alphaValue", mesh.material.GetFloat("alphaValue") == 1.0f ? -0.1f : 1.0f);
             }
             //_eyesMeshRenderer.material.color = temp;
        }

        private void LastHeart()
        {
            EventManager.StopListening("LastHeart", LastHeart);
            _isLastHeart = true;
            EventManager.StartListening("LastHeart", LastHeart);
        }

        private void NotLastHeart()
        {
            EventManager.StopListening("NotLastHeart", NotLastHeart);
            _isLastHeart = false;
            EventManager.StartListening("NotLastHeart", NotLastHeart);
        }

        private void Death()
        {
            //TODO: if death, it should only respawn player and destroyed object
            _dissolve.Dead = true;
            gameObject.layer = LayerMask.NameToLayer("ImmunePlayer");
            enabled = false;
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
            EventManager.StopListening("NotLastHeart", NotLastHeart);
        }

        private void StartImmunityCoroutine()
        {
            EventManager.StopListening("StartImmunityCoroutine", StartImmunityCoroutine);
            gameObject.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

            if (!_isLastHeart)
            {
                StartCoroutine(ImmunityTimer(immunityDuration));
            }

            EventManager.TriggerEvent("DecreasePlayerHealth");
            EventManager.StartListening("StartImmunityCoroutine", StartImmunityCoroutine);
        }

    }
}
