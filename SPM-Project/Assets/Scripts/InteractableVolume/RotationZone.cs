using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationZone : MonoBehaviour {

	private void OnTriggerEnter(Collider other) {
		//K: had to do the thing after "&&" so the player wasn't rotated at checkpoints.
		//   think it is the player going to spawn after dying and rotationZone setting
		//   values after Checkpoint does.
		if (other.gameObject.CompareTag("Player") && !CaptureKeeper.LevelHasBeenCaptured) {
			PlayerCamera.Instance.InjectSetRotation(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y);
			gameObject.SetActive(false);
		}
	}

}