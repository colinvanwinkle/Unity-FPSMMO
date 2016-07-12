//We will keep track of the players health and perhaps other info here.
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
public class playerStats : NetworkBehaviour {


	//syncvar variables are updated to all clients when they change on the server
    [SyncVar]
    private int health = 1000;


	//sends command to all clients to damage this player
	[Command]
	public void CmddamagePlayer(GameObject playerHit, GameObject playerThatHit, int dmg){
		//subtract the daamge from the players health (since this is a SyncVar it will be pushed to all clients)
        health -= dmg;
		RpcdamagePlayer (playerHit, playerThatHit, dmg);

	}

	//this is called on all clients
	[ClientRpc]
	public void RpcdamagePlayer(GameObject playerHit, GameObject playerThatHit, int dmg){


		//updates the fake ass scoreboard players and their healths, does its job for now
		GameObject[] playerList = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject.Find ("Text").GetComponent<Text> ().text = "";
		foreach (GameObject player in playerList) {
			GameObject.Find ("Text").GetComponent<Text> ().text = GameObject.Find ("Text").GetComponent<Text> ().text + "\n" + player.name + ": " + player.GetComponent<playerStats>().health;
		}
        

			//destroys player if its health is 0
            if (health <= 0)
			Destroy (playerHit);
		
	}

}
