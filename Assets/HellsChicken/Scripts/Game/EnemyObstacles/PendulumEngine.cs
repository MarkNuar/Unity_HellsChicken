using System.Collections;
using EventManagerNamespace;
using UnityEditor.Rendering;
using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles
{
    public class PendulumEngine : MonoBehaviour
    {
        private Quaternion _start;
        private Quaternion _end;

        [SerializeField] private float startAngle = 90.0f;
        [SerializeField] private float speed = 2.0f;
        
        [SerializeField] private float startTime;
        
        // Start is called before the first frame update
        private void Start()
        {
            _start = PendulumRotation(startAngle);
            _end = PendulumRotation(-startAngle);
        }

        private void FixedUpdate()
        {
            startTime += Time.deltaTime;
            transform.rotation =
                Quaternion.Lerp(_start, _end, (Mathf.Sin(startTime * speed + Mathf.PI / 2) + 1.0f) / 2.0f); 
            
            EventManager.TriggerEvent("pendulumSound");

        }

        private Quaternion PendulumRotation(float angle)
        {
            Quaternion pendulumRotation = transform.rotation;
            float rotation = pendulumRotation.eulerAngles.z + angle;

            if (rotation > 180) 
                rotation -= 360;
            
            if (rotation < -180) 
                rotation += 360;
            
            pendulumRotation.eulerAngles = new Vector3(pendulumRotation.eulerAngles.x, pendulumRotation.eulerAngles.y, rotation);

            return pendulumRotation;
        }
        
    }
}
