using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {
	float rotationX, rotationY;
	float camRadius = 0.25f; 
	[SerializeField] float lookSensitivity = 1f;
	[SerializeField] float minCameraDistance = 2f;
	[SerializeField] LayerMask collisionLayer;
	[SerializeField] Transform player;
	[SerializeField] Vector3 cameraOffset;

	public Quaternion GetRotation() { return transform.rotation; }

	void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		
	}

	void Update() {
		RotateCamera();
		transform.position = player.position + GetAdjustedCameraPosition(transform.rotation * cameraOffset);
		Debug.DrawRay(transform.position, transform.forward * 10);
	}

	Vector3 GetAdjustedCameraPosition(Vector3 relationVector) {
		if (Physics.SphereCast(player.position, camRadius, relationVector.normalized, out RaycastHit hit, relationVector.magnitude + camRadius, collisionLayer)) {
			if (hit.distance > minCameraDistance)
				return relationVector.normalized * (hit.distance - camRadius);
			else return Vector3.zero;
		}
		else return relationVector;
	}

	void RotateCamera() {
		rotationY += lookSensitivity * Input.GetAxis("Mouse X");
		rotationX -= lookSensitivity * Input.GetAxis("Mouse Y");
		rotationX = Mathf.Clamp(rotationX, -60f, 60f);
		transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
	}
}

