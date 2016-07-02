//this class is responsible for processing all projectiles.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveProjectile : MonoBehaviour {

	int hasbeenHitAlready;

	void FixedUpdate(){

		List<Projectile> projectileList = GetComponent<Fire> ().projectileList;


		//may want to change this to more efficient operation later.
		foreach (Projectile projectile in projectileList.ToArray()){
		
			projectile.bullet.transform.position += projectile.direction / 100 * projectile.bulletSpeed;
			if (Vector3.Distance (projectile.bullet.transform.position, this.transform.position) >= projectile.range) {
				Destroy (projectile.bullet);
				projectileList.Remove (projectile);
			}


			RaycastHit rayFromProjectileForward;  
			RaycastHit rayFromProjectileBackward; 

			if (Physics.Raycast (projectile.bullet.transform.position, -projectile.direction, out rayFromProjectileBackward, (float)projectile.bulletSpeed / 200) && rayFromProjectileBackward.collider.tag.Equals ("Enemy") && projectile.GetInstanceID() != hasbeenHitAlready) {
				hasbeenHitAlready = projectile.GetInstanceID ();
				projectile.bullet.GetComponent<detectCollisions> ().OnTriggerEnter (rayFromProjectileBackward.collider);
			} else if (Physics.Raycast (projectile.bullet.transform.position, projectile.direction, out rayFromProjectileForward, (float)projectile.bulletSpeed / 200) && rayFromProjectileForward.collider.tag.Equals ("Enemy") && projectile.GetInstanceID() != hasbeenHitAlready) {
				hasbeenHitAlready = projectile.GetInstanceID ();
				projectile.bullet.GetComponent<detectCollisions> ().OnTriggerEnter (rayFromProjectileForward.collider);

			}


		


		

		}


	}
}
