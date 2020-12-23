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


    public void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    public void Start()
    {
        _movement = Vector3.zero;
        _movement.x = -5f;
    }

    public void Update()
    {
        _characterController.Move(_movement * Time.deltaTime);
        if (_health == 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _health -= 10;
            Debug.Log(_health);
        }
    }
}
