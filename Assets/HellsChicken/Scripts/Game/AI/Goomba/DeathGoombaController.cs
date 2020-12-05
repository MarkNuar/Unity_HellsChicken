using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGoombaController : MonoBehaviour
{
    [SerializeField] private float gravityScale = 10f;
    
    private CharacterController _characterController;
    private Vector3 _movement;
    
    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        //_characterController.detectCollisions = false;
        _movement = Vector3.zero; 
    }

    // Update is called once per frame
    void Update()
    {
        if (_characterController.enabled)
        {
            if (_characterController.isGrounded)
                _movement.y = -20f;
            else
                _movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;
            _characterController.Move(_movement * Time.deltaTime);
        }
    }
}
