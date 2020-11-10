using System;
using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class RockObstacle : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            //TODO: IF HIT BY THE EGG, IT SHOULD FALL WITHOUT DESTROYING ITSELF
            /*
            if (other.gameObject.CompareTag("Attack"))
            {
                _rockRb.isKinematic = false;
                _rockRb.AddForce(new Vector3(0, gravityModifier * Physics.gravity.y, 0),ForceMode.Acceleration);
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                Destroy(gameObject);
            }
            else if(other.gameObject.CompareTag("Ground"))
            {
                Destroy(gameObject, 5.0f);
            }
            */
            
            Destroy(gameObject);
        }
    }
}
