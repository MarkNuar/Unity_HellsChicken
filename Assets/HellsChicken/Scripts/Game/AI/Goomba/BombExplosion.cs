using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Goomba
{
    public class BombExplosion : MonoBehaviour
    {

        [SerializeField] private GameObject explosionPrefab;
        
        private Color red = Color.red;
        private Color white = Color.white;

        private MeshRenderer _meshRenderer;
        private int i = 0;
        private float radius =5;
        private float force = 500f;
        
        private void Awake() {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        // Start is called before the first frame update
        void Start() {
            StartCoroutine(MakeExplosion());
        }

        private void Update() {
            if(i == 0)
                _meshRenderer.material.color = red;
            else 
                _meshRenderer.material.color = white;
            i = (i + 1) % 2;

        }

        //Wait for 2 second and then make the bomb explode.
        IEnumerator MakeExplosion() {  
            yield return new WaitForSeconds(1);
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playBomb");
            
            Collider[] collidersToMove = Physics.OverlapSphere(transform.position, radius);
            List<String> names =  new List<String>();
            
            foreach (Collider nearbyObject in collidersToMove) {
                if (!names.Contains(nearbyObject.name) && !nearbyObject.gameObject.CompareTag("Enemy")) {
                    Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                    if (rb != null) {
                        rb.AddExplosionForce(force, transform.position, radius);
                    }

                    Destruction dest = nearbyObject.GetComponent<Destruction>();
                    if (dest != null) {
                        dest.Destroyer();
                    }

                    names.Add(nearbyObject.gameObject.name);
                }
            }
            
            Destroy(gameObject);
            yield return null;
        }
    
    }
}
