using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    int[,] inventory = new int[16,2];
    int currInvIdx = 0;
    GameObject invText;

    void Start () {
		//gets the text object we will use to print inventory info
        invText = GameObject.Find("InventoryInfo");
    }


    public void addToInventory(int ID, int multiplicity)
    {
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
        }
        //if were adding any other object (ammo, spell)
        else
        {
            inventory[currInvIdx, 0] = ID;
            inventory[currInvIdx, 1]+= multiplicity;
        }

		//finds the next slot index by incrementing the current index until we find a slot with a 0 (no object)
        while (currInvIdx < inventory.GetLength(0) && inventory[currInvIdx, 0] != 0)
            currInvIdx++;


		updateInvText ();



    }


    //removes item at inventory slot from invenorntory
	public void removeFromInventory(string item)
    {
		int itemID = GetComponent<IDDict> ().getIDByObjectName (item);
		int slot = getItemSlot (itemID);

        inventory[slot, 0] = 0;
		inventory [slot, 1] = 0;
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

	public void swap(int start, int end){
		print ("swapped " + start + " with " + end);
		int tempID = inventory [start, 0];
		int tempCount = inventory [start, 1];

		inventory [start, 0] = inventory [end, 0];
		inventory [start, 1] = inventory [end, 1];

		inventory [end, 0] = tempID;
		inventory [end, 1] = tempCount;

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
		//updates inventory info every time we add

		invText.GetComponent<Text>().text = "";

		for (int i = 0; i < inventory.GetLength(0); i++)
		{
			if (inventory[i, 0] != 0)
				invText.GetComponent<Text>().text = invText.GetComponent<Text>().text + "\n" + inventory[i, 0] + " (" + GetComponent<IDDict>().getObjectNameByID(inventory[i, 0]) + ") " + " x " + inventory[i, 1];
		}
	}


}