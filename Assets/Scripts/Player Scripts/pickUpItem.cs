using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

//NOTES. IMPLEMENTS NEW METHOD OF FINDING WEAPON (USING X # OF RAYS TO SEE WHICH OBJECT IS IN FIELD OF VIEW MORE)
//FOR SPAWNING AMMO,WAEPONS, MAKE THEM BE AT ALL LOCATIONS BUT DISABLE THEIR MESHES

    //EVERY TIME WE ADD AMMO, WE NEED TO UPDATE IDDICT AND MAXAMMO() IN WEAPON
    //EVERY TIME WE ADD A WEAPON, WE NEED TO UPDATE CASE: IN WEAPON AND IDDICT.CS
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

        //cycles through all the objects hit and calculates the distance from the character to the weapon
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
                //TODO - GENERATE ERROR
            }
        }
        //let up too soon
        else if (Input.GetKeyUp("f"))
        {
            isKeyDown = false;
            timeStarted = 0;
        }


        


    }//end of update

    int calculateRandomAmmo(string weapon)
    {
        int maxAmmo = (Random.Range(1, Weapon.getMaxAmmo(weapon)));
        print("max ammo is " + maxAmmo);
        return maxAmmo;
    }

    [Command]
    void CmdDestroyItem(GameObject item)
    {
        RpcDestroyItem(item);
    }

    [ClientRpc]
    void RpcDestroyItem(GameObject item)
    {
        Destroy(item);
    }
}



