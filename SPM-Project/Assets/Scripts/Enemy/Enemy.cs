using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Enemy : MonoBehaviour, IEntity 
{
	
	[Header("Enemy attributes")]
	[SerializeField] [Tooltip("This enemy's current health")] private float currentHealth;
	[SerializeField] [Tooltip("This enemy's max health.")] private float maxHealth;
	[SerializeField] [Tooltip("This enemy's field of view given as dot product.")] private float fieldOfView;
	[SerializeField] [Tooltip("This enemy's maximum attack range.")] protected float attackRange;
	[SerializeField] protected float visionRange;
	[SerializeField] [Tooltip("The relative position of the enemy's gun.")] public Transform gunTransform;
	[SerializeField] [Tooltip("The relative position of the enemy's eyes.")] public Transform eyeTransform;
	
	[SerializeField] [Tooltip("This enemy's possible states.")] private State[] states;
	[SerializeField] [Tooltip("Layers that this enemy can't see through.")] protected LayerMask layerMask;
	[SerializeField] protected float movementSpeed;

	private StateMachine enemyStateMachine;

	public float weaponSpread;

	[HideInInspector] public AudioSource audioSource;
	[HideInInspector] public AudioSource audioSourceIdle;
	public AudioClip[] audioClips;
	private int minSoundDelay = 5;
	private int maxSoundDelay = 10;

	/// <summary>
	/// Returns this enemy's target.
	/// </summary>
	public PlayerController Target { get; private set; }
	
	public Vector3 VectorToTarget { get; private set; }
	public bool FinishedSearching { get; protected set; }
	public bool IsPatroller { get; protected set; }

	protected void Start() {
		Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		audioSource = CreateAudioSource();
		audioSourceIdle = CreateAudioSource();

		enemyStateMachine = new StateMachine(this, states);
		currentHealth = maxHealth;
	}

	protected AudioSource CreateAudioSource() {
		AudioSource aSource = gameObject.AddComponent<AudioSource>();
		aSource.spatialBlend = 1;
		aSource.rolloffMode = AudioRolloffMode.Linear;
		aSource.minDistance = 1;
		aSource.maxDistance = 40;
		return aSource;
	}

	protected void Update() {
		VectorToTarget = GetVectorToTarget();
		enemyStateMachine.Run();
		if (audioSourceIdle.isPlaying == false) {
			audioSourceIdle.clip = audioClips[Random.Range(0, 4)];
			audioSourceIdle.PlayDelayed(Random.Range(minSoundDelay, maxSoundDelay));
		}
		//Debug.Log(Vector3.Dot(transform.forward, VectorToTarget.normalized));
		//Debug.Log(finishedSearching);
	}

	private Vector3 GetVectorToTarget() {
		Vector3 v = Target.transform.position - transform.position;
		return v;
	}

	/// <summary>
	/// Checks to see if the enemy can see the player in its field of view and sight range, and that the player is not obscured by objects.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public bool PlayerIsInSight() {
		if (TargetIsInRange() && TargetIsInFOV(VectorToTarget) && CanSeeTarget(VectorToTarget)) { return true; }
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
		Physics.Raycast(eyeTransform.position, v, out RaycastHit hit, v.magnitude, layerMask);
		if (!hit.collider) {
			Debug.DrawRay(eyeTransform.position, v, Color.red);
			return true;
		}
		else { return false; }
	}

	/// <summary>
	/// Checks if the enemy can attack its target by SphereCasting from the given gun position, to the target, checking for collisions on the way.
	/// </summary>
	/// <returns></returns>
	public bool TargetIsAttackable() {
		if (PlayerIsInSight() && VectorToTarget.magnitude <= attackRange) {
			if (!Physics.SphereCast(gunTransform.position, 0.1f, VectorToTarget, out _, VectorToTarget.magnitude, layerMask)) { return true; }
			else { return false; }
		}
		else return false;
		
	}

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

	public Vector3 AttackSpreadCone(float radius) {
		//(sqrt(1 - z^2) * cosϕ, sqrt(1 - z^2) * sinϕ, z)
		float radradius = radius * Mathf.PI / 360;
		float z = Random.Range(Mathf.Cos(radradius), 1);
		float t = Random.Range(0, Mathf.PI * 2);
		return new Vector3(Mathf.Sqrt(1 - z * z) * Mathf.Cos(t), Mathf.Sqrt(1 - z * z) * Mathf.Sin(t), z);
	}

	public void DoAttack(float firingRate, float damage) { }
	public void PlayAudio(int clipIndex, float volume, float minPitch, float maxPitch) {
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
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

	
}