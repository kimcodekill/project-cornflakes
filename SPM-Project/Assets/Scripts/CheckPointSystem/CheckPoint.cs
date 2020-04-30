using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	/// <summary>
	/// If the checkpoint has been triggered.
	/// </summary>
	public bool Triggered { get; set; }

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			if (!Triggered) CaptureMoment();
			else Debug.Log("This checkpoint has already been triggered");
		}
	}

	private void CaptureMoment() {
		Triggered = true;
		Debug.Log("Triggered checkpoint");
		CaptureKeeper.CreateCapture(transform.position, transform.rotation);
	}

}