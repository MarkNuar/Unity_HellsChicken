using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;

public class DissolvationRock : MonoBehaviour {

    [SerializeField] private Renderer[] rocks;
    [SerializeField] private Material transarent;

    private bool dissolvation = false;

    public bool Dissolvation {
        get => dissolvation;
        set => dissolvation = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        if (dissolvation) {
            StartCoroutine(dissolvingRock());
            dissolvation = false;
        }
    }

    IEnumerator dissolvingRock() {
        float i = 0.97f;
        yield return new WaitForSeconds(2f);
        foreach (var rock in rocks) {
            rock.material = transarent;
        }
        while (i > 0) {
            foreach (var rock in rocks) {
                rock.material.SetColor("_BaseColor",new Color(1,1,1,i));
            }

            i -= 0.03f;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject,1f);
    }
}
