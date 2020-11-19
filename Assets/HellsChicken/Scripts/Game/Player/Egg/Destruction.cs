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
				{
					Transform originalVersion = transform;
					Instantiate(destroyedVersion, originalVersion.position, originalVersion.rotation);
				}

				Destroy(gameObject);
			}
			else {
				EventManager.TriggerEvent("StartImmunityCoroutine");
			}
		}
	}
}
