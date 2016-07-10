//This class keeps track of IDs and types of all
//game objects.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class IDDict : MonoBehaviour {

    //creates two dictionaries so that we can always get the ID of an object given the name of it,
    //and vice-versa
    public Dictionary<string, int> objectIDs = new Dictionary<string, int>();
    public Dictionary<int, string> objectNames = new Dictionary<int, string>();


  
    //this will hold the IDs of all the objects in our game
    //whenever we want to add an object to the game, we need to add to this list
    void Start()
    {
       addItem(1, "handgun");
       addItem(2, "Rifle");
       addItem(101, "ammo_handgun");
       addItem(102, "ammo_Rifle"); 
     }

    

    //__________________________________________________________________________________________________________________________________________

    //returns whether or not the ID is of type weapon
    public static bool isOfTypeWeapon(int ID)
    {

        //if ID is between 1 and 99, it is weapon
        if (ID > 0 && ID < 100)
            return true;

        return false;
    }

    //adds an item and its id to both dictionaries   
    void addItem(int ID, string name)
    {
        objectNames.Add(ID, name);
        objectIDs.Add(name, ID);
    }

    //gets an objects name given its ID
    public string getObjectNameByID(int ID)
    {
        return objectNames[ID];
    }

    //gets and objects ID given its name
    public int getIDByObjectName(string name)
    {
        return objectIDs[name];
    }

	public static string getItemType(int ID){
		if (ID < 100 && ID > 0)
			return "Weapon";
		else if (ID > 100 && ID < 200)
			return "Item";
		else
			return null;
	}
}
