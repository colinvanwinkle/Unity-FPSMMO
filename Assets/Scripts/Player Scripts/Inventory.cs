using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    int[,] inventory = new int[16,2];
    int currInvIdx = 0;
    GameObject invText;

    void Start () {
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

        while (currInvIdx < inventory.GetLength(0) && inventory[currInvIdx, 0] != 0)
            currInvIdx++;



        invText.GetComponent<Text>().text = "";

        for (int i = 0; i < inventory.GetLength(0); i++)
        {
            if (inventory[i, 0] != 0)
                invText.GetComponent<Text>().text = invText.GetComponent<Text>().text + "\n" + inventory[i, 0] + " (" + GetComponent<IDDict>().getObjectNameByID(inventory[i, 0]) + ") " + " x " + inventory[i, 1];
        }


    }


    //removes item at inventory slot from invenorntory
    public void removeFromInventory(int inventorySlot)
    {
       
        inventory[inventorySlot, 0] = 0;
        currInvIdx = inventorySlot;

    }


    //returns the contents of the inventory
    public int[,] getInventory()
    {
        return inventory;
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


    //HELPER METHODS ----------------------------------------------------------------------------------------------------------------------


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
}
