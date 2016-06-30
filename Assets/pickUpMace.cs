using UnityEngine;
using System.Collections;

public class pickUpMace : MonoBehaviour {

	public GameObject mace;
	// Use this for initialization

	bool isKeyDown = false;
	float timeStarted = 0f;

	// Update is called once per frame
	void Update () {

		if (GameObject.Find ("mace_model") == null)
			return;
		
		float distance = Vector3.Distance(this.transform.position, GameObject.Find("mace_model").transform.position);
		
		

		if (distance < 3) {

			if (Input.GetKeyDown ("f") && !isKeyDown) {
				isKeyDown = true;
				timeStarted = Time.time;

			}

			if (Input.GetKeyUp ("f") && isKeyDown && Time.time - timeStarted > 0.5) {
				isKeyDown = false;
				timeStarted = 0;

				if (Time.time - timeStarted > 0.5) {
					Destroy (GameObject.Find ("mace_model"));


					Instantiate (mace, Vector3.zero, Quaternion.identity);
					mace = GameObject.Find ("mace(Clone)");
					mace.transform.SetParent (GameObject.Find ("FirstPersonCharacter").transform);

			
					mace.transform.localEulerAngles = new Vector3 (310f, 176f, 180f);
					mace.transform.localPosition = new Vector3 (0.74f, -0.91f, 1.39f);
					mace.transform.localScale = new Vector3 (10f, 10f, 10f);


				}
			} else if (Input.GetKeyUp ("f")) {
				isKeyDown = false;
				timeStarted = 0;
			}



		}

	}




}
