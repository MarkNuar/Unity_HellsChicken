using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Centaur.Bow
{
    public class BowDirection : MonoBehaviour {
    
        [SerializeField] private GameObject centaur;

        private void LateUpdate()
        {
            Vector3 position = centaur.transform.position;
            transform.position = new Vector3(position.x, position.y, transform.position.z);
        }

        // Start is called before the first frame update
        private void Start(){
            EventManager.StartListening("changeBowDirection",ChangeDirection);
        }

        private void ChangeDirection() {
            EventManager.StopListening("changeBowDirection",ChangeDirection);
            transform.rotation = Quaternion.Euler(0,180,0) * transform.rotation;
            EventManager.StartListening("changeBowDirection",ChangeDirection);
        }
    
    }
}
