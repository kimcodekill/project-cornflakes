using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZone : MonoBehaviour {

	[SerializeField] private float rotationX;
	[SerializeField] private float rotationY;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponentInChildren<PlayerCamera>().InjectSetRotation(rotationX, rotationY);
			gameObject.SetActive(false);
		}
	}

}