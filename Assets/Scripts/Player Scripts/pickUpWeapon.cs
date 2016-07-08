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
    [SyncVar]
    GameObject model;
    // Use this for initialization
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
                CmdDestroyWeap(currentPotentialWeap);
                currentPotentialWeap = null;

            }
            else if (inventory.isFull())
            {
                //todo
            }
            else
            {
                activeWeapon = currentPotentialWeap;
                currentPotentialWeap = null;
                CmdDrawWeap(activeWeapon);

            }



            //resets the variables if the key is let up before the time threshold(.5 sec)
        }
        else if (Input.GetKeyUp("f"))
        {
            isKeyDown = false;
            timeStarted = 0;
        }





    }


    [Command]
    void CmdDrawWeap(GameObject weapon)
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
                weaponRotation = new Vector3(-5.33f, 85.32f, 0.9f);
                weaponPosition = new Vector3(.2f, -0.47f, 1.5f);
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

    [ClientRpc]
    void RpcDestroyWeap(GameObject weapon)
    {
        Destroy(weapon);
    }



}



