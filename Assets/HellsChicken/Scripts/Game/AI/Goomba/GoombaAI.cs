using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/*
 * AI of Goomba. Implemented as continue left - right movement. When he dies, he leaves a bomb that will explode in some seconds.
 */
public class GoombaAI : MonoBehaviour {
    
    private Rigidbody _rigidbody;
    private bool right = true;
    private float agentVelocity = 8f;
    
    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        print(right);
    }

    private void FixedUpdate() {
        if(right)
            _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
        else
            _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("wall")) {
            right = !right;
        }
    }
}
