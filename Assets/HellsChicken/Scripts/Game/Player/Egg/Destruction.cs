using UnityEngine;

namespace HellsChicken.Scripts.Game.Player.Egg
{
	public class Destruction : MonoBehaviour
	{

		[SerializeField] GameObject destroyedVersion;
	
		public void Destroyer ()
		{
			Instantiate(destroyedVersion, transform.position, transform.rotation);
		
			Destroy(gameObject);
		}

	}
}
