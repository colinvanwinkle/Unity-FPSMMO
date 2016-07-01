/*This class holds the instance variables for the 
 * characteristics for each weapon. This class is called by
 * the Fire class when mouse is clicked to shoot
 * 
 */

using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {
	
	public GameObject currentWeapon;
	public int weaponDamage;
	public float fireSpeed;
	public float bulletSpeed;
	public float reloadTime;
	public int ammo;
	public float range;
	public bool isAuto;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//gets the active weapon from the pickUpWeapon script
		pickUpWeapon weaponScript = GetComponent<pickUpWeapon> ();
		currentWeapon = weaponScript.activeWeapon;


		//assigns the instance variables depending on the weapon
		if (currentWeapon != null)
		switch (currentWeapon.name) {

			case "handgun":
				fireSpeed = 0.25f;
				reloadTime = 1.5f;
				isAuto = false;
				ammo = 12;
				range = 20;
				weaponDamage = 15;
				bulletSpeed = 50;
			break;

		}

	}
}
