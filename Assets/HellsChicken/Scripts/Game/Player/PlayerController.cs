using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float _walkingSpeed = 5.0f;
    [SerializeField] private float _jumpSpeed = 5.0f;

    private float _horizontalMovement;
    private bool _jump = false;

    private CapsuleCollider _capsuleCollider;
    private Rigidbody _rigidbody;
    private Transform _transform;

    private RaycastHit _hitInfo;
    
    private void Awake()
    {
        _capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _transform = gameObject.GetComponent<Transform>();
    }

    public void MoveHorizontally(float horizontalMovement)
    {
        _horizontalMovement = horizontalMovement;
    }

    public void Jump()
    {
        if (IsGrounded())
        {
            _jump = true;
        }
        else
        {
            _jump = false;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (_jump)
        {
            _rigidbody.AddForce(_jumpSpeed*_transform.up,ForceMode.Impulse);
            _jump = false;
        }
        _rigidbody.MovePosition(_rigidbody.position + _walkingSpeed * _horizontalMovement * Time.fixedDeltaTime * (Vector3)_transform.right);
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
