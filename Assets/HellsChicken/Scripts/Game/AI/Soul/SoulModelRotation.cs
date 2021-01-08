using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Soul
{
    public class SoulModelRotation : MonoBehaviour
    {

        [SerializeField] private SoulAI soulController;
    
        private Quaternion _leftRotation;
        private Quaternion _rightRotation;
        private Quaternion _frontRotation;
    
        // Start is called before the first frame update
        void Start()
        {
            _leftRotation = transform.rotation * Quaternion.Euler(0, -30,0);
            _rightRotation = _leftRotation * Quaternion.Euler(0, -120, 0);
            _frontRotation = transform.rotation * Quaternion.Euler(0, -90, 0);
            transform.rotation = _leftRotation;
        }

        // Update is called once per frame
        void Update()
        {
            if (soulController.IsLookingLeft())
            {
                transform.rotation = _leftRotation;
            }
            else if(soulController.IsLookingRight())
            {
                transform.rotation = _rightRotation;
            }
            else
            {
                transform.rotation = _frontRotation;
            }
        }
    }
}
