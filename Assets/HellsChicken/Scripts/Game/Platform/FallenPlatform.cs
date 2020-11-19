using System;
using System.Collections;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Platform
{
    public class FallenPlatform : MonoBehaviour {

        [SerializeField] private float speed = 1.0f;
        [SerializeField] private float amount = 1.0f;
        [SerializeField] private float gravityModifier = 60.0f;
        
        private Transform _transform;
        private Rigidbody _rigidbody;
        private float _startPosX;
        private float _startPosY;
        private bool _isColliding;
        private bool _shake;
    
        private void Awake() {
            _transform = GetComponent<Transform>();
            _rigidbody = GetComponent<Rigidbody>();
            
            Vector3 position = _transform.position;
            _startPosX = position.x;
            _startPosY = position.y;
            
            EventManager.StartListening("platformCollide",PlatformCollide);
        }

        private void Start() {
            StartCoroutine(WaitForShake(0.5f));
        }

        // Update is called once per frame
        void FixedUpdate() {
            if (!_isColliding && _shake) {
                Vector3 newPos = _transform.position;
                //TODO
                newPos.x = _startPosX + Mathf.Sin(Time.fixedTime * speed) * amount;
                newPos.y = _startPosY + Mathf.Sin(Time.fixedTime * speed) * amount;
                transform.position = newPos;
            }else if (_isColliding){
                _rigidbody.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
                Destroy(gameObject,3f); 
            }
        }

        IEnumerator WaitForCollide(float seconds) {
            yield return new WaitForSeconds(seconds);
            _isColliding = true;
            yield return null;
        }

        IEnumerator WaitForShake(float seconds) {
            while (!_isColliding) {
                yield return new WaitForSeconds(2f);
                _shake = true;
                yield return new WaitForSeconds(seconds);
                _shake = false;   
            }
            yield return null;
        }

        private void PlatformCollide(String stringName) {
            EventManager.StopListening("platformCollide",PlatformCollide);
            if (stringName.Equals(gameObject.name)) {
                StartCoroutine(WaitForCollide(0.5f));
            }
            EventManager.StartListening("platformCollide",PlatformCollide);
        }
    }
}
