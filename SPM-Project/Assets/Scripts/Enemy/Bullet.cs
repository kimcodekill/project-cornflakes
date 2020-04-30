using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[SerializeField] private float projectileSpeed;
	private Vector3 travelVector;
	private TrailRenderer trail;
	[SerializeField] private LayerMask bulletHitLayer;
	[SerializeField] private GameObject hitEffect;

	/// <summary>
	/// Gives the Bullet some number of starting values through parameters.
	/// </summary>
	/// <param name="shootDir"> The vector the bullet should travel along.</param>
	public void Initialize(Vector3 shootDir) {
		travelVector = shootDir - transform.position;
	}

	private void Start() {
		trail = GetComponent<TrailRenderer>();
	}

	private void FixedUpdate() {
		RaycastHit hit;
		if(Physics.Raycast(transform.position, travelVector, out hit, (travelVector.normalized * projectileSpeed * Time.fixedDeltaTime).magnitude, bulletHitLayer)) {
			Destroy(gameObject);
			
		}
		transform.position += travelVector.normalized * projectileSpeed * Time.fixedDeltaTime;
	}
}