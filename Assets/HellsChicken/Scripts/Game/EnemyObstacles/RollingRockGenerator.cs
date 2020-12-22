using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollingRockGenerator : MonoBehaviour
{

    
    public GameObject rollingRockPrefab;
    public int rockSpawnInterval = 10;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnRocks(rockSpawnInterval));
    }


    IEnumerator SpawnRocks(int timer)
    {
        while (true)
        {
            Instantiate(rollingRockPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(timer);
        }
    }
}
