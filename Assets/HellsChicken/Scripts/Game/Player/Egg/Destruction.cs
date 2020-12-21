using System.Collections;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
	public class Destruction : MonoBehaviour
	{
		[SerializeField] private GameObject destroyedVersion;
		public Animator anim;
		private bool isDead;
		
		public void Destroyer () 
		{
			if(!gameObject.CompareTag("Player")) 
			{
				if (destroyedVersion != null)
				{
					Transform originalVersion = transform;
					Instantiate(destroyedVersion, originalVersion.position, Quaternion.Euler(0, 180, 0));
				}

				StartCoroutine(CentaurDeath(3f));
			}
			else 
			{
				EventManager.TriggerEvent("StartImmunityCoroutine");
			}
		}

		IEnumerator CentaurDeath(float time)
		{
			isDead = true;
			anim.SetBool("isDead",isDead);
			gameObject.GetComponent<CapsuleCollider>().enabled = false;
			gameObject.GetComponent<CharacterController>().enabled = false;
			EventManager.TriggerEvent("CentaurDeath");
			yield return new WaitForSeconds(time);
			Destroy(gameObject);
		}

		public bool IsDead
		{
			get => isDead;
		}
	}
}
