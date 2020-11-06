using EventManagerNamespace;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

/*
 * AI of Goomba. Implemented as continue left - right movement. When he dies, he leaves a bomb that will explode in some seconds.
 */
public class GoombaAI : MonoBehaviour {
    
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject vanishEffectPrefab;

    private Rigidbody _rigidbody;
    private bool right = true;
    private float agentVelocity = 8f;
    private bool isColliding = false;
    
    public Vector3 position, velocity;
    
    public void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        if (!isColliding) {
            if (right)
                _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.right);
            else
                _rigidbody.MovePosition(_rigidbody.position + agentVelocity * Time.fixedDeltaTime * Vector3.left);
            position = _rigidbody.position;
            velocity = _rigidbody.velocity;
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            //Physics.IgnoreCollision(other.collider,GetComponent<CapsuleCollider>());
            isColliding = true;
        }else if (other.gameObject.CompareTag("Wall")) {
            
            right = !right;
            
        }else if (other.gameObject.CompareTag("EnemyShot")) {
            
            //creates the effect and starts the sound
            Instantiate(vanishEffectPrefab, transform.position, Quaternion.identity);
            EventManager.TriggerEvent("playTimerBomb");
            
            //create the bomb the right new position
            Vector3 newPosition = new Vector3(gameObject.transform.position.x,1,gameObject.transform.position.z);
            Instantiate(bombPrefab,newPosition,Quaternion.Euler(0,0,90));
            
            //destroy the Goomba
            Destroy(gameObject);
            
        }
    }
    
    void LateUpdate()
    {
        if (isColliding)
        {
            _rigidbody.position = position;
            _rigidbody.velocity = velocity;
        }
    }
    
    void OnCollisionExit(Collision collision){
        if (collision.collider.tag == "Player")
            isColliding = false;
    }

}
