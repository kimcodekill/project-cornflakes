using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZone : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			other.gameObject.GetComponentInChildren<PlayerCamera>().InjectSetRotation(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y);
			gameObject.SetActive(false);
		}
	}

}