using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDrone : Enemy {

	[SerializeField] private float patrolBungeeDistance;
	[SerializeField] private float movementSpeed;
	[SerializeField] private SphereCollider body;
	private NavMeshAgent agent;
	private float bodyRadius = 0.5f;

	private Vector3 origin;

	private void Awake() {
		agent = GetComponent<NavMeshAgent>();
		origin = transform.position;
	}
	private void Start() {
		base.Start();
	}

	private void Update() {
		base.Update();
		
	}

	//private float DistanceToGround() {
	//	Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, float.MaxValue, layerMask);
	//	return hit.distance;
	//}

	//private Vector3 FindRandomPosition(Vector3 startingPos, float moveRange) {
	//	Vector3 randomPos = startingPos + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange/3, moveRange/3), Random.Range(-moveRange, moveRange));
	//	RaycastHit[] hits = Physics.SphereCastAll(transform.position, body.radius, (randomPos - transform.position).normalized, (randomPos - transform.position).magnitude + 1f, layerMask);
	//	for (int i = 0; i < hits.Length; i++) {
	//		if (hits[i].collider != null) {
	//			//Debug.Log("Hit " + hits[i].collider.gameObject.name);
	//			return FindRandomPosition(startingPos + hits[i].normal, moveRange);
	//		}
	//	}
	//	return randomPos;
	//}

	private IEnumerator Patrol() {
		agent.destination = FindNewRandomNavPoint(origin, patrolBungeeDistance);
		while (agent.remainingDistance > 0.5f) {
			yield return null;
		}
		yield return null;
		StartCoroutine("Patrol");
	}

	private IEnumerator Attack() {
		if ((Vector3.Distance(transform.position, Target.transform.position) > 2.0f)) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * movementSpeed);
		}
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, VectorToTarget, Time.deltaTime * 5f, 0f));
		yield return null;
		StartCoroutine("Attack");
	}

	private IEnumerator Alerted() {
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, VectorToTarget, Time.deltaTime * 5f, 0f));
		if ((Vector3.Distance(transform.position, Target.transform.position) > attackRange)) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * movementSpeed);
		}
		yield return null;
		StartCoroutine("Alerted");
	}

	private Vector3 FindNewRandomNavPoint(Vector3 startPos, float patrolRange) {
		Vector3 newPoint = Random.insideUnitSphere * patrolRange;
		newPoint = startPos + newPoint;
		//Debug.Log(newPoint);
		Vector3 finalPosition = startPos + Vector3.zero;
		if (NavMesh.SamplePosition(newPoint, out NavMeshHit hit, patrolRange + agent.height, 1)) {
			//Debug.Log(hit.position);
			finalPosition = hit.position;
		}
		return finalPosition;
	}

	public override void StartPatrolBehaviour() {
		StartCoroutine("Patrol");
	}

	public override void StopPatrolBehaviour() {
		
		StopCoroutine("Patrol");
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
}
