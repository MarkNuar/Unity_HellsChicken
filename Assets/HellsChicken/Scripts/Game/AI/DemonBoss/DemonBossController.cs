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
    private GameObject bossWhip00;
    private GameObject bossWhip01;
    private GameObject bossWhip02;
    private GameObject bossWhip03;
    private GameObject bossWhip04;
    private GameObject bossWhip05;


    private bool isFlipped;
    private bool hasStartedFight;
    private bool isEnraged;
    private bool isDead;
    private bool hasHitWall;

    public void Start()
    {
        bossSpine = GameObject.Find("DEMON_LORD_ Spine");
        bossHead = GameObject.Find("DEMON_LORD_ Head");
        bossSword = GameObject.Find("DEMON_LORD_SWORD");
        bossWhip00 = GameObject.Find("DEMON_LORD_WHIP_HANDLE");
        bossWhip01 = GameObject.Find("DEMON_LORD_WHIP_01");
        bossWhip02 = GameObject.Find("DEMON_LORD_WHIP_02");
        bossWhip03 = GameObject.Find("DEMON_LORD_WHIP_03");
        bossWhip04 = GameObject.Find("DEMON_LORD_WHIP_04");
        bossWhip05 = GameObject.Find("DEMON_LORD_WHIP_05");


        _currentHealth = _maxHealth;
        _healthBar.SetMaxHealth(_maxHealth);
        EventManager.StartListening("activateHealthBar",ActivateHealthBar);
        hasStartedFight = false;
        isEnraged = false;
        isDead = false;
        hasHitWall = false;
    }

    public void Update()
    {

        if (_currentHealth <= _enragedHealth && !isEnraged)
        {
            isEnraged = true;
            EventManager.TriggerEvent("demonRoar");
            anim.SetTrigger("Enraged");
        }

        if (_currentHealth <= 0 && !isDead)
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
        isDead = true;
        EventManager.TriggerEvent("demonDeath");
        anim.SetTrigger("isDead");
        bossSpine.GetComponent<CapsuleCollider>().enabled = false;
        bossSword.GetComponent<CapsuleCollider>().enabled = false;
        bossHead.GetComponent<SphereCollider>().enabled = false;
        bossWhip00.GetComponent<CapsuleCollider>().enabled = false;
        bossWhip01.GetComponent<CapsuleCollider>().enabled = false;
        bossWhip02.GetComponent<CapsuleCollider>().enabled = false;
        bossWhip03.GetComponent<CapsuleCollider>().enabled = false;
        bossWhip04.GetComponent<CapsuleCollider>().enabled = false;
        bossWhip05.GetComponent<CapsuleCollider>().enabled = false;
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

    public bool hasStartedFight1 => hasStartedFight;

    public bool hasHitWall1
    {
        get => hasHitWall;
        set => hasHitWall = value;
    }

    public void Headshot()
    {
        _currentHealth -= _headShotValue;
        _healthBar.SetHealth(_currentHealth);
        EventManager.TriggerEvent("demonDamage");
        anim.SetTrigger("isDamaged");
        StartCoroutine(ResetTrigger(0.5f));
    }

    public void ChestShot()
    {
        _currentHealth -= _chestShotValue;
        _healthBar.SetHealth(_currentHealth);
        EventManager.TriggerEvent("demonDamage");
        anim.SetTrigger("isDamaged");
        StartCoroutine(ResetTrigger(0.5f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            hasHitWall = true;
        }
    }
}
