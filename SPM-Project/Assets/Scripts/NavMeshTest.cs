using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
	public Camera cam;
	private NavMeshAgent agent;
	public NavMeshSurface surface;


	private void Start() {
		//surface.BuildNavMesh();
		agent = gameObject.GetComponent<NavMeshAgent>();
	}
	// Update is called once per frame
	void Update()
    {
		
		if (Input.GetMouseButtonDown(0)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			Physics.Raycast(ray, out RaycastHit hit);
			agent.SetDestination(hit.point);
		}

    }
}
