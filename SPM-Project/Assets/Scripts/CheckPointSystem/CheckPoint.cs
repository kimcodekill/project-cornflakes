using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			CaptureMoment();
		}
	}

	private void CaptureMoment() {
		gameObject.SetActive(false);
		CaptureKeeper.CreateCapture(transform.position, transform.rotation);
	}

}