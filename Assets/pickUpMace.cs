using UnityEngine;
using System.Collections;

public class pickUpMace : MonoBehaviour {

	public GameObject mace;
	// Use this for initialization

	bool isKeyDown = false;
	float timeStarted = 0f;

	// Update is called once per frame
	void Update () {

		GameObject weapon;

		if (Input.GetKeyDown ("f") && !isKeyDown) {
			isKeyDown = true;
			timeStarted = Time.time;

		}

		if (Input.GetKeyUp ("f") && isKeyDown || (Time.time - timeStarted > 0.5 && timeStarted != 0f && isKeyDown)) {
			isKeyDown = false;
			timeStarted = 0;

			if (Time.time - timeStarted > 0.5) {
				
				Instantiate (mace, Vector3.zero, Quaternion.identity);
				weapon = GameObject.Find ("mace(Clone)");

				weapon.transform.SetParent (this.transform);
				weapon.transform.localPosition = new Vector3 (0.8f, -0.46f, 1.19f);
				weapon.transform.localScale.Scale (new Vector3 (.1f, .1f, .1f));
				Destroy (GameObject.Find ("mace_model"));


			}
		} else if (Input.GetKeyUp ("f")) {
			isKeyDown = false;
			timeStarted = 0;
		}





	}




}
