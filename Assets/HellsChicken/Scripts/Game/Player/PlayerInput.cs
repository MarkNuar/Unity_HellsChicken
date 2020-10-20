using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
   public Transform _firePoint;
       public GameObject fireStreamPrefab;
       private bool _canShoot = true;
   
       private void Start()
       {
           _canShoot = true;
       }
   
       // Update is called once per frame
       void Update()
       {
           if (Input.GetButton("Fire1"))
           {
               Shoot();
           }
       }
   
       public void Shoot()
       {
           Instantiate(fireStreamPrefab, _firePoint.position, _firePoint.rotation);
       }
       
}
