// --------------------------------------
// This script is totally optional. It is an example of how you can use the
// destructible versions of the objects as demonstrated in my tutorial.
// Watch the tutorial over at http://youtube.com/brackeys/.
// --------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruction : MonoBehaviour
{

	[SerializeField] GameObject destroyedVersion;
	
	public void Destroyer ()
	{
		Instantiate(destroyedVersion, transform.position, transform.rotation);
		
		Destroy(gameObject);
	}

}
