using HellsChicken.Scripts.Game.Player.Egg;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.FlameStream
{
    public class ParticleCollision : MonoBehaviour
    {
        private void OnParticleCollision(GameObject other) 
        {
            if (other.gameObject.CompareTag("Enemy")) 
            {
                Destruction destr = other.GetComponent<Destruction>();
                
                if(destr == null)    
                    destr = other.gameObject.transform.parent.GetComponent<Destruction>();
                
                destr.Destroyer();
            }
        }
    }
}