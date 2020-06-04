using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class EnemyWeaponBase : MonoBehaviour, IDamaging {
	///This class exists on its own, instead of implementing the Weapon-base that we use for player weapons
	///because I needed to make it different enough that it feels like inheritance wasn't quite appropriate.

	public EnemyBase owner;
	private float fireRate;
	private float damagePerShot;
	private float weaponSpread;
	private float attackRange;
	[SerializeField] [Range (0,1)] [Tooltip ("Determines how well the enemy should lead the target when firing.")] private float targetLeadFactor;
	private Vector3 previousTargetDir;

	/// <summary>
	/// Accessed from the enemy that the weapon is equipped on, to set values for the weapon based on the enemy's stats.
	/// Passed from enemy instead of setting on the weapon to keep all different values for enemies centralised.
	/// </summary>
	/// <param name="owner">The enemy this is attached to.</param>
	/// <param name="rof">Rate of fire, in rounds per minute (rpm).</param>
	/// <param name="damage">Damage per round.</param>
	/// <param name="spreadAngle">The maximum spread of the enemy's attacks.</param>
	/// <param name="range">How far the enemy shoots.</param>
	public void SetParams(EnemyBase owner, float rof, float damage, float spreadAngle, float range) {
		this.owner = owner;
		fireRate = rof;
		damagePerShot = damage;
		weaponSpread = spreadAngle;
		attackRange = range;
	}

	private void Start() {
		targetLeadFactor = Mathf.Clamp(targetLeadFactor, 0f, 1f);
	}

	/// <summary>
	/// Implements IDamaging, accessed from a number of places to read how much damage the enemy will do.
	/// </summary>
	/// <returns>The damage dealt per hit.</returns>
	public float GetDamage() {
		return damagePerShot;
	}

	public float GetDamage(float distanceRadius)
	{
		return damagePerShot;
	}

	/// <summary>
	/// Implements IDamaging, not strictly used on this script but if we want to make an enemy weapon that has an AoE this will be properly implemented.
	/// </summary>
	/// <param name="pos"></param>
	/// <param name="pos2"></param>
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
		owner.PlayAudio(4, 1, 0.8f, 1.3f);
		if (owner.enemyAnimator != null) owner.enemyAnimator.SetTrigger("FireWeapon");
		Vector3 attackVector = owner.GetVectorFromAtoB(owner.gunTransformPosition, owner.Target.transform);

		Vector3 collatedAttackVector = LeadTarget(attackVector);
		Vector3 spreadedAttack = RandomInCone(weaponSpread, collatedAttackVector.normalized) * attackRange;
		GameObject bullet = ObjectPooler.Instance.SpawnFromPool("EnemyBullet", owner.gunTransformPosition.position, Quaternion.identity);
		bullet.GetComponent<Bullet>().Initialize(owner.gunTransformPosition.position + spreadedAttack, this);
	}

	private Vector3 LeadTarget(Vector3 attackVector) {
		Vector3 targetVelocity = owner.CalculateTargetVelocity(previousTargetDir, attackVector);
		Vector3 leadVelocity = targetVelocity * targetLeadFactor;
		previousTargetDir = attackVector;
		if (attackVector.sqrMagnitude < 100)
			return attackVector;
		else return attackVector + leadVelocity;
	}

	/// <summary>
	/// New and improved spread function! Generates a random vector inside a cone around a given axis, where the width of the cone is determined by the spread angle.
	/// </summary>
	/// <param name="spreadAngle">The polar angle giving the width of the cone.</param>
	/// <param name="axis">The axis the cone should be drawn around.</param>
	/// <returns></returns>
	public Vector3 RandomInCone(float spreadAngle, Vector3 axis) {
		float radians = Mathf.PI / 180 * spreadAngle/2;
		float dotProduct = Random.Range(Mathf.Cos(radians), 1);
		float polarAngle = Mathf.Acos(dotProduct);
		float azimuth = Random.Range(-Mathf.PI, Mathf.PI);
		Vector3 yProjection = Vector3.ProjectOnPlane(Vector3.up, axis);
		if(Vector3.Dot(axis, Vector3.up) > 0.9f) {
			yProjection = Vector3.ProjectOnPlane(Vector3.forward, axis);
		}
		Vector3 y = yProjection.normalized;
		Vector3 x = Vector3.Cross(y, axis);
		Vector3 pointOnSphericalCap = Mathf.Sin(polarAngle) * (Mathf.Cos(azimuth) * x + Mathf.Sin(azimuth) * y) + Mathf.Cos(polarAngle) * axis;
		return pointOnSphericalCap;
	}

}

