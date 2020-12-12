using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class Target : MonoBehaviour
    {
        private static Vector3 _target;
        private Camera _camera;
        private GameObject mainMenuCamera;
        
        // Use this for initialization
        private void Start () 
        {
            _camera = GetComponent<Camera>();
            
            mainMenuCamera = GameObject.FindWithTag("MainMenuCamera");
            if (!(mainMenuCamera == null))
            {
                _camera.GetComponent<UniversalAdditionalCameraData>().antialiasing =
                    mainMenuCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing;
                _camera.GetComponent<UniversalAdditionalCameraData>().renderShadows =
                    mainMenuCamera.GetComponent<UniversalAdditionalCameraData>().renderShadows;
                Destroy(mainMenuCamera);
            }
        }
        
        public Vector3 GetTarget()
        {
            return _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -transform.position.z));
        }

    }
}
