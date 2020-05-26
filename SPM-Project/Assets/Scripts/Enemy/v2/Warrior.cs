using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class Warrior : MobileEnemy
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
		eyeTransform.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance > 0.5f) {
			yield return null;
		}
		Vector3 right = transform.right;
		Vector3 left = transform.right * -1;
		while (Vector3.Dot(transform.forward, right) < 0.9) {
			transform.forward = Vector3.RotateTowards(transform.forward, right, Time.deltaTime, 0f);
			yield return null;
		}
		while (Vector3.Dot(transform.forward, left) < 0.9) {
			transform.forward = Vector3.RotateTowards(transform.forward, left, Time.deltaTime, 0f);
			yield return null;
		}
		yield return new WaitForSeconds(1f);

	}

	/// <summary>
	/// Warrior Patrol-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Patrol() {
		eyeTransform.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.5f) {
			GoToNextPoint();
		}
		yield return null;
		StartCoroutine(Idle());
		yield return null;
		StartCoroutine("Patrol");
	}

	/// <summary>
	/// Warrior Alerted-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Alerted() {
		agent.destination = Target.transform.position;
		while (!agent.pathPending && agent.remainingDistance > attackRange) {
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
			eyeTransform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(eyeTransform.forward, vectorToPlayer, Time.deltaTime * 7.5f, 0f));
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
			["Pickups/HealthPickup"] = 0.4f,
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.2f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.2f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.2f : 0f,
		};
	}


}
