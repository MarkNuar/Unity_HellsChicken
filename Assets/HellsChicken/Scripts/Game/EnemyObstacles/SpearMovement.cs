using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles
{
    public class SpearMovement : MonoBehaviour
    {

        private Vector3 _startPosition;

        private float frequency = 5.0f;
        private float magnitude = 5.0f;
        private float offset = 0.0f;
    
        // Start is called before the first frame update
        void Start()
        {
            _startPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = _startPosition + transform.up * Mathf.Sin(Time.time * frequency + offset) * magnitude;
        }
    }
}
