using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryUI : MonoBehaviour
{
    bool inventoryOpen = false;
    GameObject grid;
	GameObject weaponPanel;


	//finds the grid slots (what we use as tiles for items) and sets the parents
    void Start()
    {
        grid = GameObject.Find("Grid");
        grid.transform.SetParent(GameObject.Find("GridHolder").transform);
		weaponPanel = GameObject.Find ("WeaponPanel");
		weaponPanel.transform.SetParent (GameObject.Find ("GridHolder").transform);
    }

	//check if we press i and inventory is not open, if so, disable the cursor and controller and make
	//inventory visible
    void Update()
    {
        if ((Input.GetKeyDown("i") || inventoryOpen) && !(Input.GetKeyDown("i") && inventoryOpen)) {
            inventoryOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            this.gameObject.GetComponent<FirstPersonController>().enabled = false;
            grid.transform.SetParent(GameObject.Find("Canvas").transform);
			weaponPanel.transform.SetParent (GameObject.Find ("Canvas").transform);
        }

		//if inventory is open and we press i we want to do the opposite
        else if (Input.GetKeyDown("i") && inventoryOpen)
        {
            inventoryOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.gameObject.GetComponent<FirstPersonController>().enabled = true;
            grid.transform.SetParent(GameObject.Find("GridHolder").transform);
			weaponPanel.transform.SetParent (GameObject.Find ("GridHolder").transform);

        }
    }

}