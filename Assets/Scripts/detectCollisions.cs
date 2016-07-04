/*This script attaches the each projectile.sphere that is created.
 */ 

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class detectCollisions : NetworkBehaviour {

	// When a bullet collides with an enemy
	public void OnTriggerEnter(Collider col){

		if (col.gameObject.tag.Equals("Enemy")){

			Hit hit = ScriptableObject.CreateInstance("Hit") as Hit;
			hit.init (col.gameObject, GameObject.Find(this.name.Substring(11)));
			hit.hitRegister ();
		}
			
	}




}
