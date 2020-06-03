using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class Warrior : NavMeshEnemy, ILootable
{

	private new void Start() {
		agent.avoidancePriority = Random.Range(51, 99);
		agent.radius = defaultAgentAvoidanceRadius;
		base.Start();
	}

	/// <summary>
	/// Warrior Idle-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Idle() {
		eyeTransformPosition.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance > 0.5f) {
			yield return null;
		}
		yield return StartCoroutine(ScanArea());
		//Debug.Log("scanned");
		StartCoroutine("Idle");
	}

	/// <summary>
	/// Warrior Patrol-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Patrol() {
		eyeTransformPosition.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.5f) {
			GoToNextPoint();
		}
		yield return StartCoroutine(ScanArea());
		StartCoroutine("Patrol");
	}

	/// <summary>
	/// Warrior Alerted-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Alerted() {
		agent.destination = Target.transform.position;
		while (!agent.pathPending && agent.remainingDistance > attackRange * 0.8f) {
			transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(vectorToPlayer.x, 0, vectorToPlayer.z), Time.deltaTime * 5f, 0f);
			yield return null;
		}
		yield return null;
		StartCoroutine("Alerted");
	}

	/// <summary>
	/// Warrior Attack-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Attack() {

		while (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.75f) {
			agent.destination = Target.transform.position;
			yield return null;
		}
		agent.ResetPath();
		while (!agent.hasPath) {
			while (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.75f) {
				agent.destination = Target.transform.position;
				yield return null;
			}
			agent.ResetPath();
			transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(vectorToPlayer.x, 0, vectorToPlayer.z), Time.deltaTime * 5f, 0f);
			eyeTransformPosition.rotation = Quaternion.LookRotation(Vector3.RotateTowards(eyeTransformPosition.forward, vectorToPlayer, Time.deltaTime * 7.5f, 0f));
			yield return null;
		}
		yield return null;
		StartCoroutine("Attack");
	}	

	//All of the below handle transitions between the different behaviours.
	public override void StartPatrolBehaviour() {
		agent.ResetPath();
		StartCoroutine("Patrol");
	}

	public override void StopPatrolBehaviour() {
		StopCoroutine("Patrol");
	}

	public override void StartAlertedBehaviour() {
		agent.ResetPath();
		StartCoroutine("Alerted");
	}

	public override void StopAlertedBehaviour() {
		StopCoroutine("Alerted");
	}

	public override void StartAttackBehaviour() {
		agent.ResetPath();
		StartCoroutine(AvoidanceRadiusLerp(attackingAgentAvoidanceRadius));
		StartCoroutine("Attack");
	}

	public override void StopAttackBehaviour() {
		StopCoroutine("Attack");
		StartCoroutine(AvoidanceRadiusLerp(defaultAgentAvoidanceRadius));
	}

	public override void StartIdleBehaviour() {
		agent.ResetPath();
		StartCoroutine("Idle");
	}

	public override void StopIdleBehaviour() {
		StopCoroutine("Idle");
	}

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 1f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.5f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.2f : 0f,
			[LootTable.Nothing] = 0.0f,
		};
	}


}
