using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField] float projectileSpeed;
	private Vector3 travelVector;
	private TrailRenderer trail;
	[SerializeField] private LayerMask bulletHitLayer;
	[SerializeField] private GameObject hitEffect;
	private EnemyWeaponBase owner;

	public float ProjectileSpeed { get => projectileSpeed; }

	/// <summary>
	/// Gives the Bullet some number of starting values through parameters.
	/// </summary>
	/// <param name="shootDir"> The vector the bullet should travel along.</param>
	public void Initialize(Vector3 shootDir, EnemyWeaponBase owner) {
		travelVector = shootDir - transform.position;
		this.owner = owner;
	}

	private void Start() {
		trail = GetComponent<TrailRenderer>();
		trail.enabled = true;
	}

	private void FixedUpdate() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, travelVector, out hit, (travelVector.normalized * ProjectileSpeed * Time.fixedDeltaTime).magnitude, bulletHitLayer)) {
			Destroy(gameObject);
			if (hit.collider.gameObject.GetComponent<PlayerController>()){
				EventSystem.Current.FireEvent(new HitEvent {
					Description = " " + owner.owner.gameObject.name + " hit " + owner.owner.Target.name,
					Source = owner.gameObject,
					Target = owner.owner.Target.gameObject
				});
			}
			
		}
		transform.position += travelVector.normalized * ProjectileSpeed * Time.fixedDeltaTime;
	}
}