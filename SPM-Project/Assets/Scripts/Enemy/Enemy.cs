using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;

//Author: Erik Pilström
public class Enemy : MonoBehaviour, IEntity, ICapturable 
{
	[Header("Enemy attributes")]
	[SerializeField] [Tooltip("How fast the enemy should move.")] protected float movementSpeed;
	[SerializeField] [Tooltip("This enemy's current health")] private float currentHealth;
	[SerializeField] [Tooltip("This enemy's max health.")] private float maxHealth;
	[SerializeField] [Tooltip("This enemy's field of view given as dot product.")] private float fieldOfView;
	[SerializeField] [Tooltip("How far the enemy should see.")] protected float visionRange;
	[SerializeField] [Tooltip("The relative position of the enemy's gun.")] public Transform gunTransform;
	[SerializeField] [Tooltip("The relative position of the enemy's eyes.")] public Transform eyeTransform;
	[SerializeField] [Tooltip("This enemy's maximum attack range.")] protected float attackRange;
	[SerializeField] [Tooltip("How much damage the enemy should deal per shot.")] protected float attackDamage;
	[SerializeField] [Tooltip("The spread the enemy's attacks should have, angle in degress.")] protected float attackSpread;
	[SerializeField] [Tooltip("How fast the enemy should attack, RPM.")] protected float attackSpeedRPM;
	[SerializeField] [Tooltip("Within how many degrees (around the Y axis) does the enemy have to point its weapon at the player before attacking.")] private float attackLimitDegrees;
	[SerializeField] [Tooltip("Layers that this enemy can't see through.")] protected LayerMask ObscuringLayers;

	[SerializeField] [Tooltip("This enemy's possible states.")] private State[] states;
	[SerializeField] protected EnemyWeaponBase weapon;
	[SerializeField] private GameObject deathExplosion;

	
	protected Vector3 vectorToPlayer; //Vector to the player from the enemy's eyes. Used by Enemy-children.
	private StateMachine enemyStateMachine; //Enemy's STM instance.

	/// <summary>
	/// Returns the enemy's weapon instance.
	/// </summary>
	public EnemyWeaponBase EnemyEquippedWeapon { get => weapon; }

	[HideInInspector] public AudioSource audioSource;
	[HideInInspector] public AudioSource audioSourceIdle;
	public AudioClip[] audioClips;
	[SerializeField] private AudioMixerGroup mixerGroup;
	private int minSoundDelay = 5;
	private int maxSoundDelay = 10;

	/// <summary>
	/// Returns this enemy's target.
	/// </summary>
	public PlayerController Target { get; private set; }

	/// <summary>
	/// Is the enemy done searching for the player?
	/// </summary>
	public bool FinishedSearching { get; protected set; }

	/// <summary>
	/// Is this enemy a patroller?
	/// </summary>
	public bool IsPatroller { get; protected set; }

	/// <summary>
	/// Returns the origin of this enemy.
	/// </summary>
	public Vector3 Origin { get; private set; }

	private void Awake() {

	}

	protected void Start() {
		Origin = gameObject.transform.parent.transform.position;
		EnemyEquippedWeapon.SetParams(this, attackSpeedRPM, attackDamage, attackSpread, attackRange);
		Target = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

		audioSource = CreateAudioSource(1);
		audioSourceIdle = CreateAudioSource(0.75f);

		enemyStateMachine = new StateMachine(this, states);
		currentHealth = maxHealth;
	}

	protected void Update() {
		vectorToPlayer = GetVectorToTarget(Target.transform, eyeTransform);
		enemyStateMachine.Run();
		if (audioSourceIdle.isPlaying == false) {
			audioSourceIdle.clip = audioClips[Random.Range(0, 4)];
			audioSourceIdle.PlayDelayed(Random.Range(minSoundDelay, maxSoundDelay));
		}
	}

	protected AudioSource CreateAudioSource(float volume) {
		AudioSource aSource = gameObject.AddComponent<AudioSource>();
		aSource.spatialBlend = 1;
		aSource.rolloffMode = AudioRolloffMode.Linear;
		aSource.minDistance = 10;
		aSource.maxDistance = 30;
		aSource.outputAudioMixerGroup = mixerGroup;
		aSource.volume = volume;
		return aSource;
	}

	/// <summary>
	/// Calculates and returns the vector between an origin and a target.
	/// </summary>
	/// <param name="target">Position of the target.</param>
	/// <param name="origin">Origin position.</param>
	/// <returns></returns>
	public Vector3 GetVectorToTarget(Transform target, Transform origin) {
		Vector3 v = target.position - origin.position;
		return v;
	}

	/// <summary>
	/// Checks to see if the enemy can see the player in its field of view and sight range, and that the player is not obscured by objects.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public bool TargetIsInSight() { //Checks if the Enemy's Target is in sight by checking the different sight factors.
		if (TargetIsInRange() && TargetIsInFOV(vectorToPlayer) && CanSeeTarget(vectorToPlayer)) { return true; }
		else { return false; }
	}

	private bool TargetIsInFOV(Vector3 v) { //Checks if the Enemy's Target is inside the specified field of view
		float angleToTarget = Vector3.Dot(eyeTransform.forward, v.normalized);
		if (angleToTarget >= fieldOfView) {
			return true; 
		}
		else { return false; }
	}

	private bool TargetIsInRange() { //Is Target within sight range?
		if (Vector3.Distance(transform.position, Target.transform.position) < visionRange) {
			return true;
		}
		else { return false; }
	}

	private bool CanSeeTarget(Vector3 v) { //Is the line to the target obscured by something?
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
		if (TargetIsInSight() && vectorToPlayer.magnitude <= attackRange) {
			if (!Physics.SphereCast(gunTransform.position, 0.1f, vectorToPlayer, out _, vectorToPlayer.magnitude, ObscuringLayers)) { return true; }
			else { return false; }
		}
		else return false;
		
	}

	/// <summary>
	/// Checks if the Enemy's weapon is aimed sufficiently towards the player.
	/// </summary>
	/// <returns></returns>
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

	public Vector3 CalculateTargetVelocity(Vector3 v1, Vector3 v2) {
		Vector3 v = v2 - v1;
		return v;
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
		EventSystem.Current.FireEvent(new EnemyDeathEvent() {
			Source = gameObject,
			DropAnythingAtAllChance = 0.5f,
		});
		EventSystem.Current.FireEvent(new ExplosionEffectEvent()
		{
			ExplosionEffect = deathExplosion,
			WorldPosition = transform.position,
			Rotation = Quaternion.identity,
			Scale = 1
		});
		gameObject.SetActive(false);
		Destroy(gameObject.transform.parent.gameObject, 2f);
	}
	
	public void PlayAudio(int clipIndex, float volume, float minPitch, float maxPitch) {
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
	}
	
	/// <summary>
	/// Signatures for all the behaviour coroutines that the various enemies need to be able to implement.
	/// (Getting kinda ridiculous with these coroutines, considering reworking enemies from the base up to use behaviour trees instead.)
	/// </summary>
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

	/// <summary>
	/// Implements ICaptureable interface so that the enemy can be saved by the checkpoint system.
	/// </summary>
	/// <returns></returns>
	public bool InstanceIsCapturable() {
		return true;
	}

	/// <summary>
	/// Implements ICaptureable interface so that the enemy can be saved by the checkpoint system.
	/// </summary>
	/// <returns></returns>
	public object GetPersistentCaptureID() {
		return Origin;
	}
}