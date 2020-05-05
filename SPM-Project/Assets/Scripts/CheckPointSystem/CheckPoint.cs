using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour, ICapturable {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) CaptureMoment();
	}

	private void CaptureMoment() {
		gameObject.SetActive(false);
		CaptureKeeper.CreateCapture(this);
	}

	public bool InstanceIsCapturable() {
		return true;
	}

	public object GetPersistentCaptureID() {
		return transform.position;
	}
}