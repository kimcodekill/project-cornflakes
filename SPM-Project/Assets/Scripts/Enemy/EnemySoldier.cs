using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
public class EnemySoldier : Enemy, ILootable {

	[SerializeField] [Tooltip("How far the Soldier should look while searching.")]public float searchRange;
	[SerializeField] [Tooltip("How many search loops the Soldier should go through.")] private int searchLoops;
	[SerializeField] [Tooltip("What radius the Soldier should use for avoidance while attacking.")] private float attackAvoidanceRadius;
	[SerializeField] [Tooltip("What radius the Soldier should use for avoidance while patrolling/idling.")] private float defaultAvoidanceRadius;
	[SerializeField] [Tooltip("What radius the Soldier should use for avoidance while searching.")] private float searchAvoidanceRadius;

	private Transform[] points;
	private NavMeshAgent agent;
	private int destPoint = 0;

	/// <summary>
	/// Doesn't do anything yet.
	/// Was thinking that this may be useful if we need the Soldier to find a new position nearby because of some blockage or other reason.
	/// </summary>
	public void AgentReposition() {
		agent.destination = FindNewRandomNavMeshPoint(transform.position, 1f);
	}

	protected new void Awake() {
		if (IsPatroller) points = GetComponent<PatrollingSoldier>().patrolPoints;
		agent = GetComponent<NavMeshAgent>();
		base.Awake();
	}

	private new void Start() {
		agent.avoidancePriority = Random.Range(0, 99);
		agent.radius = defaultAvoidanceRadius;
		base.Start();
	}

	/// <summary>
	/// Simple patrol from point to point.
	/// </summary>
	private void GoToNextPoint() {
		if (points.Length == 0)
			return;
		agent.destination = points[destPoint].position;
		destPoint = (destPoint + 1) % points.Length;
	}

	private void GoToGuardPoint() {
		agent.destination = Origin;
	}

	/// <summary>
	/// Soldier Idle-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Idle() {
		eyeTransform.forward = transform.forward;
		GoToGuardPoint();
		while (!agent.pathPending && agent.remainingDistance > 0.5f) {
			yield return null;
		}
		Vector3 rnd = transform.position + new Vector3(Random.Range(-100,100), 0, Random.Range(-100, 100));
		while (Vector3.Dot(transform.forward, rnd.normalized) < 0.9) {
			transform.forward = Vector3.RotateTowards(transform.forward, rnd, Time.deltaTime, 0f);
			yield return null;
		}
		yield return new WaitForSeconds(1f);
		StartCoroutine("Idle");
	}

	/// <summary>
	/// Soldier Patrol-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Patrol() {
		eyeTransform.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.5f) {
			GoToNextPoint();
		}
		yield return null;
		StartCoroutine("Patrol");
	}

	/// <summary>
	/// Soldier Alerted-behaviour.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Alerted() {
		agent.destination = Target.transform.position;
		while (!agent.pathPending && agent.remainingDistance > attackRange*0.8f) {
			transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(vectorToPlayer.x, 0, vectorToPlayer.z), Time.deltaTime * 5f, 0f);
			yield return null;
		}
		yield return null;
		StartCoroutine("Alerted");
	}

	/// <summary>
	/// Soldier Attack-behaviour.
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

	/// <summary>
	/// Soldier Search-behaviour.
	/// 
	/// This whole function is a steaming pile of garbage. It works, but man is it bad. Needs major overhaul.
	/// A lot of code repetition that I tried to get rid of, but couldn't really find a way to make it work within the coroutine, so it will stay for now.
	/// </summary>
	/// <returns></returns>
	private IEnumerator Search() {
		FinishedSearching = false;
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
			Vector3 scanLeft = (transform.position + (transform.right*-1)) - transform.position;
			Vector3 scanRight = (transform.position + transform.right) - transform.position;
			while (Vector3.Dot(transform.forward, scanLeft) < 0.95) {
				transform.forward = Vector3.RotateTowards(transform.forward, scanLeft, Time.deltaTime*3, 0f);
				yield return null;
			}
			while (Vector3.Dot(transform.forward, scanRight) < 0.95) {
				transform.forward = Vector3.RotateTowards(transform.forward, scanRight, Time.deltaTime*3, 0f);
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			areaScanned = true;
		}
		while (searches < searchLoops) {
			agent.destination = FindNewRandomNavMeshPoint(targetLocation + (CalculateTargetVelocity(lastKnownPosition1, lastKnownPosition2).normalized * searchRange/2), searchRange);
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
					transform.forward = Vector3.RotateTowards(transform.forward, scanLeft, Time.deltaTime*3, 0f);
					yield return null;
				}
				while (Vector3.Dot(transform.forward, scanRight) < 0.95f) {
					transform.forward = Vector3.RotateTowards(transform.forward, scanRight, Time.deltaTime*3, 0f);
					yield return null;
				}
				yield return new WaitForSeconds(1f);
				areaScanned = true;
			}
			yield return new WaitForSeconds(1f);
			searches++;
		}
		StartCoroutine(AvoidanceLerp(defaultAvoidanceRadius));
		FinishedSearching = true;
	}

	/// <summary>
	/// Finds a new random point on the NavMesh within range, used for the Soldier's search behaviour.
	/// </summary>
	/// <param name="startPos">The origin point for the area.</param>
	/// <param name="range">How large of a sphere should be used to find the random point.</param>
	/// <returns></returns>
	private Vector3 FindNewRandomNavMeshPoint(Vector3 startPos, float range) {
		Vector3 newPoint = Random.insideUnitSphere * range;
		newPoint = startPos + newPoint;
		Vector3 finalPosition = startPos + Vector3.zero;
		if (NavMesh.SamplePosition(newPoint, out NavMeshHit hit, range + agent.height, NavMesh.AllAreas)) {
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	/// <summary>
	/// Smoothing for the change in avoidance radius between the different states to avoid snapping.
	/// </summary>
	/// <returns></returns>
	private IEnumerator AvoidanceLerp(float avoidance) {
		while(agent.radius != avoidance) {
			agent.radius = Mathf.MoveTowards(agent.radius, avoidance, Time.deltaTime);
			yield return null;
		}
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
		StartCoroutine(AvoidanceLerp(attackAvoidanceRadius));
		StartCoroutine("Attack");
	}

	public override void StopAttackBehaviour() {
		StopCoroutine("Attack");
		StartCoroutine(AvoidanceLerp(defaultAvoidanceRadius));
	}

	public override void StartSearchBehaviour() {
		agent.ResetPath();
		StartCoroutine(AvoidanceLerp(searchAvoidanceRadius));
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
