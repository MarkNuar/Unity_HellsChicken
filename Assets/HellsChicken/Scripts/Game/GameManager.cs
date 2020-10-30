using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    [SerializeField] private GameObject initialCheckPoint;
    
    private Vector3 _currentCheckPointPos;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
            SetCurrentCheckPointPos(initialCheckPoint.transform.position);
            //Destroy(initialCheckPoint);//maybe initial checkpoint has to be destroyed after consumed.
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Vector3 GetCurrentCheckPointPos()
    {
        return _currentCheckPointPos;
    }
    
    public void SetCurrentCheckPointPos(Vector3 newCheckPointPos)
    {
        _currentCheckPointPos = newCheckPointPos;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
