using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour {

	private bool triggered;

	private void OnTriggerEnter(Collider other) {
		if (!triggered) CaptureMoment();
	}

	private void CaptureMoment() {
		
	}

}