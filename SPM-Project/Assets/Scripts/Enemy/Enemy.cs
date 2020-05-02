using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntity, ICapturable 
{
	[Header("Enemy attributes")]
	[SerializeField] protected float movementSpeed;
	[SerializeField] [Tooltip("This enemy's current health")] private float currentHealth;
	[SerializeField] [Tooltip("This enemy's max health.")] private float maxHealth;
	[SerializeField] [Tooltip("This enemy's field of view given as dot product.")] private float fieldOfView;
	[SerializeField] protected float visionRange;
	[SerializeField] [Tooltip("The relative position of the enemy's gun.")] public Transform gunTransform;
	[SerializeField] [Tooltip("The relative position of the enemy's eyes.")] public Transform eyeTransform;
	[SerializeField] [Tooltip("This enemy's maximum attack range.")] protected float attackRange;
	[SerializeField] protected float attackDamage;
	[SerializeField] protected float attackSpread;
	[SerializeField] protected float attackSpeedRPM;

	[SerializeField] [Tooltip("This enemy's possible states.")] private State[] states;
	[SerializeField] [Tooltip("Layers that this enemy can't see through.")] protected LayerMask ObscuringLayers;
	[SerializeField] protected EnemyWeaponBase weapon;
	[SerializeField] private float attackLimitDegrees;
	public EnemyWeaponBase EnemyEquippedWeapon { get => weapon; }

	private StateMachine enemyStateMachine;
	protected Vector3 vectorToPlayer;

	/// <summary>
	/// Returns this enemy's target.
	/// </summary>
	public PlayerController Target { get; private set; }
	public bool FinishedSearching { get; protected set; }
	public bool IsPatroller { get; protected set; }

	public Vector3 Origin { get; private set; }

	private void Awake() {
		Origin = transform.position;
	}

	protected void Start() {
		EnemyEquippedWeapon.SetParams(this, attackSpeedRPM, attackDamage, attackSpread, attackRange);
		Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
		enemyStateMachine = new StateMachine(this, states);
		currentHealth = maxHealth;
	}

	protected void Update() {
		vectorToPlayer = GetVectorToTarget(Target.transform, eyeTransform);
		enemyStateMachine.Run();
		//Debug.Log(Vector3.Dot(transform.forward, VectorToTarget.normalized));
		//Debug.Log(finishedSearching);
	}

	public Vector3 GetVectorToTarget(Transform target, Transform origin) {
		Vector3 v = target.position - origin.position;
		return v;
	}

	/// <summary>
	/// Checks to see if the enemy can see the player in its field of view and sight range, and that the player is not obscured by objects.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public bool PlayerIsInSight() {
		if (TargetIsInRange() && TargetIsInFOV(vectorToPlayer) && CanSeeTarget(vectorToPlayer)) { return true; }
		else { return false; }
	}

	private bool TargetIsInFOV(Vector3 v) {
		float angleToTarget = Vector3.Dot(eyeTransform.forward, v.normalized);
		if (angleToTarget >= fieldOfView) {
			//Debug.Log(Target.gameObject.name + " is in FOV of " + gameObject.name);
			return true; 
		}
		else { return false; }
	}

	private bool TargetIsInRange() {
		if (Vector3.Distance(transform.position, Target.transform.position) < visionRange) {
			//Debug.Log(Target.gameObject.name + " in range of " + gameObject.name);
			return true;
		}
		else { return false; }
	}

	private bool CanSeeTarget(Vector3 v) {
		Physics.Raycast(eyeTransform.position, v, out RaycastHit hit, v.magnitude, ObscuringLayers);
		if (!hit.collider) {
			//Debug.DrawRay(eyeTransform.position, v, Color.red);
			return true;
		}
		else { return false; }
	}

	/// <summary>
	/// Checks if the enemy can attack its target by SphereCasting from the given gun position, to the target, checking for collisions on the way.
	/// </summary>
	/// <returns></returns>
	public bool TargetIsAttackable() {
		if (PlayerIsInSight() && vectorToPlayer.magnitude <= attackRange) {
			if (!Physics.SphereCast(gunTransform.position, 0.1f, vectorToPlayer, out _, vectorToPlayer.magnitude, ObscuringLayers)) { return true; }
			else { return false; }
		}
		else return false;
		
	}

	public bool WeaponIsAimed() {
		Vector3 sightToPlayer = Vector3.ProjectOnPlane(vectorToPlayer.normalized, Vector3.up);
		Vector3 gunAim = Vector3.ProjectOnPlane(gunTransform.forward, Vector3.up);
		float radAngle = Mathf.Acos((Vector3.Dot(sightToPlayer, gunAim)) / (sightToPlayer.magnitude * gunAim.magnitude));
		float degrees = radAngle * (180 / Mathf.PI);		
		if (degrees < attackLimitDegrees) {
			return true;
		}
		else return false;
	}

	protected void ResetWeapon() {
		gunTransform.forward = Vector3.RotateTowards(gunTransform.forward, transform.forward, Time.deltaTime, 0);
	}

	/// <summary>
	/// Implements <c>TakeDamage()</c> from IPawn interface to deal damage to the enemy.
	/// </summary>
	/// <param name="amount">The amount of damage the enemy should take.</param>
	/// <returns></returns>
	public float TakeDamage(float amount) {
		currentHealth -= amount;
		if (currentHealth <= 0) { Die(); }
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

	private void Die() {
		StopAllCoroutines();
		Destroy(gameObject, 0.5f);
		//gameObject.SetActive(false);
	}

	public virtual void StartIdleBehaviour() { }
	public virtual void StopIdleBehaviour() { }
	public virtual void StartPatrolBehaviour() { }
	public virtual void StopPatrolBehaviour() { }
	public virtual void StartAlertedBehaviour() { }
	public virtual void StopAlertedBehaviour() { }
	public virtual void StartAttackBehaviour() { }
	public virtual void StopAttackBehaviour() { }
	public virtual void StartSearchBehaviour() { }
	public virtual void StopSearchBehaviour() { }

	public bool InstanceIsCapturable() {
		return true;
	}

	public object GetPersistentCaptureID() {
		return Origin;
	}
}