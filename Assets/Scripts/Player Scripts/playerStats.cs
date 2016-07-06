using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.UI;
public class playerStats : NetworkBehaviour {

	//when we make inventory we might store an array for health, inventory [0] = health
	//[1] = #of grenades (type1), [2] = # of grenades (type 2)
	 public static Dictionary<string, int> players = new Dictionary<string, int>();



	//sends command to all clients to damage this player
	[Command]
	public void CmddamagePlayer(GameObject playerHit, GameObject playerThatHit, int dmg){
		RpcdamagePlayer (playerHit, playerThatHit, dmg);


	}

	//finds the player's health in the dictionary and deudcts the dmg from it.
	//This command is executed on all clients.
	[ClientRpc]
	public void RpcdamagePlayer(GameObject playerHit, GameObject playerThatHit, int dmg){


		players [playerHit.name]-= dmg;

		//updates the fake ass scoreboard players and their healths, does its job for now
		GameObject[] playerList = GameObject.FindGameObjectsWithTag ("Enemy");
		GameObject.Find ("Text").GetComponent<Text> ().text = "";
		foreach (GameObject player in playerList) {
			GameObject.Find ("Text").GetComponent<Text> ().text = GameObject.Find ("Text").GetComponent<Text> ().text + "\n" + player.name + ": " + players[player.name];
		}

		if (players [playerHit.name] <= 0)
			Destroy (playerHit);
		
	}

}
