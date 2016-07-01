using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Fire : MonoBehaviour {
	
	public List<Projectile> projectileList = new List<Projectile>();


	void Start () {

	}
	
	//need to also implement fire speed, reload time, check if has ammo
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			GameObject camera = this.transform.GetChild (0).gameObject;

			Weapon weapon = GetComponent<Weapon> ();



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
