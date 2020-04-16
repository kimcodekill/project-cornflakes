using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrone : Enemy {

	[SerializeField] private float patrolBungeeDistance;
	[SerializeField] private float movementSpeed;
	private Vector3 origin;
	//[SerializeField] private LayerMask droneAvoid;
	[SerializeField] private SphereCollider body;

	private void Awake() {
		origin = transform.position;
	}
	private void Start() {
		base.Start();
	}

	private void Update() {
		base.Update();
		if(PlayerIsInSight()) { transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (Target.transform.position - transform.position), Time.deltaTime * 5f, 0f)); }
	}

	//private float DistanceToGround() {
	//	Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, float.MaxValue, layerMask);
	//	return hit.distance;
	//}

	private Vector3 FindRandomPosition() {
		Vector3 randomPos = origin + new Vector3(Random.Range(-patrolBungeeDistance, patrolBungeeDistance), Random.Range(-patrolBungeeDistance/5, patrolBungeeDistance/5), Random.Range(-patrolBungeeDistance, patrolBungeeDistance));
		RaycastHit[] hits = Physics.SphereCastAll(transform.position, body.radius, (randomPos - transform.position).normalized, (randomPos - transform.position).magnitude + 1f, layerMask);
		for (int i = 0; i < hits.Length; i++) {
			if (hits[i].collider != null) {
				//Debug.Log("Hit " + hits[i].collider.gameObject.name);
				return FindRandomPosition();
			}
		}
		return randomPos;
	}

	private void FollowTarget() {
		
	}

	private IEnumerator Idle() {
		Vector3 newPos = FindRandomPosition();
		//Debug.Log(newPos);
		while (Vector3.Distance(transform.position, newPos) > 0.05f) {
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, (newPos - transform.position), Time.deltaTime * 3f, 0f));
			transform.position = Vector3.MoveTowards(transform.position, newPos, Time.deltaTime * movementSpeed);
			yield return null;
		}

		yield return null;
		StartCoroutine("Idle");
	}




	public override void StartIdleBehaviour() {
		StartCoroutine("Idle");
	}

	public override void StopIdleBehaviour() {
		StopCoroutine("Idle");
	}
}
