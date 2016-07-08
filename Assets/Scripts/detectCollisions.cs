/*This script attaches the each projectile.sphere that is created.
 */ 

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class detectCollisions : NetworkBehaviour {

	public int dmg = 0;

	// When a bullet collides with an enemy, call the function that damages a player.
	public void OnCollisionEnter(Collision col){


		//substring(11) represents the owner of the bullets name....as it is "projectile_<name>"
		if (col.gameObject.tag.Equals("Enemy")){
            print("direct hit!");
            col.gameObject.GetComponent<playerStats>().CmddamagePlayer(col.gameObject, GameObject.Find(this.name.Substring(11)), dmg);
		}

		Destroy (this.gameObject);
			
	}

    public void interpolatedHit(Collider col)
    {
        //substring(11) represents the owner of the bullets name....as it is "projectile_<name>"
        if (col.gameObject.tag.Equals("Enemy"))
        {
            print("interpolated hit!");
            col.gameObject.GetComponent<playerStats>().CmddamagePlayer(col.gameObject, GameObject.Find(this.name.Substring(11)), dmg);
            
        }

        Destroy(this.gameObject);
    }




}
