using UnityEngine;
using System.Collections;

public class pickUpWeapon : MonoBehaviour {

	public GameObject activeWeapon;




	bool isKeyDown = false;
	float timeStarted = 0f;



	GameObject screen;
	GameObject currentPotentialWeap = null;

	// Use this for initialization
	void Start(){

		screen = GameObject.Find("FirstPersonCharacter");
	


	
	}

	// Update is called once per frame
	void Update () {

	



		RaycastHit[] weaponsFound = Physics.SphereCastAll(new Ray(screen.transform.position, screen.transform.forward), 1);

		for (int i = 0; i < weaponsFound.Length; i++) {
			float distCharToWeapon = Vector3.Distance (weaponsFound [i].collider.gameObject.transform.position, this.transform.position);

			float distCharToCurrentPotentialWeapon = 0;

				if (currentPotentialWeap != null)
				distCharToCurrentPotentialWeapon = Vector3.Distance (currentPotentialWeap.transform.position, this.transform.position);

				if  ( weaponsFound[i].collider.tag.Equals ("Weapon") && distCharToWeapon < 3 ) {

				if ((currentPotentialWeap == null || distCharToWeapon < distCharToCurrentPotentialWeapon) && activeWeapon != weaponsFound[i].collider.gameObject) {
					currentPotentialWeap = weaponsFound [i].collider.gameObject;
					
					print (currentPotentialWeap);

				}


			}
		}

	


	

			if (Input.GetKeyDown ("f") && !isKeyDown) {
				isKeyDown = true;
				timeStarted = Time.time;

			}

			if (isKeyDown && Time.time - timeStarted > 0.5) {
				isKeyDown = false;
				timeStarted = 0;

				if (Time.time - timeStarted > 0.5) {

				activeWeapon = currentPotentialWeap;

				activeWeapon.transform.SetParent (screen.transform);
				activeWeapon.transform.localEulerAngles = new Vector3 (310f, 176f, 180f);
				activeWeapon.transform.localPosition = new Vector3 (0.74f, -0.91f, 1.39f);

				currentPotentialWeap = null;



				}
			} else if (Input.GetKeyUp ("f")) {
				isKeyDown = false;
				timeStarted = 0;
			}



		

	}




}
