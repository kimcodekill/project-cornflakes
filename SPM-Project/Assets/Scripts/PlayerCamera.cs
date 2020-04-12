using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	[Header("Camera attributes")]
	[SerializeField] [Tooltip("Defines offset to player.")] private Vector3 cameraOffset;
	[SerializeField] [Tooltip("Layers the camera collides with.")] private LayerMask collisionLayer;
	[SerializeField] [Tooltip("The speed with which the camera moves.")] private float lookSensitivity = 1f;
	[SerializeField] [Tooltip("Cameram radius for collision detection.")] private float camRadius = 0.25f; 
	[SerializeField] [Tooltip("Minimum allowed distance between camera and objects behind it.")] private float minCollisionDistance = 2f;
	private float rotationX, rotationY;
	private Camera camera;

	[SerializeField] [Tooltip("The Player object the camera attaches to.")] private Transform player;

	/// <summary>
	/// Returns the camera's current rotation.
	/// </summary>
	/// <returns></returns>
	public Quaternion GetRotation() { return transform.rotation; }

	private void Start() {
		Cursor.lockState = CursorLockMode.Locked;
		camera = GetComponent<Camera>();
	}

	private void Update() {
		RotateCamera();
		transform.position = player.position + GetAdjustedCameraPosition(transform.rotation * cameraOffset);
		GetActualLook();
	}

	private Vector3 GetAdjustedCameraPosition(Vector3 relationVector) {
		if (Physics.SphereCast(player.position, camRadius, relationVector.normalized, out RaycastHit hit, relationVector.magnitude + camRadius, collisionLayer)) {
			if (hit.distance > minCollisionDistance)
				return relationVector.normalized * (hit.distance - camRadius);
			else return Vector3.zero;
		}
		else return relationVector;
	}

	private void RotateCamera() {
		rotationY += lookSensitivity * Input.GetAxis("Mouse X");
		rotationX -= lookSensitivity * Input.GetAxis("Mouse Y");
		rotationX = Mathf.Clamp(rotationX, -60f, 60f);
		transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
	}
	
	private void GetActualLook() {
		Ray r = camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		Physics.Raycast(r, out RaycastHit hit, float.MaxValue);
		Debug.DrawRay(r.origin, r.direction * hit.distance, Color.red);
	}

}