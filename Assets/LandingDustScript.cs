using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingDustScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyDust(2f));
    }

    private IEnumerator DestroyDust(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
