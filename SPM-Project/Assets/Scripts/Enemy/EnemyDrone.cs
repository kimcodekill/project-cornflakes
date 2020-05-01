using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : Enemy {

	[SerializeField] private float patrolBungeeDistance;
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

	private Vector3 FindRandomPosition(Vector3 startingPos, float moveRange) {
		Vector3 randomPos = startingPos + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange/3, moveRange/3), Random.Range(-moveRange, moveRange));
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, body.radius, (randomPos - transform.position).normalized, (randomPos - transform.position).magnitude + 1f, ObscuringLayers);
		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].collider != null) {
				//Debug.Log("Hit " + hits[i].collider.gameObject.name);
				return FindRandomPosition(startingPos + hits[i].normal, moveRange);
			}
		}
		return randomPos;
	}

	private IEnumerator Patrol() {
		ResetWeapon();
		Vector3 newPos = FindRandomPosition(origin, patrolBungeeDistance);
		while (Vector3.Distance(transform.position, newPos) > 0.05f) {
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (newPos - transform.position), Time.deltaTime * 3f, 0f));
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * movementSpeed);
			yield return null;
		}

		yield return null;
		StartCoroutine("Patrol");
	}

	private IEnumerator Attack() {
		/*if ((Vector3.Distance(transform.position, Target.transform.position) > 2.0f)) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * movementSpeed);
		}*/
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, vectorToPlayer, Time.deltaTime * 5f, 0f));
		yield return null;
		StartCoroutine("Attack");
	}

	private IEnumerator Alerted() {
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, vectorToPlayer, Time.deltaTime * 5f, 0f));
		if ((Vector3.Distance(transform.position, Target.transform.position) > attackRange)) {
			transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * movementSpeed);
		}
		yield return null;
		StartCoroutine("Alerted");
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
