using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : Enemy, ILootable {

	private Transform[] points;
	private NavMeshAgent agent;
	private int destPoint = 0;
	[SerializeField] public float searchRange;
	[SerializeField] private int searchLoops;
	[SerializeField] private float attackAvoidanceRadius;
	[SerializeField] private float defaultAvoidanceRadius;
	[SerializeField] private float searchAvoidanceRadius;


	public void AgentReposition() {
		agent.destination = FindNewRandomNavMeshPoint(transform.position, 1f);
	}

	protected void Awake() {
		if (IsPatroller) points = GetComponent<PatrollingSoldier>().patrolPoints;
		agent = GetComponent<NavMeshAgent>();
	}

	private void Start() {
		agent.avoidancePriority = Random.Range(0, 99);
		agent.radius = defaultAvoidanceRadius;
		base.Start();
	}

	private void GoToNextPoint() {
		if (points.Length == 0)
			return;
		agent.destination = points[destPoint].position;
		destPoint = (destPoint + 1) % points.Length;
	}

	private void GoToGuardPoint() {
		agent.destination = Origin;
	}

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

	private IEnumerator Patrol() {
		eyeTransform.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.5f) {
			GoToNextPoint();
		}
		yield return null;
		StartCoroutine("Patrol");
	}

	private IEnumerator Alerted() {
		agent.destination = Target.transform.position;
		while (!agent.pathPending && agent.remainingDistance > attackRange*0.8f) {
			transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(vectorToPlayer.x, 0, vectorToPlayer.z), Time.deltaTime * 5f, 0f);
			yield return null;
		}
		yield return null;
		StartCoroutine("Alerted");
	}

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
		//agent.radius = defaultAvoidanceRadius;
		StartCoroutine(AvoidanceLerp(defaultAvoidanceRadius));
		FinishedSearching = true;
	}

	private Vector3 FindNewRandomNavMeshPoint(Vector3 startPos, float range) {
		Vector3 newPoint = Random.insideUnitSphere * range;
		newPoint = startPos + newPoint;
		Vector3 finalPosition = startPos + Vector3.zero;
		if (NavMesh.SamplePosition(newPoint, out NavMeshHit hit, range + agent.height, NavMesh.AllAreas)) {
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	private IEnumerator AvoidanceLerp(float avoidance) {
		//Debug.Log("radius" + avoidance);
		//Debug.Log(agent.radius);
		while(agent.radius != avoidance) {
			//Debug.Log("true");
			agent.radius = Mathf.MoveTowards(agent.radius, avoidance, Time.deltaTime);
			yield return null;
		}
	}

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
		//agent.radius = attackAvoidanceRadius;
		StartCoroutine("Attack");
	}

	public override void StopAttackBehaviour() {
		StopCoroutine("Attack");
		StartCoroutine(AvoidanceLerp(defaultAvoidanceRadius));

		//agent.radius = defaultAvoidanceRadius;
	}

	public override void StartSearchBehaviour() {
		agent.ResetPath();
		StartCoroutine(AvoidanceLerp(searchAvoidanceRadius));

		//agent.radius = searchAvoidanceRadius;
		StartCoroutine("Search");
	}

	public override void StopSearchBehaviour() {
		StopCoroutine("Search");
		//agent.radius = defaultAvoidanceRadius;
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
