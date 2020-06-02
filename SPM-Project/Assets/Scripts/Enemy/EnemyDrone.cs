using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class EnemyDrone : EnemyBase, ILootable {

	[SerializeField] [Tooltip("How far from its origin should the Drone patrol.")] private float patrolBungeeDistance;
	[SerializeField] private SphereCollider body;

	/// <summary>
	/// Finds a new position for patrolling.
	/// </summary>
	/// <returns></returns>
	private Vector3 FindRandomPosition(Vector3 startingPos, float moveRange) {
		Vector3 randomPos = startingPos + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange/3, moveRange/3), Random.Range(-moveRange, moveRange));
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, body.radius, (randomPos - transform.position).normalized, (randomPos - transform.position).magnitude + 1f, ObscuringLayers);
		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].collider != null) {
				return FindRandomPosition(startingPos + hits[i].normal, moveRange);
			}
		}
		return randomPos;
	}
	
	/// <summary>
	/// Drone Idle-behaviour.
	/// </summary>
	private IEnumerator Idle() {
		Vector3 newPos = FindRandomPosition(Origin, patrolBungeeDistance);
		while (Vector3.Distance(transform.position, newPos) > 0.05f) {
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (newPos - transform.position), Time.deltaTime * 3f, 0f));
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * 3/*movementSpeed*/);
			yield return null;
		}
		yield return null;
		StartCoroutine("Idle");
	}

	/// <summary>
	/// Drone's Attack-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Attack() {
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, vectorToPlayer, Time.deltaTime * 5f, 0f));
		yield return null;
		StartCoroutine("Attack");
	}

	/// <summary>
	/// Drone's Alerted-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Alerted() {
		
		if ((Vector3.Distance(transform.position, Target.transform.position) > attackRange)) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * 3/*movementSpeed*/);
		}
		yield return null;
		StartCoroutine("Alerted");
	}

	//Behaviour transitions.
	public override void StartIdleBehaviour() {
		StartCoroutine("Idle");
	}

	public override void StopIdleBehaviour() {
		StopCoroutine("Idle");
	}

	public override void StartAttackBehaviour() {
		StartCoroutine("Attack");
	}

	public override void StopAttackBehaviour() {
		StopCoroutine("Attack");
	}

	public override void StartAlertedBehaviour() {
		StartCoroutine("Alerted");
	}

	public override void StopAlertedBehaviour() {
		StopCoroutine("Alerted");
	}

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.2f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.2f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.2f : 0f,
			[LootTable.Nothing] = 0.8f,
		};
	}
}
