using EventManagerNamespace;
using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
	public class Destruction : MonoBehaviour
	{
		[SerializeField] private GameObject destroyedVersion;
		
		public void Destroyer () 
		{
			if(!gameObject.CompareTag("Player")) 
			{
				if (destroyedVersion != null)
				{
					Transform originalVersion = transform;
					Instantiate(destroyedVersion, originalVersion.position, Quaternion.Euler(0, 180, 0));
				}

				Destroy(gameObject);
			}
			else 
			{
				EventManager.TriggerEvent("StartImmunityCoroutine");
			}
		}
	}
}
