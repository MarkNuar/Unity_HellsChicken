using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour {

    private Rigidbody _rigidbody;
    private Transform target;
    private Transform centaur = null;
    [SerializeField] private GameObject startExplosion;
    [SerializeField] private GameObject contactExplosion;
    
    private float throwForce = 20f;

    public Transform Target {
        set => target = value;
    }
    
    public Transform Centaur {
        set => centaur = value;
    }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start() {
        _rigidbody.useGravity = false;
        Destroy(gameObject,4f);
    }

    private void Update() {
        if (centaur != null && (centaur.position - transform.position).magnitude > 30) {
            Destroy(gameObject);
        }
    }

    public void launch() {
        _rigidbody.useGravity = true;
        Vector2 direction = calculateDirection();
        Instantiate(startExplosion, transform.position, Quaternion.identity);
        EventManager.TriggerEvent("centaurShot");
        _rigidbody.velocity = direction * throwForce;
    }
    
    Vector3 calculateDirection() {
        Vector3 _lookDirection = target.position - _rigidbody.position;
        float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        Vector2 direction = _lookDirection.normalized;
        return direction.normalized;
    }

    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("wall")
          || other.gameObject.CompareTag("Ground")) {
            Destroy(gameObject);
        }
        if(!other.gameObject.CompareTag("AI"))
            Instantiate(contactExplosion, other.contacts[0].point, Quaternion.identity);
        if (other.gameObject.CompareTag("Player")) {
            Physics.IgnoreCollision(other.collider, GetComponent<SphereCollider>());
        }
    }
}
