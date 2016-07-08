//this class is responsible for processing all projectiles.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class moveProjectile : NetworkBehaviour {

	int hasbeenHitAlready;

	void FixedUpdate(){

		List<Projectile> projectileList = GetComponent<Fire> ().projectileList;


		//may want to change this to more efficient operation later.
		foreach (Projectile projectile in projectileList.ToArray()){
		
			//moves the projectile in it's direction

			//if the bullet collided with something and it was destroyed, 
			//remove the projectile containing the bullet from the list and exit this function
			if (projectile.bullet == null){
				projectileList.Remove (projectile);
				return;
			}
				
				projectile.bullet.transform.position += projectile.direction / 100 * projectile.bulletSpeed;

				if (Vector3.Distance (projectile.bullet.transform.position, projectile.origin) > 5)
					projectile.bullet.GetComponent<MeshRenderer> ().enabled = true;


		
			
				if (Vector3.Distance (projectile.bullet.transform.position, this.transform.position) >= projectile.range) {
					Destroy (projectile.bullet);
					projectileList.Remove (projectile);
				}


	


			//interpolates whether a bullet will hit or not
			RaycastHit rayFromProjectileForward;  
			RaycastHit rayFromProjectileBackward; 
            
			if (Physics.Raycast (projectile.bullet.transform.position, -projectile.direction, out rayFromProjectileBackward, (float)projectile.bulletSpeed / 200) && rayFromProjectileBackward.collider.tag.Equals ("Enemy") && projectile.GetInstanceID() != hasbeenHitAlready) {
				hasbeenHitAlready = projectile.GetInstanceID ();
                projectile.bullet.GetComponent<detectCollisions>().interpolatedHit(rayFromProjectileBackward.collider);
                Destroy(projectile.bullet);
			} else if (Physics.Raycast (projectile.bullet.transform.position, projectile.direction, out rayFromProjectileForward, (float)projectile.bulletSpeed / 200) && rayFromProjectileForward.collider.tag.Equals ("Enemy") && projectile.GetInstanceID() != hasbeenHitAlready) {
				hasbeenHitAlready = projectile.GetInstanceID ();
				projectile.bullet.GetComponent<detectCollisions> ().interpolatedHit (rayFromProjectileForward.collider);
                Destroy(projectile.bullet);

			}
            
		


		

		}


	}
}
