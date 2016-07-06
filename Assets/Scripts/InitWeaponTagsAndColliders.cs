using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class InitWeaponTagsAndColliders : NetworkBehaviour  {


	void Start(){
			for (int i = 0; i < this.transform.childCount; i++) {
			Transform weapon = this.transform.GetChild (i);
			weapon.gameObject.tag= "Weapon";
			weapon.gameObject.AddComponent<BoxCollider> ();
		}

	}
}
