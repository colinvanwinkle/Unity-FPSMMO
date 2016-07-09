//we may or may not use the commented code, probably wuill be one of the last things we do. basically
//it interpolates positions of other plays on the server. 
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.FirstPerson;

public class Player_Network : NetworkBehaviour {

	public GameObject FPSCamera;
	public CharacterController characterController;
	public FirstPersonController FPSController;
	private Transform playerTransform;


	//rotation vars
	[SyncVar] private Quaternion syncPlayerRotation;
	public float rotationLerpRate = 60;
	void Start () {

		//sets the tag of each player spawned in as "Enemy" 
		//this may change in the future as we may add party members and friends
		playerTransform = GetComponent<Transform> ();

		//puts players at this location (will change)
		playerTransform.position = new Vector3 (250, 11, 170);
		this.gameObject.tag = "Enemy";

		//appends a random number to the end of the player name
		int id = Random.Range (1, 100);
		this.name = this.name + id;

		//sets the player in each inventory slot to ours so the inventory slots know who's inventory to get
		for (int i = 0; i < 16; i++) {
			GameObject invSlot = GameObject.Find ("Grid").transform.GetChild (i).gameObject;
			invSlot.GetComponent<Drag>().player = this.gameObject;
		}

		//disables character if we are not that controller
		if (!isLocalPlayer) {
			FPSCamera.GetComponent<Camera> ().enabled = false;
			FPSCamera.GetComponent<AudioListener>().enabled = false;

			characterController.enabled = false;
			FPSController.enabled = false;

		}
	}


}
