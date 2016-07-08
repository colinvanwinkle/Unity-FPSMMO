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


	[SyncVar] private Vector3 syncPosition;
	private Transform playerTransform;
	public float movementLerpRate = 60;


	//rotation vars
	[SyncVar] private Quaternion syncPlayerRotation;
	public float rotationLerpRate = 60;
	void Start () {

		//sets the tag of each player spawned in as "Enemy" 
		//this may change in the future as we may add party members and friends
		playerTransform = GetComponent<Transform> ();
		playerTransform.position = new Vector3 (250, 11, 170);
		this.gameObject.tag = "Enemy";

		//appends a random number to the end of the player name
		int id = Random.Range (1, 100);
		this.name = this.name + id;
		//playerStats.players.Add (this.gameObject.name, 1000);


		//disables character if we are not that controller
		if (!isLocalPlayer) {
			FPSCamera.GetComponent<Camera> ().enabled = false;
			FPSCamera.GetComponent<AudioListener>().enabled = false;

			characterController.enabled = false;
			FPSController.enabled = false;

		}
	}
	/*
	void FixedUpdate(){
		LerpPosition ();
		sendPosition ();
		LerpRotation ();
		sendRotation ();
	}

	private void LerpPosition(){
		if (!isLocalPlayer) {
			playerTransform.position = Vector3.Lerp (playerTransform.position, syncPosition, Time.deltaTime * movementLerpRate);
		}
	}

	[Command]
	private void CmdProvidePosToServer(Vector3 Position){
		syncPosition = Position;
	}

	[ClientCallback]
	private void sendPosition(){
		if (isLocalPlayer)
			CmdProvidePosToServer (playerTransform.position);
	}

	//FOR ROTATION
	private void LerpRotation(){

		if (!isLocalPlayer) {
			playerTransform.rotation = Quaternion.Lerp (playerTransform.rotation, syncPlayerRotation, Time.deltaTime * rotationLerpRate);
		}
	}


	[Command]
	private void CmdProvideRotationToServer(Quaternion playerRotation){
		syncPlayerRotation = playerRotation;
	}


	[ClientCallback]
	private void sendRotation(){
		if (!isLocalPlayer)
			CmdProvideRotationToServer (playerTransform.rotation);
	}
*/
	// Update is called once per frame

}
