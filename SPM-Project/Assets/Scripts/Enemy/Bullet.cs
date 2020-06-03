using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class Bullet : MonoBehaviour {

	[SerializeField] [Tooltip("How fast the bullet will move.")] private float projectileSpeed;
	private Vector3 travelVector; //Movement vector the bullet should travel.
	private TrailRenderer trail; //The bullet trail component.
	[SerializeField] [Tooltip("The layers that the bullet should be able to intersect with.")] private LayerMask bulletHitLayer;
	//[SerializeField] [Tooltip("Particle effect to play on impact.")] private GameObject hitEffect;
	private EnemyWeaponBase owner; //The weapon that fired this bullet.
	private GameObject player;

	/// <summary>
	/// Gives the Bullet some number of starting values through parameters.
	/// </summary>
	/// <param name="shootDir"> The vector the bullet should travel along.</param>
	/// <param name="owner">The enemy weapon this bullet was fired from.</param>
	public void Initialize(Vector3 shootDir, EnemyWeaponBase owner) {
		travelVector = shootDir - transform.position;
		this.owner = owner;
	}

	private void Start() {
		trail = GetComponent<TrailRenderer>();
		trail.enabled = true;
		player = PlayerController.Instance.gameObject;
	}

	private void Update() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, travelVector, out hit, (travelVector.normalized * projectileSpeed * Time.fixedDeltaTime).magnitude, bulletHitLayer)) {
			if (hit.collider.gameObject.Equals(player)){
				EventSystem.Current.FireEvent(new DamageEvent(hit.collider.GetComponent<IEntity>(), owner) );
			}

			//GameObject hitGO = Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal)); //Play the hit effect by instantiating/destroying the particle system.
			//Destroy(hitGO, 0.5f);
			gameObject.SetActive(false);
		}
		transform.position += travelVector.normalized * projectileSpeed * Time.fixedDeltaTime;
	}

}