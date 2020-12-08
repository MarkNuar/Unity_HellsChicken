using System.Collections;
using EventManagerNamespace;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HellsChicken.Scripts.Game.EnemyObstacles
{
    public class SpearMovement : MonoBehaviour
    {
        private Vector3 _startPosition;

        [SerializeField] private float frequency = 5.0f;
        [SerializeField] private float magnitude = 5.0f;
        [SerializeField] private float offset;
    
        // Start is called before the first frame update
        private void Start()
        {
            _startPosition = transform.position;
        }

        // Update is called once per frame
        private void Update()
        {
            transform.position = _startPosition + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
            EventManager.TriggerEvent("spearSound");
        }
    }
}
