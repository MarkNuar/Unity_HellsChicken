using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Serialization;

public class Target : MonoBehaviour
{
    
    [SerializeField] GameObject crosshair;
    
    private static Vector3 target;
    
    // Use this for initialization
    void Start () {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        target = transform.GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));

        crosshair.transform.position = new Vector2(target.x, target.y);
    }

    public static Vector3 GetTarget()
    {
        return target;
    }

}
