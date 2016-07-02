//this class creates projectiles only
//objects are created in the Fire class.
using UnityEngine;
using System.Collections;
	public class Projectile : ScriptableObject {



	public int damage;
	public float range;
	public float bulletSpeed;
	public Vector3 origin;
	public Vector3 direction;
	public GameObject bullet;
	public GameObject owner;


	public void init(int damage, float range, float bulletSpeed, Vector3 origin, Vector3 direction, GameObject owner){
			this.damage = damage;
			this.range = range;
			this.bulletSpeed = bulletSpeed;
			this.origin = origin;
			this.direction = direction;
			this.direction.Normalize();
			this.owner = owner;
			

		bullet = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		bullet.transform.position = origin;
		bullet.transform.localScale = new Vector3 (.1f, .1f, .1f);
		bullet.AddComponent<detectCollisions> ();
		bullet.name = "projectile_" + owner.name;

	
			
			

	}



	}






