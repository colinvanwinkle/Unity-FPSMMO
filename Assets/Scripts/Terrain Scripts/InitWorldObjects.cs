using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class InitWorldObjects : NetworkBehaviour  {

    Transform weapons_on_ground;
    Transform ammo_on_ground;


    //adds a collider and weapon tag to each weapon spawned
    void Start(){
      weapons_on_ground = GameObject.Find("weapons_on_ground").transform;

            for (int i = 0; i < weapons_on_ground.childCount; i++) {
			Transform weapon = weapons_on_ground.GetChild (i);
			weapon.gameObject.tag= "Weapon";
			weapon.gameObject.AddComponent<BoxCollider> ();
		}


     ammo_on_ground = GameObject.Find("ammo_on_ground").transform;

		//adds a boxcollider and Item tag to each ammo object on the ground
        for (int i = 0; i < ammo_on_ground.childCount; i++)
        {
            Transform ammo = ammo_on_ground.GetChild(i);
            ammo.gameObject.tag = "Item";
            ammo.gameObject.AddComponent<BoxCollider>();
        }



    }
}
