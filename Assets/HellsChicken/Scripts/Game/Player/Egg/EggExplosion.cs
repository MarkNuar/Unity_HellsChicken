using System;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class EggExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject explosionEffect;
        
        private bool _hasExploded;
        
        private float radius = 2f;
        private float force = 500f;
        
        private void OnCollisionEnter(Collision other)
        {
            if (!_hasExploded)
            {
                Explode();
                EventManager.TriggerEvent("EggExplosionNotification");
                _hasExploded = true;
            }
        }

        void Explode()
        {
            GameObject particle = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(particle, 1f);
            
            List<String> names =  new List<String>();
            EventManager.TriggerEvent("playBomb");
            
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
                        //}

                        //if (nearbyObject.gameObject.layer == 12 || nearbyObject.gameObject.layer == 13) {
                        //Destruction dest = nearbyObject.GetComponent<Destruction>();
                        if (dest == null) 
                            dest = nearbyObject.gameObject.transform.parent.GetComponent<Destruction>();
                        
                        dest.Destroyer();
                    }
                }
            }

            Destroy(gameObject);
        }
    
    }
}
