using System.Collections;
using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class DissolvationRock : MonoBehaviour {

        [SerializeField] private Renderer[] rocks;
        [SerializeField] private Material transparent;

        private bool _dissolvation = false;

        public bool Dissolvation {
            get => _dissolvation;
            set => _dissolvation = value;
        }
        
        // Update is called once per frame
        void Update(){
            if (_dissolvation) {
                StartCoroutine(DissolvingRock());
                _dissolvation = false;
            }
        }

        IEnumerator DissolvingRock() {
            float i = 0.97f;
            yield return new WaitForSeconds(5f);
            foreach (var rock in rocks) {
                rock.material = transparent;
            }
            while (i > 0) {
                foreach (var rock in rocks) {
                    rock.material.SetColor("_BaseColor",new Color(1,1,1,i));
                }

                i -= 0.03f;
                yield return new WaitForSeconds(0.05f);
            }
            Destroy(gameObject,1f);
        }
    }
}
