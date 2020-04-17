using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IPawn 
{
	
	[Header("Enemy attributes")]
	[SerializeField] [Tooltip("This enemy's current health")] private float currentHealth;
	[SerializeField] [Tooltip("This enemy's max health.")] private float maxHealth;
	[SerializeField] [Tooltip("This enemy's field of view given as dot product.")] private float fieldOfView;
	[SerializeField] [Tooltip("This enemy's maximum sight range.")] private float sightRange;
	[SerializeField] [Tooltip("The relative position of the enemy's gun.")] private Transform gunPosition;
	[SerializeField] public Transform eyePosition;
	
	[SerializeField] [Tooltip("This enemy's possible states.")] private State[] states;
	[SerializeField] [Tooltip("Layers that this enemy can't see through.")] protected LayerMask layerMask;

	private StateMachine enemyStateMachine;

	/// <summary>
	/// Returns this enemy's target.
	/// </summary>
	public PlayerController Target { get; private set; }
	
	public Vector3 VectorToTarget { get; private set; }

	protected void Start() {
		Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		enemyStateMachine = new StateMachine(this, states);
		currentHealth = maxHealth;
	}

	protected void Update() {
		VectorToTarget = CalculateVectorToTarget();
		enemyStateMachine.Run();
	}

	private Vector3 CalculateVectorToTarget() {
		Vector3 v = Target.transform.position - transform.position;
		return v;
	}

	/// <summary>
	/// Checks to see if the enemy can see the player in its field of view and sight range, and that the player is not obscured by objects.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public bool PlayerIsInSight() {
		if (TargetIsInRange() && CanSeeTarget(VectorToTarget) && TargetIsInFOV(VectorToTarget)) { return true; }
		else { return false; }
	}

	private bool TargetIsInFOV(Vector3 v) {
		float angleToTarget = Vector3.Dot(transform.forward, v.normalized);
		if (angleToTarget >= fieldOfView) {
			//Debug.Log(Target.gameObject.name + " is in FOV of " + gameObject.name);
			return true; 
		}
		else { return false; }
	}

	private bool TargetIsInRange() {
		if (Vector3.Distance(transform.position, Target.transform.position) < sightRange) {
			//Debug.Log(Target.gameObject.name + " in range of " + gameObject.name);
			return true;
		}
		else { return false; }
	}

	private bool CanSeeTarget(Vector3 v) {
		Vector3 enemyEye = transform.position + eyePosition.position;
		Physics.Raycast(enemyEye, v, out RaycastHit hit, v.magnitude, layerMask);
		///Casts Raycasts in a cone to look for the player even if the player is only peeking around a corner.
		/*Physics.Raycast(enemyEyes, v + new Vector3(0, -0.25f, 0), out RaycastHit hit2, v.magnitude, layerMask);
		Physics.Raycast(enemyEyes, v + new Vector3(0, 0.25f, 0), out RaycastHit hit3, v.magnitude, layerMask);
		Physics.Raycast(enemyEyes, v + new Vector3(0.25f, 0, 0), out RaycastHit hit4, v.magnitude, layerMask);
		Physics.Raycast(enemyEyes, v + new Vector3(-0.25f, 0, 0), out RaycastHit hit5, v.magnitude, layerMask);*/
		if (hit.collider == null/* || hit2.collider == null || hit3.collider == null || hit4.collider == null || hit5.collider == null*/) {
			//Debug.Log(Target.gameObject.name + " is seen by " + gameObject.name);
			return true;
		}
		else { return false; }
	}

	/// <summary>
	/// Checks if the enemy can attack its target by SphereCasting from the given gun position, to the target, checking for collisions on the way.
	/// </summary>
	/// <returns></returns>
	public bool TargetIsAttackable() {
		if (!Physics.SphereCast(GetGunPosition().position, 0.5f, VectorToTarget, out _, VectorToTarget.magnitude, layerMask)) { return true; }
		else { return false; }
		///if the enemy can't attack the player, move to position where it can attack player
	}

	/// <summary>
	/// Returns this enemy's gunPosition;
	/// </summary>
	/// <returns></returns>
	public Transform GetGunPosition() { return gunPosition; }

	/// <summary>
	/// Implements <c>TakeDamage()</c> from IPawn interface to deal damage to the enemy.
	/// </summary>
	/// <param name="amount">The amount of damage the enemy should take.</param>
	/// <returns></returns>
	public float TakeDamage(float amount) {
		currentHealth -= amount;
		if (currentHealth <= 0) { KillEnemy(); }
		return currentHealth;
	}

	/// <summary>
	/// Implements <c>Heal()</c> from IPawn interface to heal the enemy.
	/// </summary>
	/// <param name="amount">The amount of healing the enemy should receive.</param>
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

	public virtual void StartIdleBehaviour() {

	}

	public virtual void StopIdleBehaviour() {

	}

	public virtual void StartAttackBehaviour() {

	}

	public virtual void StopAttackBehaviour() {

	}
}