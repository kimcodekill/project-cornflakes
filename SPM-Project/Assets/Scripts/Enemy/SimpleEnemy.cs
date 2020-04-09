using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour, IPawn 
{

	[SerializeField] Transform target; //should probably be made so that the enemy loads player as target through something like FindObjectWithTag
	[SerializeField] float maxDistanceToTarget = 20f;
	[SerializeField] float fieldOfView = 0.7f;
	private StateMachine stm;
	[SerializeField] private State[] states;
	private CapsuleCollider collider;
	[SerializeField] private LayerMask layerMask;
	[SerializeField] Bullet bulletPrefab;
	[SerializeField] private float attackSpeed;
	private float attackCooldown;
	[SerializeField] private float maxHealth;
	private float health;
	[SerializeField] float enemyAttackDamage;

	void Start() {
		collider = GetComponent<CapsuleCollider>();
		stm = new StateMachine(this, states);
		health = maxHealth;
	}

	void Update() {
		attackCooldown += Time.deltaTime;
		Vector3 vectorToTarget = CalculateVectorToTarget();
		if (TargetIsInFOV(vectorToTarget, fieldOfView) && TargetIsInRange(vectorToTarget, maxDistanceToTarget) && CanSeeTarget(vectorToTarget)) {
			if (attackCooldown > attackSpeed) {
				AttackTarget(vectorToTarget);
				attackCooldown = 0;
			}

		}
	}

	/// <summary>
	/// Creates relationship vector to the target Transform
	/// </summary>
	/// <returns></returns>
	private Vector3 CalculateVectorToTarget() {
		Vector3 targetVector = target.position - transform.position;
		//Debug.Log(targetVector);
		return targetVector;
	}

	/// <summary>
	/// Checks if the target is in the field of view
	/// </summary>
	/// <param name="vectorToTarget"></param>
	/// <param name="fov"></param>
	/// <returns></returns>
	public bool TargetIsInFOV(Vector3 vectorToTarget, float fov) {
		float angleToTarget = Vector3.Dot(transform.forward, vectorToTarget.normalized);
		//Debug.Log(gameObject + "" + angleToTarget);
		if (angleToTarget >= fov)
			return true;
		else return false;
	}

	/// <summary>
	/// Checks that the target is within the specified max range
	/// </summary>
	/// <param name="vectorToTarget"></param>
	/// <param name="maxDistance"></param>
	/// <returns></returns>
	public bool TargetIsInRange(Vector3 vectorToTarget, float maxDistance) {
		float distanceToTarget = vectorToTarget.magnitude;
		//Debug.Log(gameObject + "" + distanceToTarget);
		if (distanceToTarget <= maxDistance)
			return true;
		else return false;
	}

	/// <summary>
	/// Checks if there is an object between the enemy and its target which does not have the Player tag
	/// </summary>
	/// <returns></returns>
	public bool CanSeeTarget(Vector3 vectorToTarget) {
		Vector3 enemyEyes = transform.position + collider.center + Vector3.up * (collider.height / 2 - collider.radius);
		Physics.Raycast(enemyEyes, vectorToTarget, out RaycastHit hit, vectorToTarget.magnitude, layerMask);
		Physics.Raycast(enemyEyes, vectorToTarget + new Vector3(0,-0.25f,0), out RaycastHit hit2, vectorToTarget.magnitude, layerMask);
		Physics.Raycast(enemyEyes, vectorToTarget + new Vector3(0,0.25f,0), out RaycastHit hit3, vectorToTarget.magnitude, layerMask);
		Physics.Raycast(enemyEyes, vectorToTarget + new Vector3(0.25f, 0, 0), out RaycastHit hit4, vectorToTarget.magnitude, layerMask);
		Physics.Raycast(enemyEyes, vectorToTarget + new Vector3(-0.25f, 0, 0), out RaycastHit hit5, vectorToTarget.magnitude, layerMask);
		//Debug.DrawRay(enemyEyes, vectorToTarget, Color.blue);
		//Debug.DrawRay(enemyEyes, vectorToTarget + new Vector3(0,0.25f,0), Color.blue);
		//Debug.DrawRay(enemyEyes, vectorToTarget + new Vector3(0,-0.25f,0), Color.blue);
		//Debug.DrawRay(enemyEyes, vectorToTarget + new Vector3(0.25f, 0, 0), Color.blue);
		//Debug.DrawRay(enemyEyes, vectorToTarget + new Vector3(-0.25f,0, 0), Color.blue);
		//Debug.DrawRay(transform.position + collider.center + Vector3.up * (collider.height / 3 - collider.radius), vectorToTarget);
		if (hit.collider == null || hit2.collider == null || hit3.collider == null || hit4.collider == null || hit5.collider == null) {
			return true;
		}
		else return false;
	}

	/// <summary>
	/// Creates bullets shooting towards the target along the given vector
	/// </summary>
	/// <param name="vectorToTarget"> The vector leading to the target, passed to the fired bullet from the enemy.</param>
	private void AttackTarget(Vector3 vectorToTarget) {
		Bullet instance;
		Vector3 gunPosition = transform.position + collider.center + Vector3.up * (collider.height / 3 - collider.radius);
		if (!Physics.SphereCast(gunPosition, collider.radius / 4, vectorToTarget, out _, vectorToTarget.magnitude, layerMask)) {
			instance = Instantiate(bulletPrefab, gunPosition + Vector3.up * 0.1f, Quaternion.identity);
			instance.Initialize(vectorToTarget, vectorToTarget.magnitude);
			target.gameObject.GetComponent<PlayerController>().TakeDamage(enemyAttackDamage);

		}

	}

	public float TakeDamage(float amount) 
	{
		//DebugManager.AddSection("Enemy Health", health.ToString());
		Debug.Log(health.ToString());
		health -= amount;
		if (health <= 0) { Die(); }
		return health;
	}

	private void Die() {

		Debug.LogWarning("Object '" + gameObject.name + "' gets removed using Destroy. This is illegal.");
		Destroy(gameObject);

		//gameObject.SetActive(false);

		//death sound & animations
	}

	public float Heal(float amount)
	{
		health += amount;
		if (health > maxHealth) { health = maxHealth; }
		return health;
	}
}