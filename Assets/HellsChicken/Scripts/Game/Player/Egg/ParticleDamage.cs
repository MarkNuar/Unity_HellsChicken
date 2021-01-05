using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
    public class ParticleDamage : MonoBehaviour {

        [SerializeField] private float radiusExplosion;
        [SerializeField] private float force;

        private void OnParticleCollision(GameObject other) {
            EventManager.TriggerEvent("playBomb");
           
            //Rigidbody rb = other.GetComponent<Rigidbody>();
            Destruction dest = other.GetComponent<Destruction>();

            if (other.gameObject.layer == 12 || other.gameObject.layer == 13) {
                //  if(rb != null)
                //      rb.AddExplosionForce(force, transform.position, radiusExplosion);
                        
                if (dest == null) 
                    dest = other.gameObject.transform.parent.GetComponent<Destruction>();
                        
                dest.Destroyer();

            }

        }

        private void OnParticleTrigger() {
            print("AA");
        }

        private void OnTriggerEnter(Collider other) {
            print("AA");
        }
    }
}
