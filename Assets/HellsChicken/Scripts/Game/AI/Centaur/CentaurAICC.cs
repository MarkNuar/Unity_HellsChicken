using System.Collections;
using EventManagerNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class CentaurAICC : MonoBehaviour
{
    private int still = 0; //Variable for checking if I have to be still. 
    private bool movement = true; //Variable for checking if I can be still. 
    private bool right = true;
    
    [SerializeField] private float agentVelocity = 8f;
    [SerializeField] private float timeReaction = 2f; //how often do the agent take decision?
    [SerializeField] private Transform player;
    [SerializeField] private Transform arrowPosition;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private GameObject textPrefab;
    [SerializeField] private int attackTime;

    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private LayerMask mask;

    private CharacterController _characterController;
    private Vector3 _movement; 
    private DecisionTree tree;
    private GameObject textInstance;

    private int shootIntervall = 0;

    private bool isColliding = false;
    
    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start() {
        _movement = Vector3.zero;

        //Decision
        DTDecision d1 = new DTDecision(isPlayerVisible);
        DTDecision d2 = new DTDecision(isPlayerStill);
        
        //Action
        DTAction a1 = new DTAction(hit);
        DTAction a2 = new DTAction(move);
        DTAction a3 = new DTAction(stop);
        
        d1.addLink(true,a1);
        d1.addLink(false,d2);

        d2.addLink(true, a2);
        d2.addLink(false, a3);
        
        //root
        tree = new DecisionTree(d1);

        StartCoroutine(treeCoroutine());
        
    }
    
    private void Update() {
        _movement.y += Physics.gravity.y * gravityScale * Time.deltaTime;
        if (movement) {
            if (!isColliding)
            {
                if (right)
                    _movement.x = agentVelocity;
                else
                    _movement.x = -agentVelocity;
            }
        }else
        {
            _movement.x = 0;
        }
        _characterController.Move(_movement * Time.deltaTime);
    }
    
    IEnumerator treeCoroutine() {
        while (true) {
            tree.start();
            yield return new WaitForSeconds(timeReaction);
        }
    }
    
    //Actions
    public object move() {
        movement = true;
        shootIntervall = 0;
        return null;
    }
    
    public object stop() {
        movement = false;
        shootIntervall = 0;
        return null;
    }

    public object hit() {
        if (shootIntervall == 0) {
            GameObject fire = Instantiate(bombPrefab, arrowPosition.position,
            Quaternion.LookRotation(player.position, transform.position));
            CentaurFire ar = fire.GetComponent<CentaurFire>();
            ar.Target = player.position + new Vector3(0, 0.5f, 0);
            ar.CentaurPos = transform.position;
            ar.findAngle(right,arrowPosition.position);
        }
        shootIntervall = (shootIntervall + 1) % attackTime;
        return null;
    }

    //Decisions
    public object isPlayerVisible() { 
            Vector3 ray = player.position - transform.position;
            //Debug.DrawLine(transform.position,player.position, Color.white, 0.5f);
            RaycastHit hit;
            //if (Physics.Raycast(transform.position, ray, out hit, 30f,mask)) {
            if(Physics.SphereCast(transform.position,0.5f,ray,out hit,30,mask)){
                if (hit.transform.CompareTag("Player")) {
                    if (Vector3.Dot(ray, transform.right) <= 0) {
                        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                        EventManager.TriggerEvent("changeBowDirection");
                        right = !right;
                    }

                    if (textInstance != null)
                        Destroy(textInstance);

                    movement = false;
                    still = 0;
                    return true;
                }
            }

            return false;
        }

    public object isPlayerStill() {
        if (still == 0) {
            if (Random.Range(0, 10) > 6) {
                still = +1;
                textInstance = Instantiate(textPrefab, gameObject.transform.position + new Vector3(-0.4f, 3, 0),
                    Quaternion.identity);
                return false;
            }
            else
                return true;

        }else {
            if (still > 0) {
                still += 1;
                if (still == 4) {
                    if (Random.Range(0, 2) == 0) {
                        transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
                        EventManager.TriggerEvent("changeBowDirection");
                        right = !right;
                    }
                    Destroy(textInstance);
                    still = -4;
                }
                return false;
            }else {
                still += 1;
                return true;
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            isColliding = true;
        }
        if (other.gameObject.CompareTag("Wall")) {
            transform.rotation = transform.rotation * Quaternion.Euler(0, 180, 0);
            EventManager.TriggerEvent("changeBowDirection");
            right = !right;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player")) 
            isColliding = false;
    }
}
