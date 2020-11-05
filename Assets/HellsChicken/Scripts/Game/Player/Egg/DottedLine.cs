using System;
using System.Collections.Generic;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class DottedLine : MonoBehaviour
    {
        // Inspector fields
        public Sprite dot;
        [Range(0.01f, 1f)]
        public float size;
        [Range(0.1f, 2f)]
        public float delta;

        public int numberOfPointsForParabola = 10;
        
        //Static Property with backing field
        private static DottedLine _instance;
        
        public static DottedLine Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<DottedLine>();
                return _instance;
            }
        }

        //Utility fields
        List<Vector2> positions = new List<Vector2>();
        List<GameObject> dots = new List<GameObject>();

        // Update is called once per frame
        void FixedUpdate()
        {
            if (positions.Count > 0)
            {
                DestroyAllDots();
                positions.Clear();
            }
        }

        private void DestroyAllDots()
        {
            foreach (var dot in dots)
            {
                Destroy(dot);
            }
            dots.Clear();
        }
        
        public void DrawDottedLine(Vector2 start, Vector2 end)
        {
            DestroyAllDots();

            Vector2 point = start;
            Vector2 direction = (end - start).normalized;

            while ((end - start).magnitude > (point - start).magnitude)
            {
                positions.Add(point);
                point += (direction * delta);
            }
            Render();
        }
        
        public void DrawDottedParabola(Vector2 start, float velMagn,float angle)
        {
            DestroyAllDots();

            Vector2 point = start;
            
            for (var i = 0; i < numberOfPointsForParabola; i++)
            {
                positions.Add(point);
                
                var grav = - Physics.gravity.y;
                var curX = point.x + delta;
                var deltaX = curX - start.x;
                var curY =start.y + Mathf.Tan(angle) * deltaX - grav * deltaX * deltaX /
                    (2 * velMagn * velMagn * Mathf.Cos(angle) * Mathf.Cos(angle));
                
                point = new Vector2(curX, curY );
            }
            Render();
        }

        private void Render()
        {
            foreach (var position in positions)
            {
                var g = GetOneDot();
                g.transform.position = position;
                dots.Add(g);
            }
        }
        
        GameObject GetOneDot()
        {
            var gameObject = new GameObject();
            gameObject.transform.localScale = Vector3.one * size;
            gameObject.transform.parent = transform;

            var sr = gameObject.AddComponent<SpriteRenderer>();
            sr.sprite = dot;
            return gameObject;
        }
    }
}
