using System.Collections;
using UnityEngine;

namespace HellsChicken.Scripts.Game.EnemyObstacles.Rock
{
    public class DissolvationRock : MonoBehaviour {

        [SerializeField] private Renderer[] rocks;
        [SerializeField] private Material transparent;

        private bool _dissolvation = false;

        private float _interpolation = 0;
        
        public bool Dissolvation {
            get => _dissolvation;
            set => _dissolvation = value;
        }
        
        // Update is called once per frame
        void FixedUpdate(){
            if (_dissolvation) {
                StartCoroutine(DissolvingRock());
                
                // var transparency = Mathf.Lerp(1, 0, _interpolation);
                // foreach (var rock in rocks)
                // {
                //     var color = rock.material.color;
                //     color.a = transparency;
                //     Debug.Log(color);
                //     rock.material.SetFloat("_Alpha",transparency);
                // }
                //
                // _interpolation += 0.1f * Time.fixedDeltaTime;
                // if (_interpolation > 1)
                // {
                //     _dissolvation = false;
                //     _interpolation = 0f;
                //     Destroy(gameObject,.5f);
                // }

                _dissolvation = false;
            }
        }

        IEnumerator DissolvingRock() {
            float i = 0.97f;
            yield return new WaitForSeconds(3f);
            // foreach (var rock in rocks) {
            //     rock.material = transparent;
            // }
            while (i > 0) {
                foreach (var rock in rocks) {
                    //rock.material.SetColor("_BaseColor",new Color(1,1,1,i));
                    var color = rock.material.color;
                    color.a = i;
                    rock.material.color = color;
                }
                i -= 0.03f;
                yield return new WaitForSeconds(0.05f);
            }
            //Destroy(gameObject,1f);
        }
    }
}
