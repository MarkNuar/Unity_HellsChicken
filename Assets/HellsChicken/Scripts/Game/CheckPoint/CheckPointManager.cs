using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace HellsChicken.Scripts.Game.CheckPoint
{
    public class CheckPointManager : MonoBehaviour
    {
        //NEVER PLACE TWO CHECKPOINTS IN THE SAME POSITION!
        public Light pointLight;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                GameManager.Instance.SetCurrentCheckPointPos(transform.position);
                //TODO START CHECKPOINT ACTIVATION ANIMATION
                StartCoroutine(CheckPointActivateLight(2f));
            }
        }

        private IEnumerator CheckPointActivateLight(float time)
        {
            pointLight.enabled = true;
            yield return new WaitForSeconds(time);
            pointLight.enabled = false;
            yield return null;
        }
    }
}
