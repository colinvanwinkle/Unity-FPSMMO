using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class InventoryUI : MonoBehaviour
{
    bool inventoryOpen = false;
    GameObject grid;

    void Start()
    {
        grid = GameObject.Find("Grid");
        grid.transform.SetParent(GameObject.Find("GridHolder").transform);
    }
    void Update()
    {
        if ((Input.GetKeyDown("i") || inventoryOpen) && !(Input.GetKeyDown("i") && inventoryOpen)) {
            inventoryOpen = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.lockState = CursorLockMode.Confined;
            this.gameObject.GetComponent<FirstPersonController>().enabled = false;
            grid.transform.SetParent(GameObject.Find("Canvas").transform);

        }
        else if (Input.GetKeyDown("i") && inventoryOpen)
        {
            inventoryOpen = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            this.gameObject.GetComponent<FirstPersonController>().enabled = true;
            grid.transform.SetParent(GameObject.Find("GridHolder").transform);

        }
    }

}