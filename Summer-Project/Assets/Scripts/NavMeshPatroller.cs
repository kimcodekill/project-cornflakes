﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
//This script is no longer relevant for the project and can be deleted.
public class NavMeshPatroller : MonoBehaviour
{

	private NavMeshAgent agent;
	public Transform[] points;
	private int destPoint = 0;
	public float patrolRange;
	private Vector3 origin;
	
    // Start is called before the first frame update
    void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		agent.autoBraking = false;
		origin = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
		if (!agent.pathPending && agent.remainingDistance < 0.1f)
			GoToNextPoint();
    }

	private void GoToNextPoint() {
		if (points.Length == 0)
			return;
		//agent.destination = points[destPoint].position;
		//destPoint = Random.Range(0, points.Length);
		agent.destination = FindNewRandomNavPoint(origin, patrolRange);
		
		
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
}
