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

	
	//need to also implement fire speed, reload time, check if has ammo
	void Update () {

		//checks if mouse button is pressed down
		if (Input.GetMouseButtonDown (0)) {

			//gets the camera
			GameObject camera = this.transform.GetChild (0).gameObject;

			//gets the weapon script
			Weapon weapon = GetComponent<Weapon> ();


			//creates a proectile object and adds it to the projectile list that will be processed by moveProjectile.cs
			//initializes the projectile with the gun's specifications
			Projectile projectile = ScriptableObject.CreateInstance("Projectile") as Projectile;
			projectile.init (weapon.weaponDamage, weapon.range, weapon.bulletSpeed, 
				new Vector3 (camera.transform.position.x, camera.transform.position.y, camera.transform.position.z), camera.transform.forward);

			projectileList.Add (projectile);



	
		}


	}

	void printWeaponInfo(Weapon weapon){
		print (weapon.ammo);
		print (weapon.range);
		print (weapon.isAuto);
		print (weapon.reloadTime);
		print (weapon.bulletSpeed);
		print (weapon.fireSpeed);
		print (weapon.weaponDamage);
	}

}
