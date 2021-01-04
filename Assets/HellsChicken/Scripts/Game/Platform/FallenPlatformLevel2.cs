using System;
using System.Collections;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class FallenPlatformLevel2 : MonoBehaviour {

        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float amount = 1.0f;
        [SerializeField] private float gravityModifier = 60.0f;
        [SerializeField] private float fallenTime = 1.0f;
        [SerializeField] private float timeShake;
        [SerializeField] private float delay;
        
        private Transform _transform;
        private float _startPosX;
        private float _startPosY;
        private bool _isColliding;
        private bool _shake;
        private bool _waitForCollide = false;
        private bool _done = false;

        public bool WaitForCollide1 {
            get => _waitForCollide;
            set => _waitForCollide = value;
        }

        private void Awake() 
        {
            _transform = transform;
            
            Vector3 position = _transform.position;
            _startPosX = position.x;
            _startPosY = position.y;
        }

        private void Start() 
        {
            StartCoroutine(WaitForShake(timeShake));
        }

        // Update is called once per frame
        private void FixedUpdate() 
        {
            if (!_isColliding && _shake) 
            {
                Vector3 newPos = _transform.position;
                //TODO
                newPos.x = _startPosX + Mathf.Sin(Time.fixedTime * speed) * amount;
                newPos.y = _startPosY + Mathf.Sin(Time.fixedTime * speed) * amount;
                transform.position = newPos;
            }
        }

        private void Update()
        {
            if (_waitForCollide && !_done) {
                _done = true;
                StartCoroutine(WaitForCollide(fallenTime));
            }

            if (_isColliding)
            {
                _transform.position += new Vector3(0f, gravityModifier * Physics.gravity.y, 0f) * Time.deltaTime;
            }
        }

        private IEnumerator WaitForCollide(float seconds) {
            StopCoroutine(nameof(WaitForShake));
            yield return new WaitForSeconds(seconds);
            _isColliding = true;
            yield return new WaitForSeconds(3f); 
            Destroy(gameObject);
            yield return null;
        }

        private IEnumerator WaitForShake(float seconds) 
        {
            yield return new WaitForSeconds(delay);
            while (!_isColliding) 
            {
                yield return new WaitForSeconds(2.0f);
                _shake = true;
                yield return new WaitForSeconds(seconds);
                _shake = false;   
            }
            yield return null;
        }
    }
}
