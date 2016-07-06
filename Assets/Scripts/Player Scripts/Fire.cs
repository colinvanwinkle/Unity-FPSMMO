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
		if ((Input.GetMouseButtonDown (0) || (weapon != null && Input.GetMouseButton(0) && weapon.isAutomatic())) && fireable ) {
			
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

			


				
				weapon.ammo = weapon.ammo - 1;
				print ("Ammo: " + weapon.ammo);

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

	void FixedUpdate(){
		if (weapon != null && timeSinceLastFire > weapon.spreadThresh && spreadDegree > 0 && Time.time - timeLastSpreadDec > .1f){
			spreadDegree--;
		timeLastSpreadDec = Time.time;
		}
			
	}

	//This calculates the random direciton of the bullet based on how quickly the user is firing (bullet spread)
	Vector3 calculateDir(GameObject camera){
		int x;

		//increases the amount of spread if this gun was fired faster than its threshhold
		if (timeSinceLastFire < weapon.spreadThresh && spreadDegree < 15) spreadDegree++;


		//creates two random offset vectors in the up and right direction. (makes a square potentially, need to changet this to circle);
		if (Random.value < 0.5f) x = -1 ;else x = 1;
		Vector3 vertOffSet = (x * weapon.spreadFactor * Random.value * Mathf.Sqrt(spreadDegree) / 100 * camera.transform.up);
		if (Random.value < 0.5f) x = -1 ;else x = 1;
		Vector3 HorizOffSet = (x * weapon.spreadFactor * Random.value * Mathf.Sqrt(spreadDegree) / 100 * camera.transform.right);

		//while (Vector3.Magnitude(vertOffSet) * Vector3.Magnitude(vertOffSet) + Vector3.Magnitude(vertOffSet) * Vector3.Magnitude(vertOffSet) > Mathf.Pow(Mathf.Sqrt(spreadDegree) / 100,2))
			

		Debug.DrawRay (camera.transform.position, camera.transform.forward + vertOffSet + HorizOffSet, Color.green, 6);
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
