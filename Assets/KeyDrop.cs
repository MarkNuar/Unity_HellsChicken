using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

public class KeyDrop : MonoBehaviour
{
    private Rigidbody rb;
    public Transform _landingPosition;

    public Transform bossPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        EventManager.StartListening("keyDrop",triggerKeyDrop);
    }

    public void triggerKeyDrop()
    {
        rb.position = new Vector3(bossPosition.position.x,rb.position.y,rb.position.z);
        rb.isKinematic = false;
    }

    private void Update()
    {
        if (transform.position.y <= _landingPosition.position.y)
            rb.isKinematic = true;
    }
}
