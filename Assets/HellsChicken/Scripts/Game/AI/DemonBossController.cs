using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBossController : MonoBehaviour
{
    [SerializeField] public int _health = 100;
    public Animator anim;


    public void Update()
    {
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
