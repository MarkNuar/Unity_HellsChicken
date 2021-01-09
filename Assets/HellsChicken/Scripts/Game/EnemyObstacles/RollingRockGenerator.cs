using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRockGenerator : MonoBehaviour
{

    
    public GameObject rollingRockPrefab;
    public float rockSpawnInterval = 10;

    private Rigidbody _rigidbody;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRocks(rockSpawnInterval));
    }


    IEnumerator SpawnRocks(float timer)
    {
        while (true)
        {
            Instantiate(rollingRockPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(timer);
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Lava")) {
            _rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
