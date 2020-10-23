using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggExplosion : MonoBehaviour
{
    private float timer = 2f;
    private float countdown;

    private bool hasExploded;

    [SerializeField] GameObject explosionEffect;
    private float radius = 5f;
    private float force = 500f;
    
    // Start is called before the first frame update
    void Start()
    {
        countdown = timer;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        GameObject particle = Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(particle, 1f);
        
        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(force, transform.position, radius);
            }
        }
        
        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, radius);
        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Destruction dest = nearbyObject.GetComponent<Destruction>();
            if (dest != null)
            {
                dest.Destroyer();
            }
        }

        Destroy(gameObject);
    }
}
