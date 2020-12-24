using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBossController : MonoBehaviour
{
    [SerializeField] public int _health;
    [SerializeField] public int _enragedHealth;
    public Animator anim;
    public Transform player;
    
    private GameObject bossSpine;
    private bool isFlipped;
    private bool _isDead;
    private bool _isEnraged;
    private bool _isDamaged;


    public void Start()
    {
        bossSpine = GameObject.Find("DEMON_LORD_ Spine");
    }

    public void Update()
    {

        if(_health == _enragedHealth)
            anim.SetTrigger("Enraged");
        
        if (_health == 0)
            _isDead = true;
        
        if (_isDead)
            StartCoroutine(DemonBossDeath(3f));
        
        anim.SetBool("isDead",_isDead);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Attack"))
        {
            _health -= 10;
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
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }

    IEnumerator ResetTrigger(float timer)
    {
        yield return new WaitForSeconds(timer);
        anim.ResetTrigger("isDamaged");
    }
}
