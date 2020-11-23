using UnityEngine;

namespace HellsChicken.Scripts.Game.UI.Crosshair
{
    public class FollowMouse : MonoBehaviour
    {
        private Transform _transform;
        
        private void Awake()
        {
            _transform = gameObject.GetComponent<Transform>();
        }

        // Start is called before the first frame update
        private void Start()
        {
            Cursor.visible = false;
        }

        // Update is called once per frame
        private void Update()
        {
            _transform.position = Input.mousePosition;
        }
    }
}
