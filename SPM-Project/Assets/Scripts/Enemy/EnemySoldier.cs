using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : Enemy {

	private Transform[] points;
	private NavMeshAgent agent;
	private int destPoint = 0;
	public float searchRange;
	public int searchLoops;
	private Vector3 origin;

	protected void Awake() {
		if (IsPatroller) points = GetComponent<PatrollingSoldier>().patrolPoints;
		origin = transform.position;
		agent = GetComponent<NavMeshAgent>();
	}

	private void Start() {
		base.Start();
	}

	private void GoToNextPoint() {
		if (points.Length == 0)
			return;
		agent.destination = points[destPoint].position;
		destPoint = (destPoint + 1) % points.Length;
	}

	private void GoToGuardPoint() {
		agent.destination = origin;
	}

	private IEnumerator Idle() {
		eyeTransform.forward = transform.forward;
		GoToGuardPoint();
		while (!agent.pathPending && agent.remainingDistance > 0.5f) {
			yield return null;
		}
		Vector3 rnd = transform.position + new Vector3(Random.Range(-100,100), 0, Random.Range(-100, 100));
		//Debug.Log(rnd);
		while (Vector3.Dot(transform.forward, rnd.normalized) < 0.9) {
			transform.forward = Vector3.RotateTowards(transform.forward, rnd, Time.deltaTime, 0f);
			yield return null;
		}
		//Debug.Log("done");
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
		if (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.9f) {
			while (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.8f) {
				agent.destination = Target.transform.position;
				yield return null;
			}
		}
		agent.ResetPath();
		while (!agent.hasPath) {
			if (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.9f) {
				while (Vector3.Distance(transform.position, Target.transform.position) > attackRange * 0.8f) {
					agent.destination = Target.transform.position;
					yield return null;
				}
				agent.ResetPath();
			}
			transform.forward = Vector3.RotateTowards(transform.forward, new Vector3(vectorToPlayer.x, 0, vectorToPlayer.z), Time.deltaTime * 5f, 0f);
			eyeTransform.LookAt(Target.transform);
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
		while (agent.remainingDistance > 0.5f) {
			yield return null;
		}
		areaScanned = false;
		//Debug.Log("set scanned = false");
		while (!areaScanned) {
			//Debug.Log("scanning");
			Vector3 scanLeft = (transform.position + (transform.right*-1)) - transform.position;
			Vector3 scanRight = (transform.position + transform.right) - transform.position;
			while (Vector3.Dot(transform.forward, scanLeft) < 0.95) {
				//Debug.DrawRay(transform.position, scanLeft, Color.blue);
				transform.forward = Vector3.RotateTowards(transform.forward, scanLeft, Time.deltaTime*3, 0f);
				//Debug.Log("scanning left");
				yield return null;
			}
			while (Vector3.Dot(transform.forward, scanRight) < 0.95) {
				//Debug.DrawRay(transform.position, scanRight, Color.red);
				transform.forward = Vector3.RotateTowards(transform.forward, scanRight, Time.deltaTime*3, 0f);
				//Debug.Log("scanning right");
				yield return null;
			}
			yield return new WaitForSeconds(1f);
			areaScanned = true;
			//Debug.Log(areaScanned);
		}

		while (searches < searchLoops) {
			agent.destination = FindNewRandomNavMeshPoint(targetLocation + (CalculateTargetVelocity(lastKnownPosition1, lastKnownPosition2).normalized * searchRange/2), searchRange);
			while (agent.pathPending) {
				yield return null;
			}
			while (agent.remainingDistance > 0.5f) {
				yield return null;
			}
			//Debug.Log("set scanned = false");
			areaScanned = false;
			while (!areaScanned) {
				//Debug.Log("scanning");
				Vector3 scanLeft = (transform.position + (transform.right * -1)) - transform.position;
				Vector3 scanRight = (transform.position + transform.right) - transform.position;
				while (Vector3.Dot(transform.forward, scanLeft) < 0.95f) {
					//Debug.DrawRay(transform.position, scanLeft, Color.blue);
					transform.forward = Vector3.RotateTowards(transform.forward, scanLeft, Time.deltaTime*3, 0f);
					//Debug.Log("scanning left");
					yield return null;
				}
				while (Vector3.Dot(transform.forward, scanRight) < 0.95f) {
					//Debug.DrawRay(transform.position, scanRight, Color.red);
					transform.forward = Vector3.RotateTowards(transform.forward, scanRight, Time.deltaTime*3, 0f);
					//Debug.Log("scanning right");
					yield return null;
				}
				yield return new WaitForSeconds(1f);
				areaScanned = true;
				//Debug.Log(areaScanned);

			}
			yield return new WaitForSeconds(1f);
			searches++;
			//Debug.Log(searches);
		}
		//Debug.Log("searching complete");
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

	private Vector3 CalculateTargetVelocity(Vector3 v1, Vector3 v2) {
		Vector3 velo = v2 - v1;
		return velo;
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
		StartCoroutine("Attack");
	}

	public override void StopAttackBehaviour() {
		StopCoroutine("Attack");
	}

	public override void StartSearchBehaviour() {
		agent.ResetPath();
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
}
