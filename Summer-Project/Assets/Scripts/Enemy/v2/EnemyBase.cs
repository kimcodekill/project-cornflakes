﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SocialPlatforms;

//Author: Erik Pilström
public class EnemyBase : MonoBehaviour, IEntity, ICapturable
{
	[Header("Health vars")]
	[SerializeField] [Tooltip("This enemy's current health.")] private float currentHealth;
	[SerializeField] [Tooltip("This enemy's max health.")] private float maxHealth;

	[Header("Sight vars")]
	[SerializeField] [Tooltip("This enemy's sight cone's angle in degrees.")] private float fieldOfView;
	[SerializeField] [Tooltip("How far this enemy should see.")] protected float visionRange;
	protected float defaultVisionRange;
	[SerializeField] [Tooltip("This enemy's maximum attack range.")] protected float attackRange;
	[SerializeField] [Tooltip("How close to the player (angle in degrees) this enemy has to point its weapon before attacking.")] private float attackLimitDegrees;
	[SerializeField] [Tooltip("The relative position of this enemy's gun.")] public Transform gunTransformPosition;
	[SerializeField] [Tooltip("The relative position of this enemy's eyes.")] public Transform eyeTransformPosition;
	[SerializeField] [Tooltip("Layers that this enemy cannot see through.")] protected LayerMask ObscuringLayers;

	[Header("Attacking vars")]
	[SerializeField] [Tooltip("The damage this enemy should deal per shot.")] protected float attackDamage;
	[SerializeField] [Tooltip("The spread this enemy's attacks should have, angle in degrees.")] protected float attackSpread;
	[SerializeField] [Tooltip("The speed that this enemy should attack, RPM.")] protected float attackSpeedRPM;


	[HideInInspector] public AudioSource audioSource;
	[HideInInspector] public AudioSource audioSourceIdle;
	[Header("Sound vars")]
	public AudioClip[] audioClips;
	[SerializeField] private AudioMixerGroup mixerGroup;
	private int minSoundDelay = 5;
	private int maxSoundDelay = 10;
	private float soundDelay;
	private float soundStartTime = 0f;

	[Header("Others")]
	[SerializeField] [Tooltip("This enemy's possible states.")] private State[] states;
	[SerializeField] protected EnemyWeaponBase weapon;
	[SerializeField] private GameObject deathExplosion;
	protected bool isInCombat;
	public Animator enemyAnimator;

	///Each enemy's STM instance.
	private StateMachine enemyStateMachine;
	///Vector to the player from the enemy's eyes. Used by Enemy-children.
	protected Vector3 vectorToPlayer;

	/// <summary>
	/// Returns the enemy's weapon instance.
	/// </summary>
	public EnemyWeaponBase EnemyEquippedWeapon { get => weapon; }

	/// <summary>
	/// Returns this enemy's target.
	/// </summary>
	public PlayerController Target { get; private set; }

	/// <summary>
	/// Is the enemy done searching for the player?
	/// </summary>
	public bool HasFinishedSearching { get; protected set; }

	/// <summary>
	/// Is this enemy a patroller?
	/// </summary>
	public bool IsPatroller { get; protected set; }  //Deprecated as of 2020-06-03 but leaving it for potential future use.

	/// <summary>
	/// Returns the origin of this enemy.
	/// </summary>
	public Vector3 Origin { get; private set; }


	protected void Awake()
	{
		Origin = transform.position;
	}


	protected void Start()
	{
		soundDelay = Random.Range(minSoundDelay, maxSoundDelay);
		EnemyEquippedWeapon.SetParams(this, attackSpeedRPM, attackDamage, attackSpread, attackRange);
		Target = PlayerController.Instance;

		audioSource = CreateAudioSource(1);
		audioSourceIdle = CreateAudioSource(0.75f);

		enemyStateMachine = new StateMachine(this, states);
		currentHealth = maxHealth;

		isInCombat = false;
		defaultVisionRange = visionRange;
	}

