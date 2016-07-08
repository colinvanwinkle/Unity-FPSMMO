//this class is responsible for processing all projectiles.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;


public class moveProjectile : NetworkBehaviour {


	//we assign the ID of the object that has bee hit already
	//so we don't call dmg method twice on same bullet
	int hasbeenHitAlready;

	void FixedUpdate(){
			
		//gets the projectile list
		List<Projectile> projectileList = GetComponent<Fire> ().projectileList;


		foreach (Projectile projectile in projectileList.ToArray()){
		

			//if the bullet collided with something and it was destroyed, 
			//remove the projectile containing the bullet from the list and exit this function
			if (projectile.bullet == null){
				projectileList.Remove (projectile);
				return;
			}

			//move the projectile along its path
			projectile.bullet.transform.position += projectile.direction / 100 * projectile.bulletSpeed;

			//render the mesh of the bullet if it's far enough away from the camera.
			if (Vector3.Distance (projectile.bullet.transform.position, projectile.origin) > 2)
					projectile.bullet.GetComponent<MeshRenderer> ().enabled = true;


		
				//destroys the projectile and bullet if it is out of range
				if (Vector3.Distance (projectile.bullet.transform.position, this.transform.position) >= projectile.range) {
					Destroy (projectile.bullet);
					projectileList.Remove (projectile);
				}


	


			//interpolates whether a bullet will hit or not
			RaycastHit rayFromProjectileForward;  
			RaycastHit rayFromProjectileBackward; 
            
			//checks if bullet was in object in last frame
			if (Physics.Raycast (projectile.bullet.transform.position, -projectile.direction, out rayFromProjectileBackward, (float)projectile.bulletSpeed / 200) && rayFromProjectileBackward.collider.tag.Equals ("Enemy") && projectile.GetInstanceID() != hasbeenHitAlready) {
				hasbeenHitAlready = projectile.GetInstanceID ();
                projectile.bullet.GetComponent<detectCollisions>().interpolatedHit(rayFromProjectileBackward.collider);
                Destroy(projectile.bullet);
			//checks if bullet would be in object in next frame
			} else if (Physics.Raycast (projectile.bullet.transform.position, projectile.direction, out rayFromProjectileForward, (float)projectile.bulletSpeed / 200) && rayFromProjectileForward.collider.tag.Equals ("Enemy") && projectile.GetInstanceID() != hasbeenHitAlready) {
				hasbeenHitAlready = projectile.GetInstanceID ();
				projectile.bullet.GetComponent<detectCollisions> ().interpolatedHit (rayFromProjectileForward.collider);
                Destroy(projectile.bullet);

			}
            
		

		}


	}
}
