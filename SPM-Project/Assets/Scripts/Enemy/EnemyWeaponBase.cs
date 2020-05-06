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
	[SerializeField] [Tooltip ("For how long should the LineRenderer be drawn? Only applies to Raycast attacks.")] private float lineDuration;
	[SerializeField] [Tooltip("Which Bullet gameobject to instantiate.")] private Bullet bulletPrefab;
	[SerializeField] [Tooltip ("Determines whether or not this weapon shot fire bullet projectiles or raycasts.")] private bool useBulletProjectile;
	[SerializeField] [Tooltip ("Determines how well the enemy should lead the target when firing.")] private float targetLeadFactor;
	//^ Needs to be moved from this class to the Enemy to keep things centralised. Pass through SetParams() instead.
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
		//Debug.Log(owner.gameObject + " attacked");
		Vector3 attackVector = owner.GetVectorToTarget(owner.Target.transform, owner.gunTransform);
		if (useBulletProjectile) {
			Vector3 collatedAttackVector = LeadTarget(attackVector);
			Vector3 spreadedAttack = RandomInCone(weaponSpread, collatedAttackVector.normalized) * attackRange;
			Bullet bullet = Instantiate(bulletPrefab, owner.gunTransform.position, Quaternion.identity);
			//^ Going to change this to pull the bullets from our objectpooler instead, far more performant, but will leave for the time being. 
			bullet.Initialize(owner.gunTransform.position + spreadedAttack, this);
		}
		if (!useBulletProjectile) {
			Vector3 spreadedAttack = RandomInCone(weaponSpread, attackVector.normalized) * attackRange;
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
		StartCoroutine(RaycastShotEffect());
		}
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

	private IEnumerator RaycastShotEffect() {
		shotLine.enabled = true;
		yield return new WaitForSeconds(lineDuration);
		shotLine.enabled = false;
	}

}

