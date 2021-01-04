using System;
using EventManagerNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HellsChicken.Scripts.Game.Player
{
    public class DissolveController : MonoBehaviour 
    {
        [SerializeField] private Renderer[] othersMaterials;

        private Material _material;
        private float i = 1;
        private bool _dead;

        public bool Dead 
        {
            set => _dead = value;
        }

        private void Awake() 
        {
            _material = GetComponent<Renderer>().material;
        }

        public void Start() {
            //EventManager.TriggerEvent("chickenSpawnSound");
        }

        // Update is called once per frame
        private void FixedUpdate() 
        {
            if (_dead) 
            {
                if (i < 1) 
                {
                    i += 0.006f;
                    _material.SetFloat("dissolveTime", i);
                    foreach (Renderer r in othersMaterials) 
                    {
                        r.material.SetFloat("dissolveTime", i);
                    }
                }
                else 
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }
            }
            else 
            {
                if (i > 0) 
                {
                    i -= 0.01f;
                    _material.SetFloat("dissolveTime", i);
                
                    foreach (Renderer r in othersMaterials) 
                    {
                        r.material.SetFloat("dissolveTime", i);
                    }

                    if (i <= 0) 
                    {
                        _material.SetFloat("EdgeValue", 0.06f);
                        gameObject.layer = LayerMask.NameToLayer("Player");
                    }
                }
            }
            
            /*if (dead) {
            if (i < 1) {
                i += 0.01f;
                _material.SetFloat("burn", i);
                foreach (Renderer renderer in othersMaterials) {
                    renderer.material.SetFloat("burn", i);
                }
            }
            if (i >= 0.6 & i < 1.6) {
                i += 0.01f;
                _material.SetFloat("vanish", i - 0.6f);

                foreach (Renderer renderer in othersMaterials) {
                    renderer.material.SetFloat("vanish", i - 0.6f);
                }

                /*if (i <= 0) {
                    //GetComponent<PlayerController>().enabled = true;
                    gameObject.layer = LayerMask.NameToLayer("Player");
                }
            }else if (i >= 1.6){
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }*/
            
        }
    }
}    
