/*This class holds the instance variables for the 
 * characteristics for each weapon. This class is called by
 * the Fire class when mouse is clicked to shoot
 * 
 */

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;



public class Weapon : NetworkBehaviour{
	
	public GameObject currentWeapon;
	public int weaponDamage;
	public float fireSpeed;
	public float bulletSpeed;
	public float reloadTime;
	public int ammo;
	public int maxAmmoCapacity;
	public float range;
    public int ID;
    public string ammoType;
    

	bool isAuto = false;
	bool reloading = false;
	float timeStartReload;

	public float spreadThresh;
	public float spreadFactor;

	// Update is called once per frame
	public void initWeaponInfo () {

		//gets the active weapon from the pickUpWeapon script
		pickUpWeapon weaponScript = GetComponent<pickUpWeapon> ();
		currentWeapon = weaponScript.activeWeapon;


		//assigns the instance variables depending on the weapon
		//every time we add a weapon to the game we need to add to this list
		if (currentWeapon != null) {
			switch (currentWeapon.name) {

			case "handgun":
				fireSpeed = 0.13f;
				reloadTime = 1.5f;
				isAuto = false;
				maxAmmoCapacity = 12;
				range = 40;
				weaponDamage = 12;
				bulletSpeed = 100;
				spreadThresh = .3f;
				spreadFactor = 1;
                ammoType = "ammo_handgun";
                ID = 1;
                
				break;

			case "Rifle":
				fireSpeed = .07f;
				reloadTime = 2.0f;
				isAuto = true;
				maxAmmoCapacity = 20;
				range = 80;
				weaponDamage = 15;
				bulletSpeed = 200;
				spreadThresh = .1f;
				spreadFactor = 5;
                ammoType = "ammo_Rifle";
                ID = 2;
				break;
			}

			ammo = maxAmmoCapacity;


		}
	}

	//checks if reloading is done
	void Update(){
		
		if (reloading) {
			if (Time.time - timeStartReload >= reloadTime) {
				reloading = false;
				ammo = maxAmmoCapacity;
			}
		}
	}


	//reloads the weapon
	public void reload(){
		reloading = true;
		timeStartReload = Time.time;
	}

	//return whether we have ammo
	public bool hasAmmo(){
		if (ammo > 0)
			return true;
		else
			return false;
	}


	//returns whether the weapon is reloading
	public bool isReloading(){
		if (reloading)
			return true;
		else
			return false;
	}

	//returns whether the gun is automatic
	public bool isAutomatic(){
		return isAuto;
	}

	//every time wwe add a weapon to the game we need to do this
    public static int getMaxAmmo(string weapon)
    {
        switch (weapon) { 
            case "handgun":
            	return 12;
			case "Rifle":
                return 20;

    }

        return -1;
    }




}
