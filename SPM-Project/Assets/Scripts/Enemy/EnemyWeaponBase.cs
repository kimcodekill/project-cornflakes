﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponBase : MonoBehaviour, IDamaging {

	public Enemy owner;
	private float fireRate;
	private float damagePerShot;
	private float weaponSpread;
	private float attackRange;
	[SerializeField] private LayerMask playerLayer;
	private LineRenderer shotLine;
	private WaitForSeconds lineDuration = new WaitForSeconds(0.25f);
	[SerializeField] private Bullet bulletPrefab;
	[SerializeField] private bool useBulletProjectile;
	[SerializeField] private float targetLeadFactor;
	private Vector3 oldTargetPos;

	public void SetParams(Enemy owner, float rof, float damage, float spread, float range) {
		this.owner = owner;
		fireRate = rof;
		damagePerShot = damage;
		weaponSpread = spread;
		attackRange = range;
	}

	private void Start() {
		shotLine = GetComponent<LineRenderer>();
		shotLine.enabled = false;
		targetLeadFactor = Mathf.Clamp(targetLeadFactor, 0f, 1f);
	}

	public float GetDamage() {
		return damagePerShot;
	}

	public float GetExplosionDamage(Vector3 pos, Vector3 pos2) {
		return 0;
	}
	
	public float GetFireRate() {
		return 60.0f / fireRate;
	}

	public void DoAttack() {
		Vector3 attackVector = owner.GetVectorToTarget(owner.Target.transform, owner.gunTransform);
		if (useBulletProjectile) {
			Vector3 collatedAttackVector = attackVector + LeadTarget();
			Vector3 spreadedAttack = RandomInCone(weaponSpread, collatedAttackVector.normalized) * attackRange;
			Bullet bullet = Instantiate(bulletPrefab, owner.gunTransform.position, Quaternion.identity);
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

	//private Vector3 AddSpread(Vector3 direction) {
	//	return new Vector3(
	//		Random.Range(-weaponSpread, weaponSpread) + direction.x,
	//		Random.Range(-weaponSpread, weaponSpread) + direction.y,
	//		direction.z
	//		).normalized;
	//}

	private Vector3 LeadTarget() {
		Vector3 currentPos = owner.Target.transform.position;
		Vector3 leadingVector = currentPos - oldTargetPos;
		oldTargetPos = currentPos;
		return leadingVector * targetLeadFactor;
	}

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
		yield return lineDuration;
		shotLine.enabled = false;
	}

}

