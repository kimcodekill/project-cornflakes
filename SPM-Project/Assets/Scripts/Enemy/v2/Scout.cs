using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class Scout : NavMeshEnemy, ILootable
{
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
		eyeTransformPosition.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance > 0.5f) {
			yield return null;
		}
		yield return StartCoroutine(ScanArea());
		//Debug.Log("scanned");
		StartCoroutine("Idle");
	}

	/// <summary>
	/// Scout Patrol-behaviour.
	/// </summary>
	private IEnumerator Patrol() {
		eyeTransformPosition.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.5f) {
			GoToNextPoint();
		}
		yield return StartCoroutine(ScanArea());
		StartCoroutine("Patrol");
	}

	/// <summary>
	/// Scout Alerted-behaviour.
	/// </summary>
	private IEnumerator Alerted() {
		agent.destination = FindNearestOnMesh(Target.transform.position);
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
			agent.destination = FindNearestOnMesh(Target.transform.position);
			yield return null;
		}
		agent.ResetPath();
		while (!agent.hasPath) {
			while (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.75f) {
				agent.destination = FindNearestOnMesh(Target.transform.position);
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

	/// <summary>
	/// Scout Search-behaviour.
	/// </summary>
	private IEnumerator Search() {
		HasFinishedSearching = false;
		Vector3 lastKnownPosition1, lastKnownPosition2;
		lastKnownPosition1 = Target.transform.position;
		yield return new WaitForSeconds(0.25f);
		lastKnownPosition2 = Target.transform.position;
		Vector3 targetLocation = lastKnownPosition1 + CalculateTargetVelocity(lastKnownPosition1, lastKnownPosition2).normalized;
		agent.destination = FindNearestOnMesh(targetLocation);
		//Debug.Log("" + agent.destination);
		while (agent.pathPending) {
			yield return null;
		}
		while (agent.remainingDistance > 1.5f) {
			yield return null;
		}
		//Debug.Log("break");
		yield return StartCoroutine(ScanArea());
		StartCoroutine(AvoidanceRadiusLerp(defaultAgentAvoidanceRadius));
		HasFinishedSearching = true;
	}

	//All of the below handle transitions between the different behaviours.
	public override void StartPatrolBehaviour() {
		isInCombat = false;
		agent.ResetPath();
		StartCoroutine("Patrol");
	}

	public override void StopPatrolBehaviour() {
		StopCoroutine("Patrol");
	}

	public override void StartAlertedBehaviour() {
		isInCombat = true;
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
		isInCombat = false;
		agent.ResetPath();
		StartCoroutine("Idle");
	}

	public override void StopIdleBehaviour() {
		StopCoroutine("Idle");
	}

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.4f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.2f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.2f : 0f,
			[LootTable.Nothing] = 0.5f,
		};
	}



}
