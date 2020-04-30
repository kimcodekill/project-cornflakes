using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	public bool Triggered { get; set; }

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (!Triggered) CaptureMoment();
			else Debug.Log("This checkpoint has already been triggered");
		}
	}

	private void CaptureMoment() {
		CaptureKeeper.CreateCapture(transform.position, transform.rotation);
		Triggered = true;
	}

}