using System.Collections;
using Cinemachine;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using HellsChicken.Scripts.Game.UI.Crosshair;
using HellsChicken.Scripts.Game.UI.Menu;
using UnityEngine;
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
        [SerializeField] private ParticleSystem flameStream;
        [SerializeField] private Transform firePosition;
        [SerializeField] private float flamesCooldown = 2f;
        [SerializeField] private MeshRenderer[] eyesMeshRenderer;
        [SerializeField] private PauseMenu pauseMenu;
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
        
        private float _speedInclined;
        private float _slopeAngle;
        private bool _wasSlidingOnPrevFame;
        private bool _isSliding;
        private float _slideHorizontalMovementAccumulator;
        private float _slideVerticalMovementAccumulator;
        private Vector3 _hitNormal;
        [SerializeField] private float slideFriction = 0.3f;
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
        [SerializeField] private float eggCooldown = 2;

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
            
            //TODO 
            //_capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            _wasSlidingOnPrevFame = false;
            _isSliding = false;
            
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
            _dissolve = GetComponent<DissolveController>();
        }

        private void OnEnable()
        {
            EventManager.StartListening("PlayerDeath", Death);
            EventManager.StartListening("LastHeart", LastHeart);
            EventManager.StartListening("StartImmunityCoroutine", StartImmunityCoroutine);
        }

        private void Start()
        {
            //_characterController.detectCollisions = false;
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
            
            // //TODO: velocity correction if only the player is grounded
            // var playerVelocityCorrected = new Vector3(_moveDirection.x, _moveDirection.y, 0f);
            // if (_isYMovementCorrected)
            //     playerVelocityCorrected.y += _yMovementCorrection;
            // egg.GetComponent<Rigidbody>().velocity = baseEggVelocity + playerVelocityCorrected;
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
        
        private Vector3 GetVelocityCorrected()
        {
            var playerVelocityCorrected = new Vector3(_moveDirection.x, _moveDirection.y, 0f);
            if (_isYMovementCorrected)
                playerVelocityCorrected.y += yMovementCorrection;
            return playerVelocityCorrected;
        }
        
        private void Update()
        {
            if (!_isDead && !PauseMenu.GetGameIsPaused() )
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

                //SLIDE CHECK
                _wasSlidingOnPrevFame = _isSliding;
                _isSliding = SlideCheck();
                
                //HORIZONTAL MOVEMENT
                _moveDirection.x = Input.GetAxis("Horizontal") * walkSpeed;
                var cachedHorizontalMovement = _moveDirection.x;
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

                //SLIDING
                if (_isSliding)
                {
                    if (Mathf.Sign(cachedHorizontalMovement * _hitNormal.x) < 0) // in case we are gliding while sliding and heading towards the wall
                        cachedHorizontalMovement = 0f;
                    
                    //FIRST FRAME SLIDING
                    if (!_wasSlidingOnPrevFame)
                    {
                        // first frame in which we are sliding
                        _speedInclined = Mathf.Abs(GetVelocityCorrected().y) * Mathf.Sin(_slopeAngle) * (1-slideFriction);
                        _slideHorizontalMovementAccumulator = _speedInclined * Mathf.Cos(_slopeAngle) * Mathf.Sign(_hitNormal.x);
                        _slideVerticalMovementAccumulator = -_speedInclined * Mathf.Sin(_slopeAngle);
                    }
                    //NOT FIRST FRAME SLIDING
                    else
                    {
                        _speedInclined += -_gravity * gravityScale * (fallMultiplier - 1) * Mathf.Abs(Mathf.Sin(_slopeAngle)) * (1-slideFriction) * Time.deltaTime;
                        _slideHorizontalMovementAccumulator += _speedInclined * Mathf.Cos(_slopeAngle) * Mathf.Sign(_hitNormal.x) * Time.deltaTime;
                        _slideVerticalMovementAccumulator += -_speedInclined * Mathf.Sin(_slopeAngle) * Time.deltaTime;
                    }
                    
                    //SLIDE MOVEMENT APPLICATION
                    _moveDirection.x = _slideHorizontalMovementAccumulator;// + cachedHorizontalMovement;
                    _moveDirection.y = _slideVerticalMovementAccumulator;
                    
                    //GLIDING WHILE SLIDING
                    if (Input.GetButton("Jump")) //TODO apply some variation to the velocity while gliding
                    {
                        _isGliding = true;
                        _slideHorizontalMovementAccumulator = 0f;
                        _slideVerticalMovementAccumulator = 0f;
                        var slidingSpeedInclined = glidingSpeed * Mathf.Sin(_slopeAngle) * (1-slideFriction);
                        // if (Mathf.Sign(cachedHorizontalMovement * _hitNormal.x) < 0) // in case we are gliding while sliding and heading towards the wall
                        //     cachedHorizontalMovement = 0f;
                        _moveDirection.x = slidingSpeedInclined * Mathf.Cos(_slopeAngle) * Mathf.Sign(_hitNormal.x) + cachedHorizontalMovement;
                        _moveDirection.y = -slidingSpeedInclined * Mathf.Sin(_slopeAngle);
                        EventManager.TriggerEvent("wingsFlap");
                    }
                    else
                    {
                        _isGliding = false;
                    }
                }
                else
                {
                    //RESUME NORMAL SPEED AFTER SLIDING
                    if (_wasSlidingOnPrevFame)
                    {
                        _moveDirection.x = cachedHorizontalMovement;
                        _moveDirection.y = _slideVerticalMovementAccumulator;
                        _slopeAngle = 0f;
                        _speedInclined = 0f;
                        _slideHorizontalMovementAccumulator = 0f;
                        _slideVerticalMovementAccumulator = 0f;
                        //do not update moveDirection
                    }
                    
                    //STICK TO THE PAVEMENT
                    _isYMovementCorrected = false;
                    if (IsGrounded() && IsFalling()) //The falling check is made because when the character is on ground, it has a negative velocity
                    {
                        _isYMovementCorrected = true;
                        _moveDirection.y = -yMovementCorrection;
                        _isGliding = false;

                        if (_isMoving)
                            EventManager.TriggerEvent("footSteps");
                    }

                    if (!_isMoving || !IsGrounded())
                        EventManager.TriggerEvent("stopFootSteps");
                
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

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag("MovingPlatform")) 
            {
                EventManager.TriggerEvent("platformCollide",hit.gameObject.name);
            }
            
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
        
        //If true, it sets _hitNormal and _slopeAngle variables to the current values. 
        private bool SlideCheck()
        {
            Vector3 normalSum = Vector3.zero;
            CharacterController cc = _characterController;
            //var capRad = _capsuleCollider.radius;
            float carRad = cc.radius;
            Vector3 ccc = cc.center + transform.position;
            //var skinWidth = (capRad - carRad) * 2; //this is 0.1 in our case, correct
            float skinWidth = 0.1f;
            Vector3 sourcePoint = new Vector3(ccc.x, ccc.y - (cc.height / 2 - carRad) + skinWidth / 2, ccc.z);
            RaycastHit[] slideHitPoints = new RaycastHit[5];
            // Debug.DrawLine(Vector3.zero,sourcePoint,Color.white, 3f);
            // Debug.Log(carRad);
            var numberOfHits = Physics.SphereCastNonAlloc(sourcePoint, carRad, Vector3.down, slideHitPoints, skinWidth, slideMask); //,maxDistance: 20f,layerMask: LayerMask.NameToLayer("SphereSlidingCheck"), QueryTriggerInteraction.Ignore);
            // Debug.Log(numberOfHits);
            
            if (numberOfHits == 0)
                return false;
            
            for(var i = 0; i < numberOfHits; i++)
            {
                if (!slideHitPoints[i].collider.CompareTag("SlipperyGround"))
                    return false;
                
                normalSum += slideHitPoints[i].normal;
            }
            
            _hitNormal = normalSum.normalized;
            _slopeAngle = Mathf.Deg2Rad * Vector3.Angle(Vector3.up, _hitNormal);
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

        private void Death()
        {
            //TODO: if death, it should only respawn player and destroyed objects
            // EventManager.TriggerEvent("RefillPlayerHealth");
            // _characterController.enabled = false;
            // if (GameManager.Instance)
            //     _transform.position = GameManager.Instance.GetCurrentCheckPointPos();
            // _characterController.enabled = true;
            //If player dies, reload the entire scene.
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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

        private IEnumerator EnableDeath(float time)
        {
            yield return new WaitForSeconds(time);
            EventManager.TriggerEvent("KillPlayer");
            yield return null;
        }
    }
}
