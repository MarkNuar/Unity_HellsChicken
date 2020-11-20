using System;
using System.Collections;
using System.Collections.Generic;
using HellsChicken.Scripts.Game.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DissolveController : MonoBehaviour {

    [SerializeField] private Renderer[] othersMaterials;

    private Material _material;
    private float i = 0;
    private bool dead = false;


    public bool Dead {
        get => dead;
        set => dead = value;
    }

    private void Awake() {
        _material = GetComponent<Renderer>().material;
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void FixedUpdate() {
        /*if (dead) {
            if (i < 1) {
                i += 0.006f;
                _material.SetFloat("dissolveTime", i);
                foreach (Renderer renderer in othersMaterials) {
                    renderer.material.SetFloat("dissolveTime", i);
                }
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }else {
            if (i > 0) {
                i -= 0.008f;
                _material.SetFloat("dissolveTime", i);
                
                foreach (Renderer renderer in othersMaterials) {
                    renderer.material.SetFloat("dissolveTime", i);
                }

                if (i <= 0) {
                    //GetComponent<PlayerController>().enabled = true;
                    gameObject.layer = LayerMask.NameToLayer("Player");
                }
            }
        }*/
        if (dead) {
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
                }*/
            }else if (i >= 1.6){
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}    
