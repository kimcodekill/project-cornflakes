using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class EnemyWeaponBase : MonoBehaviour, IDamaging {
	//This class exists on its own, instead of implementing the Weapon-base that we use for player weapons
	//because I needed to make it different enough that it feels like inheritance wasn't quite appropriate.

	public Enemy owner;
	private float fireRate;
	private float damagePerShot;
	private float weaponSpread;
	private float attackRange;
	[SerializeField] [Tooltip("Needs to check for collision with the player, thus needs the layer for the player.")] private LayerMask playerLayer;
	private LineRenderer shotLine;
	private WaitForSeconds lineDuration = new WaitForSeconds(0.25f);
	[SerializeField] [Tooltip("Which Bullet gameobject to instantiate.")] private Bullet bulletPrefab;
	[SerializeField] [Tooltip ("Determines whether or not this weapon shot fire bullet projectiles or raycasts.")] private bool useBulletProjectile;
	[SerializeField] [Tooltip ("Determines how well the enemy should lead the target when firing.")] private float targetLeadFactor;
	//^ Needs to be moved from this class to the Enemy to keep things centralised. Pass through SetParams() instead.
	private Vector3 oldTargetPos;

	/// <summary>
	/// Accessed from the enemy that the weapon is equipped on, to set values for the weapon based on the enemy's stats.
	/// Passed from enemy instead of setting on the weapon to keep all different values for enemies centralised.
	/// </summary>
	/// <param name="owner">The enemy this is attached to.</param>
	/// <param name="rof">Rate of fire, in rounds per minute (rpm).</param>
	/// <param name="damage">Damage per round.</param>
	/// <param name="spreadAngle">The maximum spread of the enemy's attacks.</param>
	/// <param name="range">How far the enemy shoots.</param>
	public void SetParams(Enemy owner, float rof, float damage, float spreadAngle, float range) {
		this.owner = owner;
		fireRate = rof;
		damagePerShot = damage;
		weaponSpread = spreadAngle;
		attackRange = range;
	}

	private void Start() {
		shotLine = GetComponent<LineRenderer>();
		shotLine.enabled = false;
		targetLeadFactor = Mathf.Clamp(targetLeadFactor, 0f, 1f);
	}

	/// <summary>
	/// Implements IDamaging, accessed from a number of places to read how much damage the enemy will do.
	/// </summary>
	/// <returns></returns>
	public float GetDamage() {
		return damagePerShot;
	}

	/// <summary>
	/// Implements IDamaging, not strictly used on this script but if we want to make an enemy weapon that has an AoE this will be properly implemented.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="pos2"></param>
	/// <returns></returns>
	public float GetExplosionDamage(Vector3 pos, Vector3 pos2) {
		return 0;
	}
	
	/// <summary>
	/// Returns the weapons rpm as the time between shots in seconds.
	/// </summary>
	/// <returns></returns>
	public float GetFireRate() {
		return 60.0f / fireRate;
	}

	/// <summary>
	/// Attack function that actuially performs the enemy's attack. Called from EnemyAttackingState
	/// Can be done with either bullet projectile or raycast attack, as determined by the bool.
	/// </summary>
	public void DoAttack() {
		Vector3 attackVector = owner.GetVectorToTarget(owner.Target.transform, owner.gunTransform);
		if (useBulletProjectile) {
			Vector3 collatedAttackVector = attackVector + LeadTarget();
			Vector3 spreadedAttack = AddSpread(collatedAttackVector.normalized) * attackRange;
			Bullet bullet = Instantiate(bulletPrefab, owner.gunTransform.position, Quaternion.LookRotation(owner.gunTransform.right));
			//^ Going to change this to pull the bullets from our objectpooler instead, far more performant, but will leave for the time being. 
			bullet.Initialize(owner.gunTransform.position + spreadedAttack, this);
		}
		if (!useBulletProjectile) {
			Vector3 spreadedAttack = AddSpread(attackVector.normalized)* attackRange;
			shotLine.SetPosition(0, owner.gunTransform.position);
			shotLine.SetPosition(1, owner.gunTransform.position + spreadedAttack);
			if (Physics.Raycast(owner.gunTransform.position, spreadedAttack, out RaycastHit hit, attackRange, playerLayer)) {
				if (hit.collider.gameObject.GetComponent<PlayerController>()) {
					shotLine.SetPosition(1, hit.point);
					EventSystem.Current.FireEvent(new HitEvent {
						Description = " " + owner.gameObject.name + " hit " + owner.Target.name,
						Source = gameObject,
						Target = owner.Target.gameObject
					});
				}
			}
		StartCoroutine(ShotEffect());
		}
	}

	private Vector3 AddSpread(Vector3 direction) {
		return new Vector3(
			Random.Range(-weaponSpread, weaponSpread) + direction.x,
			Random.Range(-weaponSpread, weaponSpread) + direction.y,
			direction.z
			).normalized;
	}

	private Vector3 LeadTarget() {
		Vector3 currentPos = owner.Target.transform.position;
		Vector3 leadingVector = currentPos - oldTargetPos;
		oldTargetPos = currentPos;
		return leadingVector * targetLeadFactor;
	}

	private IEnumerator ShotEffect() {
		shotLine.enabled = true;
		yield return lineDuration;
		shotLine.enabled = false;
	}

}

