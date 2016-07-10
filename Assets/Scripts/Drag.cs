using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  {
	public GameObject player;
	static int startSlot;
	static Vector3 startPos;
	static GameObject hovered;

	public void OnBeginDrag(PointerEventData eventData){
		startSlot = int.Parse(eventData.pointerDrag.name.Substring(4));
		startPos = this.transform.position;
	}



	public void OnEndDrag(PointerEventData data){
		this.transform.position = startPos;

		//gets the box closest to the mouse when the cursor is let up
		float closestBoxPos = 10000;
		GameObject closestBox = null;

		foreach (Transform box in GameObject.Find("Grid").GetComponentInChildren<Transform>()) {
			if (Vector3.Distance (box.position, Input.mousePosition) < closestBoxPos) {
				closestBoxPos = Vector3.Distance (box.position, Input.mousePosition);
				closestBox = box.gameObject;
			}
		}
		if (Vector3.Distance (GameObject.Find ("WeaponPanel").transform.position, Input.mousePosition) < closestBoxPos) {
			closestBox = GameObject.Find ("WeaponPanel");
			closestBoxPos = Vector3.Distance (GameObject.Find ("WeaponPanel").transform.position, Input.mousePosition);
		}



		//gets the first slot we are going to swap, needed for following if statement
		GameObject slotOwner = GameObject.Find("Slot" + startSlot).GetComponent<Drag>().player;


		//if we drop the item in the waepon spot and we have a weapom in the slotOwner Gameobject, we want to equip the new weapon;
		if (closestBox.name.Equals ("WeaponPanel") && slotOwner.GetComponent<Inventory> ().inventory [startSlot, 0] < 100 && closestBoxPos < 100) {
			slotOwner.GetComponent<Inventory> ().equipNewWeap (startSlot);
		} else if (closestBoxPos < 100) {
			int endSlot = int.Parse (closestBox.name.Substring (4));
			player.GetComponent<Inventory> ().swap (startSlot, endSlot);
		} else {
			player.GetComponent<Inventory> ().removeFromInventory (startSlot);
		}
	}

	
	public void OnDrag(PointerEventData data){
		this.transform.position = Input.mousePosition;

	}





	}
		
	

