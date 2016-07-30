using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class WeapPrefabHolder : NetworkBehaviour {
	
	public GameObject handgun;
	public GameObject Rifle;
	//public GameObject ammo_Rifle;
	//public GameObject ammo_handgun;

	public GameObject getObject(string obj){

		switch (obj) {
		case "handgun":
			return handgun;
			break;

		case "Rifle":
			return Rifle;
			break;
			/**
		case "ammo_rifle":
			return ammo_Rifle;
			break;

		case "ammo_handgun":
			return ammo_handgun;
			break;

**/
		}

		return null;

	}


	public GameObject getObject(int ID){
		return getObject(GetComponent<IDDict> ().getObjectNameByID (ID));

	}


}
