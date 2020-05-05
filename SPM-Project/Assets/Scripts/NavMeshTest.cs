using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//Author: Erik Pilström
//This script is no longer relevant for the project and can be deleted.
public class NavMeshTest : MonoBehaviour
{
	public Camera cam;
	private NavMeshAgent agent;
	public NavMeshSurface surface;


	private void Start() {
		//surface.BuildNavMesh();
		agent = gameObject.GetComponent<NavMeshAgent>();
	}

	void Update()
    {
		
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(ray, out RaycastHit hit);
			agent.SetDestination(hit.point);
		}

    }
}
