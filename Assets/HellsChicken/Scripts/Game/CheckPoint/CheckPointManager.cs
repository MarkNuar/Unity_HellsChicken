using System.Collections;
using UnityEngine;

namespace HellsChicken.Scripts.Game.CheckPoint
{
    public class CheckPointManager : MonoBehaviour
    {
        //NEVER PLACE TWO CHECKPOINTS IN THE SAME POSITION!

        [SerializeField] private Material checkPointActivatedMaterial;
        [SerializeField] private Material checkPointDeactivatedMaterial;

        private void Start()
        {
            gameObject.GetComponent<MeshRenderer>().material = checkPointDeactivatedMaterial;
        }
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.SetCurrentCheckPointPos(transform.position);
                //TODO START CHECKPOINT ACTIVATION ANIMATION
                StartCoroutine(CheckPointColorChange(1f));
            }
        }

        private IEnumerator CheckPointColorChange(float time)
        {
            gameObject.GetComponent<MeshRenderer>().material = checkPointActivatedMaterial;
            yield return new WaitForSeconds(time);
            gameObject.GetComponent<MeshRenderer>().material = checkPointDeactivatedMaterial;
            yield return null;
        }
    }
}
