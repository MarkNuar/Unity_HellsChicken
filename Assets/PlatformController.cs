using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator anim;
    public void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.CompareTag("Attack"))
            anim.SetTrigger("Destroy");
        
    }
}
