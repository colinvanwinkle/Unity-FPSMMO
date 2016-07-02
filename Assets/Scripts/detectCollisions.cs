/*This script attaches the each projectile.sphere that is created.
 */ 

using UnityEngine;
using System.Collections;

public class detectCollisions : MonoBehaviour {

	// Use this for initialization
	public void OnTriggerEnter(Collider col){

		if (col.gameObject.tag.Equals("Enemy")){

			Hit hit = ScriptableObject.CreateInstance("Hit") as Hit;
			hit.init (col.gameObject, GameObject.Find(this.name.Substring(11)));
			hit.hitRegister ();
		}
			
	}




}
