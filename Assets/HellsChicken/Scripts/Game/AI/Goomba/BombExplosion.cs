﻿using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Goomba
{
    public class BombExplosion : MonoBehaviour
    {
        [SerializeField] private GameObject explosionPrefab;
        [SerializeField] private GameObject vanishEffectPrefab;
        public GameObject DegoroAnimation;
        
        private MeshRenderer _meshRenderer;
        private int _cont;
        
        [SerializeField] private float radius = 5;
        [SerializeField] private float force = 500f;
        
        private void Awake() 
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        // Start is called before the first frame update
        void Start() 
        {
            //Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playTimerBomb");
            StartCoroutine(MakeExplosion());
        }

        private void Update()
        {
            _meshRenderer.material.color = _cont == 0 ? Color.red : Color.white;
            _cont = (_cont + 1) % 2;
        }

        //Wait for 2 second and then make the bomb explode.
        private IEnumerator MakeExplosion() 
        {  
            yield return new WaitForSeconds(2);
            
            Vector3 position = transform.position;
            Instantiate(explosionPrefab, position - new Vector3(0,1,0), Quaternion.identity);
            
            List<String> names =  new List<String>();
            
            Collider[] collidersToMove = Physics.OverlapSphere(position, radius);
            
            foreach (Collider nearbyObject in collidersToMove) 
            {
                if (!names.Contains(nearbyObject.name))
                {
                    names.Add(nearbyObject.gameObject.name);
                    
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    Destruction dest = nearbyObject.GetComponent<Destruction>();

                    if (!nearbyObject.gameObject.CompareTag("Enemy"))
                    {
                        if (rb != null)
                            rb.AddExplosionForce(force, transform.position, radius);

                        if (dest != null)
                            dest.Destroyer();
                    }
                }
            }
            EventManager.TriggerEvent("GoombaExplosion");
            DegoroAnimation.SetActive(false);
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(DelayDestroyGoomba(2.0f));
        }

        private IEnumerator DelayDestroyGoomba(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
            yield return null;
        }
    }
}
