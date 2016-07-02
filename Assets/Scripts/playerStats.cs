using UnityEngine;
using System.Collections;


public class playerStats : MonoBehaviour {

	public int health = 100;
	int armor = 0;

	public void playerHit(int damage, GameObject playerThatHitYou){
		health -= damage;
		print (this.gameObject.name + " took " + damage + " from " + playerThatHitYou.name);

		if (health <= 0) {
			Destroy (this.gameObject);
			print (this.gameObject.name + " was killed by " + playerThatHitYou.name);
		}

	}
	

}
