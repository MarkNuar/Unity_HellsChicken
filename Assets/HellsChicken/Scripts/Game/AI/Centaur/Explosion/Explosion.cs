using UnityEngine;

namespace HellsChicken.Scripts.Game.AI.Centaur.Explosion
{
    public class Explosion : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start(){
            Destroy(gameObject,2f);
        }

    }
}
