using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponBase : MonoBehaviour, IDamaging {

	private Enemy owner;
	private float fireRate;
	private float damagePerShot;
	private float weaponSpread;
	private float attackRange;
	[SerializeField] private LayerMask playerLayer;
	private LineRenderer shotLine;
	private WaitForSeconds lineDuration = new WaitForSeconds(0.25f);

	public void SetParams(Enemy owner, float rof, float damage, float radius, float range) {
		this.owner = owner;
		fireRate = rof;
		damagePerShot = damage;
		weaponSpread = radius;
		attackRange = range;
	}

	private void Start() {
		shotLine = GetComponent<LineRenderer>();
		shotLine.enabled = false;
	}

	private void Update() {
		Debug.DrawRay(owner.gunTransform.position, owner.GetVectorToTarget(owner.Target.transform, owner.gunTransform), Color.blue);
		//Vector3 attackVector = owner.GetVectorToTarget(owner.Target.transform, owner.gunTransform);
		//Debug.DrawRay(owner.gunTransform.position, attackVector, Color.cyan);
	}

	public float GetDamage() {
		return damagePerShot;
	}

	public float GetFireRate() {
		return 60.0f / fireRate;
	}

	public void DoAttack() {
		//Debug.Log("shooting");
		shotLine.SetPosition(0, owner.gunTransform.position);
		Vector3 attackVector = owner.GetVectorToTarget(owner.Target.transform, owner.gunTransform);
		//Debug.DrawRay(owner.gunTransform.position, attackVector, Color.cyan);
		//Debug.DrawLine(owner.gunTransform.position, attackVector, Color.blue);
		Vector3 spreadedAttack = AddSpread(attackVector).normalized * attackRange;
		//Debug.DrawRay(owner.gunTransform.position, spreadedAttack, Color.blue);
		shotLine.SetPosition(1, owner.gunTransform.position + spreadedAttack);
		if (Physics.Raycast(owner.gunTransform.position, spreadedAttack, out RaycastHit hit, attackRange, playerLayer)) {
			//Debug.Log(hit.collider.gameObject);
			if (hit.collider.GetComponent<PlayerController>()) {
				//Debug.Log("hit player");
				shotLine.SetPosition(1, hit.point);
				EventSystem.Current.FireEvent(new HitEvent {
					Description = " " + owner.gameObject.name + " hit " + owner.Target.name,
					Source = gameObject,
					Target = owner.Target.gameObject
				});
			}
		}
		StartCoroutine(ShotEffect());
		//else Debug.Log("missed");
	}

	private Vector3 AddSpread(Vector3 direction) {
		return new Vector3(Random.Range(-weaponSpread, weaponSpread) + direction.x, Random.Range(-weaponSpread, weaponSpread) + direction.y, Random.Range(-weaponSpread, weaponSpread) + direction.z).normalized;
	}

	private IEnumerator ShotEffect() {
		shotLine.enabled = true;
		//Debug.Log("line on");
		yield return lineDuration;
		shotLine.enabled = false;
	}

}

