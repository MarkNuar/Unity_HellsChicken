using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallGenerator : MonoBehaviour {

    [SerializeField] private GameObject _fireBall;

    private GameObject _lastFireBall;
    
    // Start is called before the first frame update
    void Start() {
        _lastFireBall = Instantiate(_fireBall,transform.position + new Vector3(0,1f,0), Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other) {
        Destroy(_lastFireBall);
        _lastFireBall = Instantiate(_fireBall,transform.position + new Vector3(0,1f,0), Quaternion.identity);
    }
}
