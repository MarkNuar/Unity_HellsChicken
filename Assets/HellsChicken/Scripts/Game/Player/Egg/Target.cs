using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class Target : MonoBehaviour
    {
        private static Vector3 _target;
        private Camera _camera;
        
        // Use this for initialization
        void Start () {
            //Cursor.visible = false;
            _camera = GetComponent<Camera>();
        }
        
        public Vector3 GetTarget()
        {
            return _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));
        }

    }
}
