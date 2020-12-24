using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBossController : MonoBehaviour
{
    [SerializeField] public int _health = 100;
    public Animator anim;
    private CharacterController _characterController;
    private Vector3 _movement;

    private bool _isMoving;
    private bool _isDead;
    private bool _isFighting;
    private bool _isEnraged;


    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Start()
    {
        _movement = Vector3.zero;
    }

    public void Update()
    {
        
        if (_movement.x != 0)
            _isMoving = true;
        else
            _isMoving = false;
        
        if (_health == 0)
            _isDead = true;
        
        
        anim.SetBool("isMoving",_isMoving);
        anim.SetBool("isDead",_isDead);
        
        _characterController.Move(_movement * Time.deltaTime);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _health -= 10;
            Debug.Log("oo");
        }
    }
}
