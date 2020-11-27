using UnityEngine;

namespace HellsChicken.Scripts.Game
{
    public class SkyBoxRotation : MonoBehaviour {

        private Material _skyBoxMaterial;
        private float _angle;
        
        private static readonly int Rotation = Shader.PropertyToID("_Rotation");

        private void Awake() {
            _skyBoxMaterial = RenderSettings.skybox;
        }
        
        // Update is called once per frame
        private void Update() {
            _angle = (_angle + 0.03f) % 360;
            _skyBoxMaterial.SetFloat(Rotation, _angle);
        }
    }
}
