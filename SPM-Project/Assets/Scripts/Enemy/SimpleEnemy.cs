using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : Pawn, IPawn 
{
	[Header("Enemy attributes")]
	[SerializeField] [Tooltip("How often/fast the enemy attacks.")] private float attackCooldown;
	[SerializeField] [Tooltip("How much damage the enemy deals per shot.")] private float attackDamage;
	[SerializeField] [Tooltip("Enemy's maximum attack range.")] private float attackRange = 20f;
	[SerializeField] [Tooltip("Enemy's current health")] private float currentHealth;
	[SerializeField] [Tooltip("Enemy's max health.")] private float maxHealth;
	[SerializeField] [Tooltip("Bullet object to instantiate when enemy attacks.")] private Bullet bulletPrefab;
	private float interalAttackCD;

	[Header("Target acquisition")]
	[SerializeField] [Tooltip("Enemy's target.")] private Transform target; //should probably be made so that the enemy loads player as target through something like FindObjectWithTag
	[SerializeField] [Tooltip("Enemy's field of view (dot product).")] private float fieldOfView = 0.7f;
	[SerializeField] [Tooltip("Layers that enemy can't see through.")] private LayerMask layerMask;
	
	private CapsuleCollider collider;

	private void Start() {
		collider = GetComponent<CapsuleCollider>();
		currentHealth = maxHealth;
	}

	private void Update() {
		interalAttackCD += Time.deltaTime;
		Vector3 vectorToTarget = CalculateVectorToTarget();
		if (TargetIsAttackable(vectorToTarget)) {
			if (interalAttackCD > attackCooldown) {
				AttackTarget(vectorToTarget);
				interalAttackCD = 0;
			}
		}
	}
	
	private Vector3 CalculateVectorToTarget() {
		Vector3 v = target.position - transform.position;
		return v;
	}

	private bool TargetIsAttackable(Vector3 v) {
		if (TargetIsInFOV(v) && TargetIsInRange(v) && CanSeeTarget(v)) { return true; }
		else { return false; }
	}

	private bool TargetIsInFOV(Vector3 v) {
		float angleToTarget = Vector3.Dot(transform.forward, v.normalized);
		if (angleToTarget >= fieldOfView) { return true; }
		else { return false; }
	}

	private bool TargetIsInRange(Vector3 v) {
		float distanceToTarget = v.magnitude;
		if (distanceToTarget <= attackRange) { return true; }
		else { return false; }
	}

	private bool CanSeeTarget(Vector3 v) {
		Vector3 enemyEyes = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
		Physics.Raycast(enemyEyes, v, out RaycastHit hit, v.magnitude, layerMask);
		///Casts Raycasts in a cone to look for the player even if the player is only peeking around a corner.
		Physics.Raycast(enemyEyes, v + new Vector3(0,-0.25f,0), out RaycastHit hit2, v.magnitude, layerMask);
		Physics.Raycast(enemyEyes, v + new Vector3(0,0.25f,0), out RaycastHit hit3, v.magnitude, layerMask);
		Physics.Raycast(enemyEyes, v + new Vector3(0.25f, 0, 0), out RaycastHit hit4, v.magnitude, layerMask);
		Physics.Raycast(enemyEyes, v + new Vector3(-0.25f, 0, 0), out RaycastHit hit5, v.magnitude, layerMask);
		if (hit.collider == null || hit2.collider == null || hit3.collider == null || hit4.collider == null || hit5.collider == null) {
			return true;
		}
		else { return false; }
	}

	private void AttackTarget(Vector3 v) {
		Bullet instance;
		Vector3 gunPosition = transform.position + collider.center + Vector3.up * (collider.height / 3 - collider.radius);
		if (!Physics.SphereCast(gunPosition, collider.radius / 4, v, out _, v.magnitude, layerMask)) {
			instance = Instantiate(bulletPrefab, gunPosition + Vector3.up * 0.1f, transform.rotation);
			instance.Initialize(v, v.magnitude);
			target.gameObject.GetComponent<PlayerController>().TakeDamage(attackDamage);
		}
	}

	/// <summary>
	/// Implements <c>TakeDamage()</c> from IPawn interface to deal damage to the gameObject.
	/// </summary>
	/// <param name="amount">The amount of damage the gameObject should take.</param>
	/// <returns></returns>
	public float TakeDamage(float amount) {
		currentHealth -= amount;
		if (currentHealth <= 0) { KillEnemy(); }
		return currentHealth;
	}

	/// <summary>
	/// Implements <c>Heal()</c> from IPawn interface to heal the gameObject.
	/// </summary>
	/// <param name="amount">The amount of healing the gameObject should receive.</param>
	/// <returns></returns>
	public float Heal(float amount) {
		currentHealth += amount;
		if (currentHealth > maxHealth) { currentHealth = maxHealth; }
		return currentHealth;
	}

	private void KillEnemy() {
		//Destroy(gameObject);
		gameObject.SetActive(false);
	}
}