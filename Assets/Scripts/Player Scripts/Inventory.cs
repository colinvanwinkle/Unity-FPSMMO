using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Inventory : MonoBehaviour {
	//[i,0] represents the objectID, [i,1] represents multiplicity of that object, 
	//and [i,2] represents ammo left in gun, if it is a gun.
    public int[,] inventory = new int[16,3];
    int currInvIdx = 0;



    public void addToInventory(int ID, int multiplicity)
    {

		currInvIdx = 0;
		//finds the next slot index by incrementing the current index until we find a slot with a 0 (no object)
		while (currInvIdx < inventory.GetLength(0) && inventory[currInvIdx, 0] != 0)
			currInvIdx++;
		
        //gets the item slot of the item with the given ID (assuming it 
        //exists in inventory already). If it doesn't exist, create a new 
        //slot for it

        int itemSlot = getItemSlot(ID);

        if (itemSlot != -1 && !IDDict.isOfTypeWeapon(ID))
        {
            inventory[itemSlot, 1] += multiplicity;
        }

        //if were adding a weapon to the inventory
        else if (IDDict.isOfTypeWeapon(ID))
        {
            inventory[currInvIdx, 0] = ID;
            inventory[currInvIdx, 1] = 1;
			inventory [currInvIdx, 2] = Weapon.getMaxAmmo (GetComponent<IDDict> ().getObjectNameByID (ID));
		
        }
        //if were adding any other object (ammo, spell)
        else
        {
            inventory[currInvIdx, 0] = ID;
            inventory[currInvIdx, 1]+= multiplicity;
        }



		updateInvText ();



    }


    //removes item at inventory slot from invenorntory
	public void removeFromInventory(string item)
    {
		int itemID = GetComponent<IDDict> ().getIDByObjectName (item);
		int slot = getItemSlot (itemID);

        inventory[slot, 0] = 0;
		inventory [slot, 1] = 0;
		inventory [slot, 2] = 0;

        currInvIdx = slot;

		updateInvText ();


    }

	//overloaded methods to remove the item from the inventor and drop it on the ground
	public void removeFromInventory(int slot){

		//calls the drop item method in the pickUpItem script
		GetComponent<pickUpItem>().dropItem(inventory[slot,0]);
		inventory [slot, 0] = 0;
		inventory [slot, 1] = 0;
		inventory [slot, 2] = 0;

		currInvIdx = slot;

		updateInvText ();



	}

	//removes speicifed number of items at inventory slot from invenorntory
	public void removeAmountFromInventory(string item, int amount)
	{
		int itemID = GetComponent<IDDict> ().getIDByObjectName (item);
		int slot = getItemSlot (itemID);

		//subtract the amount of items from the slot of the inventory
		inventory [slot, 1] -= amount;

		//if we got rid of more items that we had, reset the inventory slot
		if (inventory [slot, 1] <= 0) {
			inventory [slot, 0] = 0;
			inventory [slot, 1] = 0;
			currInvIdx = slot;

		}

		updateInvText ();
			
	}


	//swaps the two items in the inventory
	public void swap(int start, int end){
		
		int tempID = inventory [start, 0];
		int tempCount = inventory [start, 1];
		int tempAmmo = inventory [start, 2];
			
		inventory [start, 0] = inventory [end, 0];
		inventory [start, 1] = inventory [end, 1];
		inventory [start, 2] = inventory [end, 2];


		inventory [end, 0] = tempID;
		inventory [end, 1] = tempCount;
		inventory [end, 2] = tempAmmo;

		updateInvText ();

	}

	public void equipNewWeap(int newWeapSlot){

		//gets the new weapon from the 
		pickUpWeapon weapon = GetComponent<pickUpWeapon> ();
		//gets the amount of ammo in the weapons slot in the inventory
		int ammoToAdd = inventory [newWeapSlot, 2];

		//swaps the current weapon with the weapon being swapped for current weapon in
		//inventory
		inventory [newWeapSlot, 2] = GetComponent<Weapon> ().ammo;

		//destroys weapon on clients screen (we will change this later as it only updates
		//the client's screen
		Destroy (weapon.activeWeapon);

		//THIS BLOCK IS NOT FUNCTIONAL AT THIS POINT AS IT ONLY SPAWNS A WEAPON ON THE CLIENT'S SCREEN AND DOESN'T EVEN SET ITS POSITION TO 0,0,0
		string nameOfWeap = GetComponent<IDDict> ().getObjectNameByID (inventory [newWeapSlot, 0]);
		GameObject newWeap = (GameObject) GameObject.Instantiate (GameObject.Find ("Terrain").GetComponent<WeapPrefabHolder> ().getObject(nameOfWeap), new Vector3 (0f, 0f, 0f), Quaternion.identity); 
		//we will need to keep this instantiation line

		newWeap.transform.SetParent (this.gameObject.transform);
		newWeap.transform.position = Vector3.zero;
		newWeap.name = nameOfWeap;
		newWeap.AddComponent<NetworkIdentity> ();
		newWeap.GetComponent<NetworkIdentity> ().localPlayerAuthority = true;
		//____________________________________________________________________________________________

		//sets the old slot of new current weapon to the new slot of the old weapon
		inventory [newWeapSlot, 0] = GetComponent<IDDict> ().getIDByObjectName (GetComponent<pickUpWeapon> ().activeWeapon.name);
	
		//sets the new weapon to the weapon we just instantiated 
		weapon.activeWeapon = newWeap;
		GetComponent<Weapon> ().currentWeapon = newWeap;
		GetComponent<Weapon> ().initWeaponInfo (false, ammoToAdd);
			

		GameObject.Find ("WeaponSlot").GetComponent<Text> ().text = "Current Weapon: " + newWeap.name;

		updateInvText ();

	}



    //returns whether or not the inventory is full
    public bool isFull()
    {
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            if (inventory[i, 0] == 0)
                return false;
        }

        return true;

    }


	//returns the contents of the inventory
	public int[,] getInventory()
	{
		return inventory;
	}


	//returns the amount of specified items in inventory 
	public int getNumberOfItem(string item){
		int slotNum = getItemSlot (GetComponent<IDDict> ().getIDByObjectName (item));
		if (slotNum == -1)
			return 0;
		else {
			return inventory [slotNum, 1];
		}

	}




    //HELPER METHODS ----------------------------------------------------------------------------------------------------------------------

	//user this method for reload implementation
     //returns the slot at which the item with given ID is found,
     //return -1 if it is not found.
	private int getItemSlot(int ID)
    {
        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            if (inventory[i,0] == ID)
                return i;
        }
        return -1;
    }

	private void updateInvText(){

		//updates the inventory text
		for (int i = 0; i < inventory.GetLength(0); i++)
		{
			if (inventory [i, 0] != 0)
				GameObject.Find ("Slot" + i).GetComponent<Text> ().text = inventory [i, 0] + " (" + GetComponent<IDDict> ().getObjectNameByID (inventory [i, 0]) + ") " + " x " + inventory [i, 1];
			else
				GameObject.Find ("Slot" + i).GetComponent<Text> ().text = "Empty";
		}
	}


}