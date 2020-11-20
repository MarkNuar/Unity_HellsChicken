using System;
using UnityEngine;

public class DynamicPlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpSpeed = 10f;
    [SerializeField] private float gravityMultiplier = 4f;
    private Rigidbody _rb;
    private Transform _tr;

    private float _horizontalInput;
    private bool _jump;
    
    private Vector3 _movement;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _tr = GetComponent<Transform>();
    }
 
    void Update()
    {
        _movement = new Vector3(Input.GetAxisRaw("Horizontal"),0f,0f);
        if (Input.GetButtonDown("Jump"))
            _jump = true;
    }
 
    private void FixedUpdate()
    {
        MoveCharacter(_movement);
    }

    void MoveCharacter(Vector3 direction)
    {
        //HORIZONTAL MOVEMENT
        _rb.MovePosition(_tr.position + direction * (moveSpeed * Time.fixedDeltaTime));
        //JUMP
        if (_jump)
        {
            _rb.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            _jump = false;
        }
        //GRAVITY APPLICATION
        _rb.AddForce(new Vector3(0f,Physics.gravity.y * gravityMultiplier,0f));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Ground"))
        {
            
        }
    }
}
