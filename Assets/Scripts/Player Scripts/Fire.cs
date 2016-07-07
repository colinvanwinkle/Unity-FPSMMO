/*This class manages the firing of a weapon
 * Calls weapon class to get info about gun.
 * 
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Fire : NetworkBehaviour {

	//list we will add projectiles to
	public List<Projectile> projectileList = new List<Projectile>();
	bool fireable = true;
	float timeSinceLastFire;
	float timeOfLastFire;
 	Weapon weapon;
	Weapon weaponAtLastFire;

	int spreadDegree;
	float timeLastSpreadDec;
	
	//need to also implement fire speed, reload time, check if has ammo
	void Update () {

		if (!isLocalPlayer)
			return;

		//checks if mouse button is clicked				//check if it is held and an automatic (weapon must not be null beacuse we are using weapon reference)
		if ((Input.GetMouseButtonDown (0) || (weapon != null && Input.GetMouseButton (0) && weapon.isAutomatic ())) && fireable) {
			
			//gets the weapon script
			weapon = GetComponent<Weapon> ();



			//gets the characteristics of the current weapon if a new weapon is being used
			if (weapon.currentWeapon != GetComponent<pickUpWeapon> ().activeWeapon) {
				weapon.currentWeapon = GetComponent<pickUpWeapon> ().activeWeapon;
				weapon.initWeaponInfo ();
			}


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

			//add a projectile to every players screen in the game
			CmdAddProjectile (weapon.weaponDamage, weapon.range, weapon.bulletSpeed, camera.transform.position + camera.transform.forward, calculateDir (camera), this.gameObject);

			


				
			weapon.ammo--;
			print ("Ammo: " + weapon.ammo);

		} else if (Input.GetKeyDown ("r") && !weapon.isReloading() && weapon.ammo < weapon.maxAmmoCapacity) {
			weapon.reload ();
			fireable = false;
		}// end of user input

		timeSinceLastFire = Time.time - timeOfLastFire;

		//expands crosshair

        if (weapon != null)
		GameObject.Find ("Crosshair").transform.localScale =  .017f * new Vector3 (spreadDegree * weapon.spreadFactor, spreadDegree * weapon.spreadFactor, 0);


		//gets the weapon object(have to put this line here because if user hasn't
		//picked up weapon, weapon variable will not be set and this will generate null
		//pointer exception.

		//if we have a weapon, it hasn't been fired since its "cooldown" time, and it has ammo, we can fire it.


		//(eventually we can remove &&weapon.currentweapon because all weapons will have a current weapon)
		if (weapon != null && weapon.currentWeapon != null && timeSinceLastFire >= weapon.fireSpeed && weapon.hasAmmo () && !weapon.isReloading()) {
			fireable = true;
		}
		//else, if we have a weapon, but it doesn't have ammo and isn't currently reloading, we should reload it.
		else if (weapon != null  && weapon.currentWeapon != null && !weapon.hasAmmo() && !weapon.isReloading()) {
			weapon.reload();
			fireable = false;
		}


  } //end of update

	void FixedUpdate(){
		//decreases the width of the spray if enough time has passed since last shot
		if (weapon != null && timeSinceLastFire > weapon.spreadThresh && spreadDegree > 0 && Time.time - timeLastSpreadDec > 0.05f){
			spreadDegree--;
		timeLastSpreadDec = Time.time;
		}
			
	}

	//This calculates the random direciton of the bullet based on how quickly the user is firing (bullet spread)
	Vector3 calculateDir(GameObject camera){
		int x;

		//increases the amount of spread if this gun was fired faster than its threshhold
		if (timeSinceLastFire < weapon.spreadThresh && spreadDegree < 15) spreadDegree++;


		//creates two random offset vectors in the up and right direction. 
		if (Random.value < 0.5f) x = -1 ;else x = 1;
		Vector3 vertOffSet = (x  * Random.value * Mathf.Sqrt(spreadDegree * weapon.spreadFactor) / 100 * camera.transform.up);
		if (Random.value < 0.5f) x = -1 ;else x = 1;
		Vector3 HorizOffSet = (x * Random.value * Mathf.Sqrt(spreadDegree * weapon.spreadFactor) / 100 * camera.transform.right);


		//this loop does the same thing as above block, but recalculates if the bullet falls outside the circle defined by 
		//this equation in the while statement
		while (Vector3.Magnitude (vertOffSet) * Vector3.Magnitude (vertOffSet) + Vector3.Magnitude (HorizOffSet) * 
			Vector3.Magnitude (HorizOffSet) > Mathf.Pow (Mathf.Sqrt (spreadDegree * weapon.spreadFactor) / 100, 2)) {
		
			if (Random.value < 0.5f) x = -1 ;else x = 1;
			vertOffSet = (x * Random.value * Mathf.Sqrt(spreadDegree * weapon.spreadFactor) / 100 * camera.transform.up);
			if (Random.value < 0.5f) x = -1 ;else x = 1;
			HorizOffSet = (x * Random.value * Mathf.Sqrt(spreadDegree * weapon.spreadFactor) / 100 * camera.transform.right);
		}

		//returns the offset vector for bullet spray
		return camera.transform.forward + vertOffSet + HorizOffSet;

	}

	//calls on the server to update all the clients info on the projectiles
	[Command]
	void CmdAddProjectile(int dmg, float range, float speed, Vector3 origin, Vector3 dir, GameObject owner){
		RpcAddProjectile( dmg,  range,  speed,  origin,  dir,  owner);
	}

	//creates a projectile and puts it in each users projectile list
	[ClientRpc]
	void RpcAddProjectile(int dmg, float range, float speed, Vector3 origin, Vector3 dir, GameObject owner){
		Projectile projectile = ScriptableObject.CreateInstance ("Projectile") as Projectile;
		projectile.init (dmg, range, speed, origin, dir, owner);

		//this actually gets the Fire.cs script of each client individually, so this is not referring to our local player's script
		//therefore, this adds a projectile to every players projectile list.
		GetComponent<Fire> ().projectileList.Add (projectile);
	
	}

}
