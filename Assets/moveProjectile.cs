using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class moveProjectile : MonoBehaviour {

	 
	void FixedUpdate(){
		foreach (Projectile projectile in GetComponent<Fire>().projectileList){
			projectile.bullet.transform.position += projectile.direction / 100 * projectile.bulletSpeed;
		}
	}
}
