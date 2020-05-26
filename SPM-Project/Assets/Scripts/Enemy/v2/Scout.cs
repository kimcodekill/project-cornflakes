using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class Scout : MobileEnemy
{

	[SerializeField] [Tooltip("How far the Scout should look while searching.")] public float searchRange;
	[SerializeField] [Tooltip("How many search loops the Scout should go through.")] private int searchLoops;
	[SerializeField] [Tooltip("What radius the Scout should use for avoidance while searching.")] private float searchingAvoidanceRadius;


	private new void Start() {
		agent.avoidancePriority = Random.Range(0, 50);
		agent.radius = defaultAgentAvoidanceRadius;
		base.Start();
	}
	
	/// <summary>
	/// Scout Idle-behaviour.
	/// </summary>
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
	/// Scout Patrol-behaviour.
	/// </summary>
	private IEnumerator Patrol() {
		eyeTransform.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.5f) {
			GoToNextPoint();
		}
		yield return null;
		StartCoroutine("Patrol");
	}

	/// <summary>
	/// Scout Alerted-behaviour.
	/// </summary>
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
	/// Scout Attack-behaviour.
	/// </summary>
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

	/// <summary>
	/// Scout Search-behaviour.
	/// This whole function is a steaming pile of garbage. It works, but man is it bad. Needs major overhaul.
	/// A lot of code repetition that I tried to get rid of, but couldn't really find a way to make it work within the coroutine, so it will stay for now.
	/// </summary>
	private IEnumerator Search() {
		HasFinishedSearching = false;
		float searches = 0;
		bool areaScanned = false;
		Vector3 lastKnownPosition1, lastKnownPosition2;
		lastKnownPosition1 = Target.transform.position;
		yield return null;
		lastKnownPosition2 = Target.transform.position;
		Vector3 targetLocation = lastKnownPosition1 + CalculateTargetVelocity(lastKnownPosition1, lastKnownPosition2).normalized;
		agent.destination = targetLocation;
		while (agent.pathPending) {
			yield return null;
		}
		while (agent.remainingDistance > 1.5f) {
			yield return null;
		}
		areaScanned = false;
		while (!areaScanned) {
			Vector3 scanLeft = (transform.position + (transform.right * -1)) - transform.position;
			Vector3 scanRight = (transform.position + transform.right) - transform.position;
			while (Vector3.Dot(transform.forward, scanLeft) < 0.95) {
				transform.forward = Vector3.RotateTowards(transform.forward, scanLeft, Time.deltaTime * 3, 0f);
				yield return null;
			}
			while (Vector3.Dot(transform.forward, scanRight) < 0.95) {
				transform.forward = Vector3.RotateTowards(transform.forward, scanRight, Time.deltaTime * 3, 0f);
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			areaScanned = true;
		}
		while (searches < searchLoops) {
			agent.destination = FindNewRandomNavMeshPoint(targetLocation + (CalculateTargetVelocity(lastKnownPosition1, lastKnownPosition2).normalized * searchRange / 2), searchRange);
			while (agent.pathPending) {
				yield return null;
			}
			while (agent.remainingDistance > 1.5f) {
				yield return null;
			}
			areaScanned = false;
			while (!areaScanned) {
				Vector3 scanLeft = (transform.position + (transform.right * -1)) - transform.position;
				Vector3 scanRight = (transform.position + transform.right) - transform.position;
				while (Vector3.Dot(transform.forward, scanLeft) < 0.95f) {
					transform.forward = Vector3.RotateTowards(transform.forward, scanLeft, Time.deltaTime * 3, 0f);
					yield return null;
				}
				while (Vector3.Dot(transform.forward, scanRight) < 0.95f) {
					transform.forward = Vector3.RotateTowards(transform.forward, scanRight, Time.deltaTime * 3, 0f);
					yield return null;
				}
				yield return new WaitForSeconds(1f);
				areaScanned = true;
			}
			yield return new WaitForSeconds(1f);
			searches++;
		}
		StartCoroutine(AvoidanceRadiusLerp(defaultAgentAvoidanceRadius));
		HasFinishedSearching = true;
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

	public override void StartSearchBehaviour() {
		agent.ResetPath();
		StartCoroutine(AvoidanceRadiusLerp(searchingAvoidanceRadius));
		StartCoroutine("Search");
	}

	public override void StopSearchBehaviour() {
		StopCoroutine("Search");
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
