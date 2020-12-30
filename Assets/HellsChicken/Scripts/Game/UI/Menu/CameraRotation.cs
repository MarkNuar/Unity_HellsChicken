using System.Collections;
using UnityEngine;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class CameraRotation : MonoBehaviour
    {
        public Transform lookAtPoint;
        public Transform rotationCenter;
        public float cameraRotationSpeed = 2.0f;

        // Update is called once per frame
        void Update()
        {
            transform.LookAt(lookAtPoint);
            transform.RotateAround(rotationCenter.position,Vector3.up, cameraRotationSpeed);
        }
    }
}
