using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : Enemy {

	[SerializeField] private float patrolBungeeDistance;
	[SerializeField] private float movementSpeed;
	[SerializeField] private SphereCollider body;

	private Vector3 origin;

	private void Awake() {
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

	private Vector3 FindRandomPosition(Vector3 startingPos, float moveRange) {
		Vector3 randomPos = startingPos + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange/3, moveRange/3), Random.Range(-moveRange, moveRange));
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, body.radius, (randomPos - transform.position).normalized, (randomPos - transform.position).magnitude + 1f, layerMask);
		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].collider != null) {
				//Debug.Log("Hit " + hits[i].collider.gameObject.name);
				return FindRandomPosition(startingPos + hits[i].normal, moveRange);
			}
		}
		return randomPos;
	}

	private Vector3 FindPositionAroundTarget(Vector3 targetPosition, float minDistance, float maxDistance) {
		float[] potentialXCoords = { Random.Range(targetPosition.x + minDistance, targetPosition.x + maxDistance), Random.Range(targetPosition.x - minDistance, targetPosition.x - maxDistance) };
		float[] potentialYCoords = { Random.Range(targetPosition.y + minDistance, targetPosition.y + maxDistance), Random.Range(targetPosition.y - minDistance, targetPosition.y - maxDistance) };
		float[] potentialZCoords = { Random.Range(targetPosition.z + minDistance, targetPosition.x + maxDistance), Random.Range(targetPosition.z - minDistance, targetPosition.x - maxDistance) };
		Vector3 pos = new Vector3(potentialXCoords[Random.Range(0,2)], potentialYCoords[Random.Range(0,2)], potentialZCoords[Random.Range(0,2)]);
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, body.radius, (pos - transform.position).normalized, (pos - transform.position).magnitude + 1f, layerMask);
		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].collider != null) {
				return FindPositionAroundTarget(targetPosition + hits[i].normal, minDistance, maxDistance);
			}
		}
		return pos;
	}

	private IEnumerator Idle() {
		Vector3 newPos = FindRandomPosition(origin, patrolBungeeDistance);
		while (Vector3.Distance(transform.position, newPos) > 0.05f) {
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (newPos - transform.position), Time.deltaTime * 3f, 0f));
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * movementSpeed);
			yield return null;
		}

		yield return null;
		StartCoroutine("Idle");
	}

	private IEnumerator AttackBehaviour() {
		Vector3 newPos = FindPositionAroundTarget(Target.transform.position, 2f, 3f);
		while(Vector3.Distance(transform.position, newPos) > 0.05f) {
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, VectorToTarget, Time.deltaTime * 5f, 0f));
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * movementSpeed*4f);
			yield return null;
		}

		yield return null;
		StartCoroutine("AttackBehaviour");
	}

	public override void StartIdleBehaviour() {
		//Debug.Log("Drone idling");
		StartCoroutine("Idle");
	}

	public override void StopIdleBehaviour() {
		//Debug.Log("Drone stopping idling");
		StopCoroutine("Idle");
	}

	public override void StartAttackBehaviour() {
		//Debug.Log("Drone attacking");
		StartCoroutine("AttackBehaviour");
	}

	public override void StopAttackBehaviour() {
		//Debug.Log("Drone stopping attacking");
		StopCoroutine("AttackBehaviour");
	}
}
