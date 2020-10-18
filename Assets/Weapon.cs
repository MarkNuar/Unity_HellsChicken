using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform _firePoint;
    [SerializeField] [Range(0.1f,5.0f)] private float _waitBetweenFires = 2f;
    public GameObject firePrefab;
    private bool _canShoot = true;

    private void Start()
    {
        _canShoot = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && _canShoot==true)
        {           
            _canShoot = false;
            Shoot();
            StartCoroutine(EnableFire(_waitBetweenFires));
        }
    }

    public void Shoot()
    {
        Instantiate(firePrefab, _firePoint.position, _firePoint.rotation);
    }
    
    IEnumerator EnableFire(float time)
    {
        yield return new WaitForSeconds(time);
        _canShoot = true;
        
        yield return null;
    }
}
