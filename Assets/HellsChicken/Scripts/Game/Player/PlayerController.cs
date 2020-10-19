using UnityEngine;
using UnityEngine.Serialization;

namespace HellsChicken.Scripts.Game.Player
{
    [RequireComponent(typeof(Rigidbody))]

    public class PlayerController : MonoBehaviour
    {

        [SerializeField] private float _walkSpeed = 5.0f;
        [SerializeField] private float _jumpFactor = 5.0f;

        private float _horizontalMovement;
        private bool _jump = false;

        private Vector3 _tempVelocity;
        
        private CapsuleCollider _capsuleCollider;
        private Rigidbody _rigidbody;
        private Transform _transform;

        private RaycastHit _hitInfo;

        private void Awake()
        {
            _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
            _rigidbody = gameObject.GetComponent<Rigidbody>();
            _transform = gameObject.GetComponent<Transform>();
            _tempVelocity = new Vector3();
        }

        public void MoveHorizontally(float horizontalMovement)
        {
            _horizontalMovement = horizontalMovement;
        }

        public void Jump()
        {
            _jump = IsGrounded();
        }
    

        private void FixedUpdate()
        {
            if (_jump)
            {
                _tempVelocity.y = _jumpFactor;
                //_rigidbody.AddForce(_jumpFactor*_transform.up,ForceMode.Impulse);
                _jump = false;
            }
            else
            {
                _tempVelocity.y = _rigidbody.velocity.y;
            }
            //Speed update 
            _tempVelocity.x = _walkSpeed * _horizontalMovement;
            _tempVelocity.z = 0f;
            _rigidbody.velocity = _tempVelocity;
            //_rigidbody.MovePosition(_rigidbody.position + _walkSpeed * _horizontalMovement * Time.fixedDeltaTime * (Vector3)_transform.right);
            _horizontalMovement = 0f;
            
        }
        
        private bool IsGrounded()
        {
            //return Physics.SphereCast(_transform.position, _capsuleCollider.bounds.extents.x, -Vector3.up, out _hitInfo, _capsuleCollider.bounds.extents.y + 0.1f);
            //Uncomment following line to see the raycast
            //Debug.DrawRay(transform.position, (-Vector3.up * (transform.position.y - _capsuleCollider.bounds.extents.y + 10.1f)), Color.white);
            return Physics.Raycast(_transform.position, -Vector3.up, _capsuleCollider.bounds.extents.y + 0.01f);
        }
    }
}