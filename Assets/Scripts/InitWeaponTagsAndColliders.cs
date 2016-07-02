using UnityEngine;
using System.Collections;

public class InitWeaponTagsAndColliders : MonoBehaviour {


	void Start(){

		print ("hi");
		for (int i = 0; i < this.transform.childCount; i++) {
			Transform weapon = this.transform.GetChild (i);
			weapon.gameObject.tag= "Weapon";
			weapon.gameObject.AddComponent<BoxCollider> ();
			weapon.gameObject.AddComponent<Rigidbody> ();
		}

	}
}
