/*This class does not attach to any object. it modifies any player
 * that has been or has given a hit by getting the gameObject variables
 */
using UnityEngine;
using System.Collections;

public class Hit : ScriptableObject {
	
	GameObject playerThatGotHit;
	GameObject playerThatHit;

	public void init(GameObject playerThatGotHit, GameObject playerThatHit){
		this.playerThatGotHit = playerThatGotHit;
		this.playerThatHit = playerThatHit;
	}

	public void hitRegister(){

	playerThatGotHit.GetComponent<playerStats> ().playerHit( playerThatHit.GetComponent<Weapon> ().weaponDamage, playerThatHit);
	}


}
