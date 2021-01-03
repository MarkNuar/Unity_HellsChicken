using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallGenerator : MonoBehaviour {

    [SerializeField] private GameObject _fireBall;
    [SerializeField] private float timer;

    private GameObject _lastFireBall;
    
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(startTimer(timer));
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(_lastFireBall);
        _lastFireBall = Instantiate(_fireBall,transform.position + new Vector3(0,1f,0), Quaternion.identity);
    }

    IEnumerator startTimer(float t) {
        yield return new WaitForSeconds(t);
        _lastFireBall = Instantiate(_fireBall,transform.position + new Vector3(0,1f,0), Quaternion.identity);
        yield return null;
    }
}
