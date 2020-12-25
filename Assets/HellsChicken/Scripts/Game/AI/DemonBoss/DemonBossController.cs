﻿using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class DemonBossController : MonoBehaviour
{
    [SerializeField] public int _MaxHealth;
    [SerializeField] public int _enragedHealth;
    public GameObject healthBarCanvas;
    public HealthBarScript _healthBar;
    private int _currentHealth;
    public Animator anim;
    public Transform player;
    
    private GameObject bossSpine;
    private GameObject bossHead;
    private GameObject bossSword;
    
    private bool isFlipped;
    private bool _isDead;
    private bool _isEnraged;
    private bool _isDamaged;


    public void Start()
    {
        bossSpine = GameObject.Find("DEMON_LORD_ Spine");
        bossHead = GameObject.Find("DEMON_LORD_ Head");
        bossSword = GameObject.Find("DEMON_LORD_SWORD");
        _currentHealth = _MaxHealth;
        _healthBar.SetMaxHealth(_MaxHealth);
        EventManager.StartListening("activateHealthBar",ActivateHealthBar);
    }

    public void Update()
    {

        if(_currentHealth == _enragedHealth)
            anim.SetTrigger("Enraged");
        
        if (_currentHealth == 0)
            _isDead = true;
        
        if (_isDead)
            StartCoroutine(DemonBossDeath(5f));
        
        anim.SetBool("isDead",_isDead);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _currentHealth -= 10;
            _healthBar.SetHealth(_currentHealth);
            anim.SetTrigger("isDamaged");
            StartCoroutine(ResetTrigger(0.5f));
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f,180f,0f);
            isFlipped = false;
        }
        
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }

    IEnumerator DemonBossDeath(float timer)
    {
        bossSpine.GetComponent<CapsuleCollider>().enabled = false;
        bossSword.GetComponent<CapsuleCollider>().enabled = false;
        bossHead.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    IEnumerator ResetTrigger(float timer)
    {
        yield return new WaitForSeconds(timer);
        anim.ResetTrigger("isDamaged");
    }

    private void ActivateHealthBar()
    {        
        EventManager.StopListening("activateHealthBar",ActivateHealthBar);
        healthBarCanvas.SetActive(true);
        EventManager.StartListening("activateHealthBar",ActivateHealthBar);
    }
}
