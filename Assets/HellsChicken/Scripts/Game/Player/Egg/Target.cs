using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class Target : MonoBehaviour
    {
    
        [SerializeField] GameObject crosshair;
    
        private static Vector3 _target;

        private Camera _camera;
        
        // Use this for initialization
        void Start () {
            Cursor.visible = false;
            _camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void Update()
        {
            _target = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));

            crosshair.transform.position = new Vector2(_target.x, _target.y);
        }

        public static Vector3 GetTarget()
        {
            return _target;
        }

    }
}
