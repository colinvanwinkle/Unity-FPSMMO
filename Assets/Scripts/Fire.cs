/*This class manages the firing of a weapon
 * Calls weapon class to get info about gun.
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Fire : MonoBehaviour {

	//list we will add projectiles to
	public List<Projectile> projectileList = new List<Projectile>();
	bool fireable = true;
	float timeSinceLastFire;
	float timeOfLastFire;
 	Weapon weapon;
	Weapon weaponAtLastFire;
	
	//need to also implement fire speed, reload time, check if has ammo
	void Update () {

	

		//checks if mouse button is clicked				//check if it is held and an automatic (weapon must not be null beacuse we are using weapon reference)
		if ((Input.GetMouseButtonDown (0) || (weapon != null && Input.GetMouseButton(0) && weapon.isAutomatic())) && fireable ) {
			
			//gets the weapon script
			weapon = GetComponent<Weapon> ();



			//gets the characteristics of the current weapon if a new weapon is being used
			if (weaponAtLastFire != weapon) 
				weapon.initWeaponInfo ();


				//we want to exit this update() if user has no weapon
				if (weapon.currentWeapon == null) {
					print ("no weapon");
					return;
				}
				
				//sets this weapon to be last weapon fired
				weaponAtLastFire = weapon;
				fireable = false;

				//gets the current game-time (used for fireSpeed)
				timeOfLastFire = Time.time;

				//gets the camera
				GameObject camera = this.transform.GetChild (0).gameObject;



				//creates a proectile object and adds it to the projectile list that will be processed by moveProjectile.cs
				//initializes the projectile with the gun's specifications
				Projectile projectile = ScriptableObject.CreateInstance ("Projectile") as Projectile;
				projectile.init (weapon.weaponDamage, weapon.range, weapon.bulletSpeed, 
					new Vector3 (camera.transform.position.x, camera.transform.position.y, camera.transform.position.z) + camera.transform.forward, camera.transform.forward);

				projectileList.Add (projectile);
				weapon.ammo = weapon.ammo - 1;
				print (weapon.ammo);

	} // end of user input



		timeSinceLastFire = Time.time - timeOfLastFire;

		//gets the weapon object(have to put this line here because if user hasn't
		//picked up weapon, weapon variable will not be set and this will generate null
		//pointer exception.

		//if we have a weapon, it hasn't been fired since its "cooldown" time, and it has ammo, we can fire it.


		//(eventually we can remove &&weapon.currentweapon because all weapons will have a current weapon)
		if (weapon != null && weapon.currentWeapon != null && timeSinceLastFire >= weapon.fireSpeed && weapon.hasAmmo ()) {
			fireable = true;
		}
		//else, if we have a weapon, but it doesn't have ammo and isn't currently reloading, we should reload it.
		else if (weapon != null  && weapon.currentWeapon != null && !weapon.hasAmmo() && !weapon.isReloading()) {
			weapon.reload();
			fireable = false;
		}


  } //end of update

}
