using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class FallenPlatform : MonoBehaviour {

    [SerializeField] private float _speed = 1.0f;
    [SerializeField] private float _amount = 1.0f;
    [SerializeField] private float _gravityModifier = 60.0f;



    private Transform _transform;
    private Rigidbody _rigidbody;
    private float _startPosX;
    private float _startPosY;
    private bool isColliding = false;
    private bool shake = false;
    
    private void Awake() {
        _transform = GetComponent<Transform>();
        _rigidbody = GetComponent<Rigidbody>();
        _startPosX = _transform.position.x;
        _startPosY = _transform.position.y;
        EventManager.StartListening("platformCollide",platformCollide);
    }

    private void Start() {
        StartCoroutine(waitForShake(0.5f));
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (!isColliding && shake) {
            Vector3 newPos = _transform.position;
            //TODO
            newPos.x = _startPosX + Mathf.Sin(Time.fixedTime * _speed) * _amount;
            newPos.y = _startPosY + Mathf.Sin(Time.fixedTime * _speed) * _amount;
            transform.position = newPos;
        }else if (isColliding){
           _rigidbody.AddForce(new Vector3(0, _gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
           Destroy(gameObject,3f); 
        }
    }

    IEnumerator waitForCollide(float seconds) {
        yield return new WaitForSeconds(seconds);
        isColliding = true;
        yield return null;
    }

    IEnumerator waitForShake(float seconds) {
        while (!isColliding) {
            yield return new WaitForSeconds(2f);
            shake = true;
            yield return new WaitForSeconds(seconds);
            shake = false;   
        }
        yield return null;
    }

    public void platformCollide(String name) {
        EventManager.StopListening("platformCollide",platformCollide);
        if (name.Equals(gameObject.name)) {
            StartCoroutine(waitForCollide(0.5f));
        }
        EventManager.StartListening("platformCollide",platformCollide);
    }
}
