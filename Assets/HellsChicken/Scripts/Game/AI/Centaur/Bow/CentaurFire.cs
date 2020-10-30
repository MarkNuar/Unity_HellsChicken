using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CentaurFire : MonoBehaviour {
    
    [SerializeField] private GameObject startExplosion;
    [SerializeField] private GameObject contactExplosion;

    private Rigidbody _rigidbody;
    private Vector3 target;
    private float throwForce = 20f;
    private Vector3 startpos;
    private float speed = 20f;
    private float arcHeight = 0.7f;
    private Vector3 startPos;

    public Vector3 Target {
        set => target = value;
    }
    
    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        startPos = transform.position;
        Instantiate(startExplosion, transform.position, Quaternion.identity);
        EventManager.TriggerEvent("centaurShot");
        startpos = _rigidbody.position;
    }

    //when the bomb goes too far, destroy it.
    private void Update() {
        if ((startPos - transform.position).magnitude > 60) {
            Destroy(gameObject);
        }
    }
    
    //Find it on internet
    private void FixedUpdate() {
        float x0 = startpos.x;
        float x1 = target.x;
        float dist = x1 - x0;
        float nextX = Mathf.MoveTowards(transform.position.x, x1, speed * Time.deltaTime);
        float baseY = Mathf.Lerp(startpos.y,  target.y, (nextX - x0) / dist);
        float arc = arcHeight * (nextX - x0) * (nextX - x1) / (-0.25f * dist * dist);
        Vector3 nextPos = new Vector3(nextX, baseY + arc, transform.position.z);
        transform.position = nextPos;
		
        if (nextPos == target) {
            Instantiate(contactExplosion,nextPos, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    /*Second method, Linear.
     Vector3 calculateDirection() {
        Vector3 _lookDirection = target - _rigidbody.position;
        //float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        Vector2 direction = _lookDirection.normalized;
        
        Debug.DrawLine(gameObject.transform.position, direction, Color.blue, 5f);
        return direction.normalized;
    }

    public void launch() {
        Vector3 direction = calculateDirection();
        _rigidbody.velocity = calculateDirection() * throwForce;
    }*/

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Wall")
          || other.gameObject.CompareTag("Ground")) {
            Destroy(gameObject);
        }
        if(!other.gameObject.CompareTag("Enemy"))
            Instantiate(contactExplosion, other.contacts[0].point, Quaternion.identity);
        if (other.gameObject.CompareTag("Player")) {
            Physics.IgnoreCollision(other.collider, GetComponent<SphereCollider>());
        }
    }
}
