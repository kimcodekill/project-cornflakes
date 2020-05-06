using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailer : MonoBehaviour {

	private TrailRenderer tr;

	private float lastYPosition;

	private void Start() {
		tr = GetComponent<TrailRenderer>();
	}

	private void Update() {
		float currentYVelocity = PlayerController.Instance.PhysicsBody.GetCurrentVelocity().y;
		float currentYPosition = PlayerController.Instance.transform.position.y;
		if (currentYVelocity > 0 && currentYPosition > lastYPosition) tr.time = Mathf.MoveTowards(0f, 5f, Time.deltaTime * 200);
		else tr.time = Mathf.MoveTowards(tr.time, 0f, Time.deltaTime * 10);
		lastYPosition = currentYPosition;
	}

}