using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour {

    [SerializeField] private float h = 10;
    [SerializeField] private float gravity = -18;
    
    private Rigidbody _rigidbody;
    private Vector3 lastOrientation;
    private Transform target;
    private Transform centaur = null;
    
    private float throwForce = 20f;

    public Transform Target {
        set => target = value;
    }
    
    public Transform Centaur {
        set => centaur = value;
    }

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        lastOrientation = Vector3.left;
    }

    private void Start() {
        _rigidbody.useGravity = false;
        StartCoroutine(timer());
    }

    private void Update() {
        if (centaur != null && (centaur.position - transform.position).magnitude > 20) {
            Destroy(gameObject);
        }
    }

    public void launch() {
        //Physics.gravity = Vector3.up * gravity; 
        _rigidbody.useGravity = true;
        Vector2 direction = calculateDirection();
        _rigidbody.velocity = direction * throwForce;
    }
    
    Vector3 calculateDirection() {
        /*float displacementY = target.position.y - _rigidbody.position.y;
        Vector3 displacementXZ = new Vector3(target.position.x - _rigidbody.position.x,0,
            target.position.z - _rigidbody.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * h); //radice di -2gh
        Vector3 velocityXZ = displacementXZ / (float)(Math.Sqrt(-2 * h/gravity) + Math.Sqrt(2 * (displacementY - h) / gravity));

        return velocityXZ + velocityY;*/
        Vector3 _lookDirection = target.position - _rigidbody.position;
        float angle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        Vector2 direction = _lookDirection.normalized;
        return direction.normalized;
        /*GameObject egg = Instantiate(eggPrefab, eggThrowPoint.transform.position, Quaternion.Euler(0.0f, 0.0f, angle));
        egg.GetComponent<Rigidbody>().velocity = direction * throwForce;*/
    }

    IEnumerator timer() {
        yield return new WaitForSeconds(4f);
        Destroy(gameObject);
        yield return null;
    }
    
    public void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("wall")
          || other.gameObject.CompareTag("Ground")) {
            Destroy(gameObject);
        }
    }
}