	protected void Update()
	{
		vectorToPlayer = GetVectorFromAtoB(eyeTransformPosition, Target.transform);
		//Debug.DrawRay(eyeTransformPosition.position, vectorToPlayer, Color.green);
		//Debug.DrawRay(eyeTransformPosition.position, eyeTransformPosition.forward, Color.red);
		enemyStateMachine.Run();

	}

	private void FixedUpdate()
	{
		if (audioSourceIdle.isPlaying == false && Time.time - soundStartTime > soundDelay)
		{
			audioSourceIdle.clip = audioClips[Random.Range(0, 4)];
			audioSourceIdle.Play();
			soundStartTime = Time.time;
			soundDelay = Random.Range(minSoundDelay, maxSoundDelay);

		}
	}

	protected AudioSource CreateAudioSource(float volume)
	{
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
	/// Checks to see if the enemy can see the player in its field of view and sight range, and that the player is not obscured by objects.
	/// </summary>
	/// <param name="v"></param>
	/// <returns></returns>
	public bool TargetIsInSight()
	{ //Checks if the Enemy's Target is in sight by checking the different sight factors.
		if (TargetIsInRange() && TargetIsInFOV(vectorToPlayer) && CanSeeTarget(vectorToPlayer))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	protected virtual IEnumerator GradualLookAtPlayer()
	{
		while (Vector3.Angle(transform.forward, vectorToPlayer) > 5)
		{
			transform.forward = Vector3.RotateTowards(transform.forward, vectorToPlayer, Time.deltaTime * 5f, 0f);
			yield return null;
		}
		//Debug.Log("" + gameObject.transform.parent.gameObject + " is looking at player");
	}

	///Checks if the Enemy's Target is inside the specified field of view
	private bool TargetIsInFOV(Vector3 v)
	{
		float angleToTarget = Vector3.Angle(eyeTransformPosition.forward, v.normalized);
		if (angleToTarget <= fieldOfView / 2)
		{ //divide field of view by 2 because Vector3.Angle only goes 0-180 in a semicircle from front to back
			return true;
		}
		else
		{
			return false;
		}
	}

	///Is Target within sight range?
	private bool TargetIsInRange()
	{
		if (Vector3.Distance(transform.position, Target.transform.position) < visionRange)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	///Is the line to the target obscured by something?
	protected bool CanSeeTarget(Vector3 v)
	{
		Physics.Raycast(eyeTransformPosition.position, v, out RaycastHit hit, v.magnitude, ObscuringLayers);
		if (!hit.collider)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Checks if the enemy can attack its target by SphereCasting from the given gun position, to the target, checking for collisions on the way.
	/// </summary>
	/// <returns></returns>
	public bool TargetIsAttackable()
	{
		if (TargetIsInSight() && vectorToPlayer.magnitude <= attackRange)
		{
			if (!Physics.SphereCast(gunTransformPosition.position, 0.1f, vectorToPlayer, out _, vectorToPlayer.magnitude, ObscuringLayers))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Checks if the Enemy's weapon is aimed sufficiently towards the player.
	/// </summary>
	/// <returns></returns>
	public bool WeaponIsAimed()
	{
		Vector3 sightToPlayer = Vector3.ProjectOnPlane(vectorToPlayer.normalized, Vector3.up);
		Vector3 gunAim = Vector3.ProjectOnPlane(gunTransformPosition.forward, Vector3.up);
		float radAngle = Mathf.Acos((Vector3.Dot(sightToPlayer, gunAim)) / (sightToPlayer.magnitude * gunAim.magnitude));
		float degrees = radAngle * (180 / Mathf.PI);
		if (degrees < attackLimitDegrees)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	/// <summary>
	/// Does what it says it does: calculates delta-V between two vectors.
	/// </summary>
	/// <param name="v1">The first vector.</param>
	/// <param name="v2">The second vector.</param>
	/// <returns>Velocity between v2 and v1.</returns>
	public Vector3 CalculateTargetVelocity(Vector3 v1, Vector3 v2)
	{
		Vector3 v = v2 - v1;
		return v;
	}

	/// <summary>
	/// Calculates and returns the vector from a base transform A to a target transform B.
	/// </summary>
	/// <param name="A">Base transform.</param>
	/// <param name="B">Target transform.</param>
	/// <returns>The vector from A to B.</returns>
	public Vector3 GetVectorFromAtoB(Transform A, Transform B)
	{
		Vector3 v = B.position - A.position;
		return v;
	}

	public IEnumerator ScanArea()
	{
		Vector3 right = transform.right;
		Vector3 left = transform.right * -1;
		while (Vector3.Dot(transform.forward, right) < 0.9)
		{
			transform.forward = Vector3.RotateTowards(transform.forward, right, Time.deltaTime, 0f);
			yield return null;
		}
		while (Vector3.Dot(transform.forward, left) < 0.9)
		{
			transform.forward = Vector3.RotateTowards(transform.forward, left, Time.deltaTime, 0f);
			yield return null;
		}
	}

	/// <summary>
	/// Implements <c>TakeDamage()</c> from IPawn interface to deal damage to the enemy.
	/// </summary>
	/// <param name="amount">The amount of damage the enemy should take.</param>
	/// <returns></returns>
	public float TakeDamage(float amount, DamageType damageType)
	{
		//Debug.Log("" + gameObject + " took damage.");
		if (!isInCombat)
		{
			EventSystem.Current.FireEvent(new EnemyHurt()
			{
				Entity = this
			});
			//Debug.Log("" + this.gameObject.transform.parent.gameObject + "fired EH event");
		}
		if (this is Scout || this is Warrior) enemyAnimator.SetTrigger("Hit");
		currentHealth -= amount;
		if (currentHealth <= 0)
		{
			Die();
		}
		return currentHealth;
	}

	/// <summary>
	/// Implements <c>Heal()</c> from IPawn interface to heal the enemy.
	/// </summary>
	/// <param name="amount">The amount of healing the enemy should receive.</param>
	/// <returns></returns>
	public float Heal(float amount)
	{
		currentHealth += amount;
		if (currentHealth > maxHealth)
		{
			currentHealth = maxHealth;
		}
		return currentHealth;
	}

	private void Die()
	{
		StopAllCoroutines();

		EventSystem.Current.FireEvent(new EnemyDeathEvent(gameObject, maxHealth));

		if (this is Scout) EventSystem.Current.FireEvent(new ExplosionEffectEvent(deathExplosion, transform.position + new Vector3(0, 2.5f, 0), Vector3.up, 1f));
		else if (this is Warrior) EventSystem.Current.FireEvent(new ExplosionEffectEvent(deathExplosion, transform.position + new Vector3(0, 3f, 0), Vector3.up, 1f));
		else EventSystem.Current.FireEvent(new ExplosionEffectEvent(deathExplosion, transform.position + new Vector3(0, 1f, 0), Vector3.up, 1f));
		StopAllCoroutines();
		UnRegEventListeners();
		gameObject.transform.parent.gameObject.SetActive(false);
		//Destroy(gameObject.transform.parent.gameObject, 2f);
	}

	public void PlayAudio(int clipIndex, float volume, float minPitch, float maxPitch)
	{
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
	}

	// Signatures for all the behaviour transitions that the various enemies need to be able to implement.
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
	/// Implements ICapturable, ran on checkpoint load.
	/// </summary>
	/// <param name="wasEnabled">Whether or not the loading of the GameObject resulted in it being enabled.</param>
	public void OnLoad(bool wasEnabled)
	{
		if (!wasEnabled) UnRegEventListeners();
	}

	/// <summary>
	/// Implements ICaptureable interface so that the enemy can be saved by the checkpoint system.
	/// </summary>
	/// <returns></returns>
	public object GetPersistentCaptureID()
	{
		return Origin;
	}

	protected virtual void RegEventListeners() { }

	public virtual void UnRegEventListeners() { }
}