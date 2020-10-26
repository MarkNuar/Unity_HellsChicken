﻿using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class Target : MonoBehaviour
    {
        
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
        }

        public static Vector3 GetTarget()
        {
            return _target;
        }

    }
}
