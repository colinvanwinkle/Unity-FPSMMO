using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDropHandler  {
	public GameObject player;
	int startSlot;

	public void OnBeginDrag(PointerEventData eventData){
		startSlot = int.Parse(eventData.pointerDrag.name.Substring(4));
		print (startSlot);
	}

	public void OnDrop(PointerEventData data){

		//check if it is the item we are dropping on
		RaycastResult ray = data.pointerCurrentRaycast;
		int endSlot = int.Parse(ray.gameObject.name.Substring (4));
		print (endSlot);
		player.GetComponent<Inventory> ().swap (startSlot, endSlot);

	}
	
}
