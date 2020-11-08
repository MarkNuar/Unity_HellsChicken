using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyPlayerController : MonoBehaviour
{
    public Vector3 Gravity = Vector3.down * 9.81f;
    public float GravityScale = 3.0f;
    public float Velocity = 12;
    public float GroundControl = 3.0f;
    public float AirControl = 3.0f;
    public float JumpVelocity = 9;
    public float GroundHeight = 2.1f;
    private bool _jump;

    private Vector3 _playerVelocity;
    private float _horizontalMovement;
    private Rigidbody _rigidbody;
    private Transform _transform;
    
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _transform = gameObject.GetComponent<Transform>();
        _playerVelocity = Vector3.zero;

        _rigidbody.useGravity = false;
    }
 
    void Update() {
        _jump = _jump || Input.GetButtonDown("Jump");
        _horizontalMovement = Input.GetAxisRaw("Horizontal");
    }
 
    void FixedUpdate() {
        // Try with a spherecast
        bool grounded = Physics.Raycast(transform.position, Gravity.normalized, GroundHeight);

        _playerVelocity.x = _horizontalMovement * Velocity;
        
        _playerVelocity.z = 0f;

        if (grounded)
        {
            _playerVelocity.y = -5f;
        }
        
        if (grounded && _jump)
        {
            _playerVelocity.y = Mathf.Sqrt(JumpVelocity * -3.0f * Gravity.y * GravityScale);
        }
        
        _playerVelocity.y += Gravity.y * GravityScale * Time.fixedDeltaTime;
        Debug.Log(_playerVelocity);
        
        _transform.position += _playerVelocity * Time.fixedDeltaTime;
        
        _jump = false;
    }
}
