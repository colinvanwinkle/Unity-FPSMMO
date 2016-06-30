using UnityEngine;
using System.Collections;

public class pickUpWeapon : MonoBehaviour {

	public GameObject[] weapons; 
	bool isKeyDown = false;
	float timeStarted = 0f;






	// Use this for initialization
	void Start(){
	}

	// Update is called once per frame
	void Update () {



	
		float distance = Vector3.Distance(this.transform.position, GameObject.Find("mace_model").transform.position);

		RaycastHit[] weapons = Physics.SphereCastAll(new Ray(this.transform.position, Vector3.forward), 5);

		for (int i = 0; i < weapons.Length; i++) {
			print (weapons [i].collider.tag);

		}

		//get distance and if it is less than threshold, we can pick it up
		//assign weapon to object with closest distance
		//also, put a sphere at position to visualize where its travelling


		if (distance < 3) {

			if (Input.GetKeyDown ("f") && !isKeyDown) {
				isKeyDown = true;
				timeStarted = Time.time;

			}

			if (isKeyDown && Time.time - timeStarted > 0.5) {
				isKeyDown = false;
				timeStarted = 0;

				if (Time.time - timeStarted > 0.5) {
				//	Destroy (GameObject.Find ("mace_model"));


				//	Instantiate (mace, Vector3.zero, Quaternion.identity);
				//	weapons[0] = GameObject.Find ("mace(Clone)");
				//	weapons[0].transform.SetParent (GameObject.Find ("FirstPersonCharacter").transform);

			
					//weapons[0].transform.localEulerAngles = new Vector3 (310f, 176f, 180f);
				//	weapons[0].transform.localPosition = new Vector3 (0.74f, -0.91f, 1.39f);
				//	weapons[0].transform.localScale = new Vector3 (10f, 10f, 10f);


				}
			} else if (Input.GetKeyUp ("f")) {
				isKeyDown = false;
				timeStarted = 0;
			}



		}

	}




}
