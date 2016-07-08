/*This script attaches to each projectile.sphere that is created.
 */ 

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class detectCollisions : NetworkBehaviour {

	public int dmg = 0;

	// When a bullet collides with an enemy, call the function that damages a player.
	public void OnCollisionEnter(Collision col){

		//calls the damage player method from the player who was hit's script and passes in dmg and attacker info
		//substring(11) represents the owner of the bullets name....as it is "projectile_<name>"
		if (col.gameObject.tag.Equals("Enemy"))
            col.gameObject.GetComponent<playerStats>().CmddamagePlayer(col.gameObject, GameObject.Find(this.name.Substring(11)), dmg);
		
		Destroy (this.gameObject);
			
	}

	//when we find an interpolated hit using raycast we call this method
    public void interpolatedHit(Collider col)
    {
		//calls the damage player method from the player who was hit's script and passes in dmg and attacker info
		//substring(11) represents the owner of the bullets name....as it is "projectile_<name>"
        if (col.gameObject.tag.Equals("Enemy"))
            col.gameObject.GetComponent<playerStats>().CmddamagePlayer(col.gameObject, GameObject.Find(this.name.Substring(11)), dmg);
           
        Destroy(this.gameObject);
    }




}
