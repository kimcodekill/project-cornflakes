using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldier : Enemy
{
	[SerializeField] [Tooltip("The patrol points. NEVER USE ONLY ONE.")] private Transform[] points;
	//Seriously, never have only one. If you want a non-patrolling soldier, untick the checkbox for Is Patrolling.

	private NavMeshAgent agent;
	private int destPoint = 0;
	public bool isPatrolling;
	private Vector3 origin;

	private void Awake() {
		origin = transform.position;
		agent = GetComponent<NavMeshAgent>();
		
	}

	private void Start() {
		base.Start();
		
	}

	private IEnumerator Patrol() {
		eyeTransform.forward = transform.forward;
		while (!agent.pathPending && agent.remainingDistance < 0.1f) {
			GoToNextPoint();
		}
		yield return null;
		StartCoroutine("Patrol");
	}

	private IEnumerator Alerted() {
		//Debug.Log("cr alerted");
		agent.destination = Target.transform.position;
		while (!agent.pathPending && agent.remainingDistance > attackRange) {
			yield return null;
		}
		yield return null;
		StartCoroutine("Alerted");
	}

	private IEnumerator Attack() {
		if ((Vector3.Distance(transform.position, Target.transform.position) > attackRange / 2)) {
			while ((Vector3.Distance(transform.position, Target.transform.position) > attackRange / 2)) {
				agent.destination = Target.transform.position;
				yield return null;
			}
		}
		agent.ResetPath();
		while(!agent.hasPath) {
			if ((Vector3.Distance(transform.position, Target.transform.position) > attackRange / 2)) {
				while ((Vector3.Distance(transform.position, Target.transform.position) > attackRange / 2)) {
					agent.destination = Target.transform.position;
					yield return null;
				}
				agent.ResetPath();
			}
			transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, VectorToTarget, Time.deltaTime * 5f, 0f));
			eyeTransform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(eyeTransform.forward, VectorToTarget, Time.deltaTime * 70f, 0f));
			yield return null;
		}
		yield return null;
		StartCoroutine("Attack");
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

	public override void StartPatrolBehaviour() {
		if (isPatrolling) {
			StartCoroutine("Patrol");
		}
		else GoToGuardPoint();
	}

	public override void StopPatrolBehaviour() {
		agent.ResetPath();
		StopCoroutine("Patrol");
	}

	public override void StartAlertedBehaviour() {
		StartCoroutine("Alerted");
	}

	public override void StopAlertedBehaviour() {
		StopCoroutine("Alerted");
	}

	public override void StartAttackBehaviour() {
		StartCoroutine("Attack");
	}

	public override void StopAttackBehaviour() {
		//agent.ResetPath();
		StopCoroutine("Attack");
	}
}
