/* This class is responsible for managing picking up weapons from the ground
 */

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class pickUpWeapon : NetworkBehaviour
{

    //weapon being held
    public GameObject activeWeapon;


    //instance variables to track how long key has been pressed
    bool isKeyDown = false;
    float timeStarted = 0f;

    //screen refers to camera object, FPSCharacter
    GameObject screen;
    //refers to the weapon that if 'F' key is held, character will pick it up
    GameObject currentPotentialWeap = null;


    void Start()
    {
        if (!isLocalPlayer)
            return;

        //makes object easier to access in later code
        screen = this.gameObject.transform.GetChild(0).gameObject;
    }

    // Update is called once per frame
    void Update()
    {


        if (!isLocalPlayer)
            return;

        //THIS BLOCK ACKNOWLEDGES WEAPONS ON THE GROUND

        //sends a sphere forward from the camera and returns an array of all the objects hit
        RaycastHit[] weaponsFound = Physics.SphereCastAll(new Ray(screen.transform.position, screen.transform.forward), 1);

        //cycles through all the objects hit and calculates the distance from the character to the weapon
        for (int i = 0; i < weaponsFound.Length; i++)
        {

            //gets distance from player to the wewapon
            float distCharToWeapon = Vector3.Distance(weaponsFound[i].collider.gameObject.transform.position, this.transform.position);

            float distCharToCurrentPotentialWeapon = 0;

            //gets the distance to the weapon at this point in times deemed as the current potential weapoon
            if (currentPotentialWeap != null)
                distCharToCurrentPotentialWeapon = Vector3.Distance(currentPotentialWeap.transform.position, this.transform.position);

            //only continues if we have a weapon object and we are close enough to the object
            if (weaponsFound[i].collider.tag.Equals("Weapon") && distCharToWeapon < 3)
            {

                //changes current potential weapon to this weapon if it is closer to the player than the current potentiual weapon and disregards the weapon that the player is holding 
                if ((currentPotentialWeap == null || distCharToWeapon < distCharToCurrentPotentialWeapon) && activeWeapon != weaponsFound[i].collider.gameObject)
                {
                    currentPotentialWeap = weaponsFound[i].collider.gameObject;

                    print(currentPotentialWeap);

                }


            }
        }


        //______________________________________________________________________________________________________________________________

        //THIS BLOCK IS RESPONSIBLE FOR USER COMMANDS FOR PICKING THE WEAPON UP

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

            //if we currently have a weapon and we have room in our inventory
            //we want to add it to our players inventory
            if (activeWeapon != null && !inventory.isFull())
            {
                //gets the id of the object based on the weapons name
                inventory.addToInventory(GetComponent<IDDict>().getIDByObjectName(currentPotentialWeap.name), 1);
				print ("destroying weap");
                CmdDestroyWeap(currentPotentialWeap);
                currentPotentialWeap = null;

            }
            else if (inventory.isFull())
            {
				print ("inventory is fulL!");
            }
            else
            {	//we have no weapon equipped anwe want to equip this one
                activeWeapon = currentPotentialWeap;
                currentPotentialWeap = null;
                CmdDrawWeap(activeWeapon);

            }



        }

		//resets the variables if the key is let up before the time threshold(.5 sec)
        else if (Input.GetKeyUp("f"))
        {
            isKeyDown = false;
            timeStarted = 0;
        }
			


    }



    [Command]
    public void CmdDrawWeap(GameObject weapon)
    {
        RpcDrawWeap(weapon);
    }


    [ClientRpc]
    void RpcDrawWeap(GameObject weap) {


        //sets the parent of the new weapon 

        weap.transform.SetParent(this.transform.GetChild(0));


        //calibrates the rotation and position of these guns relative to the camera of the user.
        //even though this code executes on every user, this.transform still refers to the Command executer.
        Vector3 weaponRotation = Vector3.zero;
        Vector3 weaponPosition = Vector3.zero;

        switch (weap.name)
        {

            case "mace":
                weaponRotation = new Vector3(310f, 176f, 180f);
                weaponPosition = new Vector3(0.74f, -0.91f, 1.39f);
                break;

            case "handgun":
                weaponRotation = new Vector3(353.8f, 95.5801f, 5.80f);
                weaponPosition = new Vector3(0.95f, -0.89f, 1.68f);
                break;
            case "Rifle":
                weaponRotation = new Vector3(270f, 86.32f, 0f);
                weaponPosition = new Vector3(.4f, -0.47f, 1f);
                break;
        }

        //changes the weapon's properties
        weap.transform.localPosition = weaponPosition;
        weap.transform.localEulerAngles = weaponRotation;

    }

    [Command]
    void CmdDestroyWeap(GameObject weapon)
    {
        RpcDestroyWeap(weapon);
    }

	//Destroys the weap on all clients when it is picked up and put into
	//inventory
    [ClientRpc]
    void RpcDestroyWeap(GameObject weapon)
    {
        Destroy(weapon);
    }



	[Command]
	public void CmdDropToGround(int ID){
		GameObject itemDropped = GameObject.Instantiate (GameObject.Find ("Terrain").GetComponent<PrefabHolder> ().getObject (ID)) as GameObject;
		itemDropped.AddComponent<NetworkIdentity> ();
		//ClientScene.RegisterPrefab (itemDropped, NetworkHash128.Parse(itemDropped.name));
		NetworkServer.Spawn (itemDropped);
		itemDropped.transform.SetParent (this.transform);
		itemDropped.transform.localPosition = new Vector3 (0, 0, 0);

		//RpcDropToGround (itemDropped, ID);

	}

	[ClientRpc]
	void RpcDropToGround(GameObject item, int ID){

		//dropping ammo isnt working, also, picking up weapon again doesn't cause it to destroy
		item.transform.localPosition = new Vector3 (0, 0, 0);
		item.tag = IDDict.getItemType (ID);
		item.AddComponent<Rigidbody> ();
		item.AddComponent<BoxCollider> ();
			item.name = GetComponent<IDDict> ().getObjectNameByID (ID);


		if (item.tag.Equals ("Weapon")) {
			item.transform.SetParent (GameObject.Find ("weapons_on_ground").transform);
		}
		else if (item.tag.Equals("Item"))
			item.transform.SetParent(GameObject.Find("ammo_on_ground").transform);


	}

}



