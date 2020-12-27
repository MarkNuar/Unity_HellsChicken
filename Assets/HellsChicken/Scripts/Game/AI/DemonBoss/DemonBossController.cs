using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using Pathfinding.Util;
using UnityEngine;

public class DemonBossController : MonoBehaviour
{
    [SerializeField] public int _maxHealth;
    [SerializeField] public int _enragedHealth;
    [SerializeField] public int _headShotValue;
    [SerializeField] public int _chestShotValue;
    public GameObject healthBarCanvas;
    public HealthBarScript _healthBar;
    private int _currentHealth;
    public Animator anim;
    public Transform player;
    
    private GameObject bossSpine;
    private GameObject bossHead;
    private GameObject bossSword;
    
    private bool isFlipped;
    private bool hasStartedFight;

    public void Start()
    {
        bossSpine = GameObject.Find("DEMON_LORD_ Spine");
        bossHead = GameObject.Find("DEMON_LORD_ Head");
        bossSword = GameObject.Find("DEMON_LORD_SWORD");
        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        EventManager.StartListening("activateHealthBar",ActivateHealthBar);
        hasStartedFight = false;
    }

    public void Update()
    {

        if(_currentHealth <= _enragedHealth)
            anim.SetTrigger("Enraged");

        if (_currentHealth <= 0)
            DemonBossDeath();

        if (_currentHealth != _maxHealth)
            hasStartedFight = true;

    }

    private void OnCollisionEnter(Collision other)
    {
        foreach (ContactPoint contact in other.contacts)
        {
            var colName = contact.thisCollider.name;
            switch (colName)
            {
                case "DEMON_LORD_ Head":
                    Headshot();
                    break;
                case "DEMON_LORD_ Spine":
                    ChestShot();
                    break;
            }
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

    void DemonBossDeath()
    {
        anim.SetTrigger("isDead");
        bossSpine.GetComponent<CapsuleCollider>().enabled = false;
        bossSword.GetComponent<CapsuleCollider>().enabled = false;
        bossHead.GetComponent<SphereCollider>().enabled = false;
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

    public bool getHasStartedFight()
    {
        return hasStartedFight;
    }

    public void Headshot()
    {
        _currentHealth -= _headShotValue;
        _healthBar.SetHealth(_currentHealth);
        anim.SetTrigger("isDamaged");
        StartCoroutine(ResetTrigger(0.5f));
    }

    public void ChestShot()
    {
        _currentHealth -= _chestShotValue;
        _healthBar.SetHealth(_currentHealth);
        anim.SetTrigger("isDamaged");
        StartCoroutine(ResetTrigger(0.5f));
    }
}
