﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;


public class pickUpItem : NetworkBehaviour
{

    GameObject screen;
    GameObject currentPotentialItem = null;
    bool isKeyDown = false;
    float timeStarted = 0f;

    void Start()
    {

        if (!isLocalPlayer)
            return;

        screen = this.gameObject.transform.GetChild(0).gameObject;

    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        //THIS BLOCK ACKNOWLEDGES ITEMS ON THE GROUND

        //sends a sphere forward from the camera and returns an array of all the objects hit
        RaycastHit[] itemsFound = Physics.SphereCastAll(new Ray(screen.transform.position, screen.transform.forward), 1);

        //cycles through all the objects hit and calculates the distance from the character to the item
        for (int i = 0; i < itemsFound.Length; i++)
        {

            //gets distance from player to the wewapon
            float distCharToItem = Vector3.Distance(itemsFound[i].collider.gameObject.transform.position, this.transform.position);

            float distCharToCurrentPotentialItem = 0;

            //gets the distance to the weapon at this point in times deemed as the current potential weapoon
            if (currentPotentialItem != null)
                distCharToCurrentPotentialItem = Vector3.Distance(currentPotentialItem.transform.position, this.transform.position);

            //only continues if we have a weapon object and we are close enough to the object
            if (itemsFound[i].collider.tag.Equals("Item") && distCharToItem < 3)
            {

                //changes current potential weapon to this weapon if it is closer to the player than the current potentiual weapon and disregards the weapon that the player is holding 
                if ((currentPotentialItem == null || distCharToItem < distCharToCurrentPotentialItem))
                {
                    currentPotentialItem = itemsFound[i].collider.gameObject;

                    print(currentPotentialItem);

                }


            }
        }


        //______________________________________________________________________________________________________________________________

        //THIS BLOCK IS RESPONSIBLE FOR USER COMMANDS FOR PICKING THE ITEM UP

        //starts timing when 'F' key is pressed down
        if (Input.GetKeyDown("f") && !isKeyDown)
        {
            isKeyDown = true;
            timeStarted = Time.time;

        }

        //if the user holds the 'F' key for more than a half second
        if (isKeyDown && Time.time - timeStarted > 0.5)
        {
            isKeyDown = false;
            timeStarted = 0;


            //gets the inventory of the player
            Inventory inventory = GetComponent<Inventory>();

            if (!inventory.isFull())
            {
                if (currentPotentialItem.name.Length >= 4 && currentPotentialItem.name.Substring(0, 4).Equals("ammo"))
                 {
                    int ammoToAdd= calculateRandomAmmo(currentPotentialItem.name.Substring(5));
                    inventory.addToInventory(GetComponent<IDDict>().getIDByObjectName(currentPotentialItem.name), ammoToAdd);
                    
                 }
                else
                {
                inventory.addToInventory(GetComponent<IDDict>().getIDByObjectName(currentPotentialItem.name), 1);
                }

                CmdDestroyItem(currentPotentialItem);
                Debug.Log("picked up " + currentPotentialItem.name);
                currentPotentialItem = null;

            }
            else
            {
                //TODO - GENERATE ERROR if inventory is full
            }
        }
        //let up too soon
        else if (Input.GetKeyUp("f"))
        {
            isKeyDown = false;
            timeStarted = 0;
        }


        


    }//end of update


	//calculates a random amount of ammo to add to the player's inventory
    int calculateRandomAmmo(string weapon)
    {
       
		return Random.Range(1, Weapon.getMaxAmmo(weapon));
    }

    [Command]
    void CmdDestroyItem(GameObject item)
    {
        RpcDestroyItem(item);
    }

	//destroys the item picked up on all clients
    [ClientRpc]
    void RpcDestroyItem(GameObject item)
    {


        Destroy(item);
    }

	//caled by inventory script when we remove an item
	public void dropItem(int ID){
		
		CmdDropItem (ID);
	}


	//this code is only executed on server
	[Command]
	void CmdDropItem(int ID){

		//gets the prefab from the network manager and loops through the list of prefabs until we have a name match with the ID
		List<GameObject> prefabs = GameObject.Find ("NetworkManager").GetComponent<NetworkManager> ().spawnPrefabs;
		GameObject prefab = null;

		foreach (GameObject obj in prefabs) {
			if (obj.name.Equals (GetComponent<IDDict> ().getObjectNameByID (ID))) {
				prefab = obj;
				break;
		}
	}
			

		//when we find the prefab we want, we NetworkServer.spawn() it on all clients
		//and set the properties of the client
		GameObject item = Instantiate (prefab);

		NetworkServer.Spawn (item);
		RpcDropItem (item, ID);

	
	}

	//this code is executed on all clients
	[ClientRpc]
	void RpcDropItem(GameObject item , int ID){

		//sets the parent to the object of the caller of this method (the person who dropped the item)
		item.transform.SetParent (this.gameObject.transform);
		item.transform.localPosition = new Vector3 (0, 0, 0);
		//sets the tag depending on the item tag
		item.tag = IDDict.getItemType (ID);
		item.AddComponent<BoxCollider> ();
		item.name = GetComponent<IDDict> ().getObjectNameByID (ID);

		//sets the parent of the item depending on its tag (we will probably have to name different types of items
		//appropriate and check the substring to get the actual type other than generic "item" like we do in the second
		//else if statement
		if (item.tag.Equals ("Weapon")) {
			item.transform.SetParent (GameObject.Find ("weapons_on_ground").transform);
		}
		else if (item.tag.Equals("Item") && item.name.Substring(0,4).Equals("ammo"))
			item.transform.SetParent(GameObject.Find("ammo_on_ground").transform);
	}


}



