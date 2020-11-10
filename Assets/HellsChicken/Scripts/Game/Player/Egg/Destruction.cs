using System;
using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
	public class Destruction : MonoBehaviour
	{
		
		[SerializeField] GameObject destroyedVersion;
		

		public void Destroyer () {
			if(!gameObject.CompareTag("Player")) {
				if (destroyedVersion != null)
					Instantiate(destroyedVersion, transform.position, transform.rotation);

				Destroy(gameObject);
			}
			else {
				EventManager.TriggerEvent("StartImmunityCoroutine");
			}
		}
	}
}
