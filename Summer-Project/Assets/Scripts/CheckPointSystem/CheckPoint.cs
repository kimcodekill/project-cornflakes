using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class CheckPoint : MonoBehaviour, ICapturable {

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) CaptureMoment();
	}

	private void CaptureMoment() {
		gameObject.SetActive(false);
		CaptureKeeper.NewLevel = false;
		CaptureKeeper.LevelHasBeenCaptured = true;
		CaptureKeeper.CreateCapture(this);
	}

	public void OnLoad(bool wasEnabled) { }

	public object GetPersistentCaptureID() {
		return transform.position;
	}
}