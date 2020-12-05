using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class EggExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject explosionEffect;
        
        private bool _hasExploded;
        
        [SerializeField] private float radius = 3f;
        [SerializeField] private float force = 500f;


        private bool _hasCollided = false;
        
        private void OnCollisionEnter()
        {
            if (!_hasCollided)
            {
                _hasCollided = true;
                if (!_hasExploded)
                {
                    gameObject.GetComponent<MeshRenderer>().enabled = false;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
                    Explode();
                    EventManager.TriggerEvent("EggExplosionNotification");
                    _hasExploded = true;
                }
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                if (!_hasCollided)
                {
                    _hasCollided = true;
                    if (!_hasExploded)
                    {
                        gameObject.GetComponent<MeshRenderer>().enabled = false;
                        gameObject.GetComponent<SphereCollider>().enabled = false;
                        Explode();
                        EventManager.TriggerEvent("EggExplosionNotification");
                        _hasExploded = true;
                    }
                }
            }
        }

        private void Explode()
        {
            Transform egg = transform;
            GameObject particle = Instantiate(explosionEffect, egg.position, egg.rotation);
            Destroy(particle, 1f);
            
            List<String> names =  new List<String>();
            EventManager.TriggerEvent("EggExplosion");
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
            
            foreach (Collider nearbyObject in colliders) 
            {
                if (!names.Contains(nearbyObject.name)) 
                {
                    names.Add(nearbyObject.name);
                    
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    Destruction dest = nearbyObject.GetComponent<Destruction>();

                    if (nearbyObject.gameObject.layer == 12 || nearbyObject.gameObject.layer == 13) 
                    {
                        if(rb != null)
                            rb.AddExplosionForce(force, transform.position, radius);
                        
                        if (dest == null) 
                            dest = nearbyObject.gameObject.transform.parent.GetComponent<Destruction>();
                        else
                            dest.Destroyer();
                    }
                }
            }
            
            
            StartCoroutine(DelayDestroyEgg(2.0f));
        }

        private IEnumerator DelayDestroyEgg(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
            yield return null;
        }
    }
}
