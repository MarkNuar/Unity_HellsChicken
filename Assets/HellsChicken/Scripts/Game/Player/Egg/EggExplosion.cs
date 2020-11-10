using System;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class EggExplosion : MonoBehaviour
    {
        [SerializeField] private float timerBeforeEggExplosion = 2.0f;
        
        //private float _countdown;

        //private bool _hasExploded;

        [SerializeField] private GameObject explosionEffect;
        private float radius = 2f;
        private float force = 500f;
    
        // // Start is called before the first frame update
        // void Start()
        // {
        //     _countdown = timerBeforeEggExplosion;
        // }
        //
        // // Update is called once per frame
        // void Update()
        // {
        //     _countdown -= Time.deltaTime;
        //     if (_countdown <= 0f && !_hasExploded)
        //     {
        //         Explode();
        //         EventManager.TriggerEvent("EggExplosionNotification");
        //         _hasExploded = true;
        //     }
        // }

        private void OnCollisionEnter(Collision other)
        {
            Explode();
            //EventManager.TriggerEvent("EggExplosionNotification");
            //_hasExploded = true;
        }

        void Explode()
        {
            GameObject particle = Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(particle, 1f);
            List<String> names =  new List<String>();
        
            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);
            foreach (Collider nearbyObject in collidersToMove) {
                if (!names.Contains(nearbyObject.name)) {
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    Destruction dest = nearbyObject.GetComponent<Destruction>();

                    if (nearbyObject.gameObject.layer == 12) {

                        rb.AddExplosionForce(force, transform.position, radius);
                    }

                    if (nearbyObject.gameObject.layer == 12 || nearbyObject.gameObject.layer == 11) {
                        Destruction destr = nearbyObject.GetComponent<Destruction>();
                        if (destr == null)
                            destr = nearbyObject.gameObject.transform.parent.GetComponent<Destruction>();
                        destr.Destroyer();
                    }
                }
            }

            Destroy(gameObject);
        }
    
    }
}
