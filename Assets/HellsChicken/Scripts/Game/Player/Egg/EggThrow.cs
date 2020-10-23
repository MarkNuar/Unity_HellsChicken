using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

public class EggThrow : MonoBehaviour
{
    private float throwForce = 20f;

    [SerializeField] GameObject eggPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] GameObject crosshair;
    
    private Vector3 target;
    private float angle;
    
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));

        crosshair.transform.position = new Vector2(target.x, target.y);
        
        Vector3 lookDirection = target - firePoint.position;
        angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
        
        if (Input.GetMouseButton(0)) {
            
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            float distance = lookDirection.magnitude;
            Vector2 direction = lookDirection / distance;
            direction.Normalize();
            ThrowEgg(direction);
        }
    }

    void ThrowEgg(Vector2 direction)
    {
        GameObject egg = Instantiate(eggPrefab, firePoint.transform.position, Quaternion.Euler(0.0f, 0.0f, angle));
        Rigidbody rb = egg.GetComponent<Rigidbody>();
        egg.GetComponent<Rigidbody>().velocity = direction * throwForce;
    }
}
