using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DissolveController : MonoBehaviour {

    [SerializeField] private float fadeStartTime = 0.01f;
    [SerializeField] private float fadeEndTime = 0.01f;
    [SerializeField] private Renderer[] othersMaterials;
    
    private Material _material;
    private float i = 1;
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
    void Update(){
        if (dead) {
            if (i < 1) {
                i += fadeEndTime;
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
                i -= fadeStartTime;
                _material.SetFloat("dissolveTime", i);
                foreach (Renderer renderer in othersMaterials) {
                    renderer.material.SetFloat("dissolveTime", i);
                }
            }
        }
    }
}
