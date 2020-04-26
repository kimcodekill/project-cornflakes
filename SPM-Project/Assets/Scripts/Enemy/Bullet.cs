using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	[Header("Bullet attributes")]
	[SerializeField] [Tooltip("How fast the bullet travels.")] private float projectileSpeed;
	private float distanceToTravel;
	private float totalDistanceTravelled;
	private Vector3 movementDirection;

	/// <summary>
	/// Gives the Bullet some number of starting values through parameters.
	/// </summary>
	/// <param name="shootDir"> The vector the bullet should travel along.</param>
	/// <param name="distanceToTarget">How far the bullet should travel before destroying.</param>
	public void Initialize(Vector3 shootDir, float distanceToTarget) {
		movementDirection = shootDir;
		distanceToTravel = distanceToTarget;
	}

	void Update() {
		CheckDistance(totalDistanceTravelled, distanceToTravel);
		totalDistanceTravelled += projectileSpeed * Time.deltaTime;
		transform.position += movementDirection.normalized * projectileSpeed * Time.deltaTime;
	}

	private void CheckDistance(float distance, float maxDistance) {
		if (distance >= maxDistance)
			Destroy(gameObject);
			//gameObject.SetActive(false);
	}
}