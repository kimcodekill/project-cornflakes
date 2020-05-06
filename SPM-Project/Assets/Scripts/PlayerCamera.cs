using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

	[SerializeField] [Tooltip("The camera's possible states")] private State[] states;

	[Header("Camera attributes")]
	[SerializeField] [Tooltip("Layers the camera collides with.")] private LayerMask collisionLayer;
	[SerializeField] private LayerMask playerLayer;
	[SerializeField] [Tooltip("The speed with which the camera moves.")] private float lookSensitivity = 1f;
	[SerializeField] [Tooltip("Camera radius for collision detection.")] private float camRadius = 0.25f; 
	[SerializeField] [Tooltip("Minimum allowed distance between camera and objects behind it.")] private float minCollisionDistance;
	[SerializeField] [Tooltip("The Player transform the camera attaches to.")] private Transform playerMesh;
	private PlayerRenderer pr;

	// Since the StateMachine is responsible for cameraOffset now, 
	// the value gets set through CameraState assetmenu instances
	private Vector3 cameraOffset;
	private float rotationX, rotationY;


	private StateMachine stateMachine;

	/// <summary>
	/// The actual camera attached to the PlayerCamera.
	/// </summary>
	public Camera Camera { get; private set; }

	private void Start() {
		Camera = GetComponent<Camera>();
		DebugManager.AddSection("CameraState", "");
		pr = GetComponent<PlayerRenderer>();
		stateMachine = new StateMachine(this, states);
	}

	private void Update()
	{
		stateMachine.Run();
	}

	// I don't fully understand why we're doing camera updates in LateUpdate, 
	// but the statemachine will run in Update
	private void LateUpdate() {
		RotateCamera();
		transform.position = playerMesh.position + GetAdjustedCameraPosition(transform.rotation * cameraOffset);
		Debug.DrawRay(transform.position, transform.forward * 10);
	}

	private Vector3 GetAdjustedCameraPosition(Vector3 relationVector) {
		pr.SetTransparency(1.0f);
		if (Physics.SphereCast(playerMesh.position, camRadius, relationVector.normalized, out RaycastHit hit, relationVector.magnitude + camRadius, collisionLayer)) {
			if (hit.distance > minCollisionDistance) {
				if (IsPlayerInFrontOfCamera()) {
					pr.SetTransparency(0.0f);
				}
				return relationVector.normalized * (hit.distance - camRadius);
			}
			else {
				pr.SetTransparency(0.0f);
				return Vector3.up * 2.25f;
			}
		}
		else return relationVector;
	}

	private bool IsPlayerInFrontOfCamera() {
		Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, cameraOffset.magnitude, playerLayer);
		if (hit.collider) return true;
		else return false;
	}
	private void RotateCamera() {
		rotationY += lookSensitivity * Input.GetAxis("Mouse X");
		rotationX -= lookSensitivity * Input.GetAxis("Mouse Y");
		rotationX = Mathf.Clamp(rotationX, -60f, 60f);
		transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
	}

	/// <summary>
	/// Adds rotation values to the camera that won't be overwritten by the internal rotation script.
	/// </summary>
	/// <param name="rotationX">The desired rotation around the X axis.</param>
	/// <param name="rotationY">The desired rotation around the Y axis.</param>
	public void InjectRotation(float rotationX, float rotationY) {
		this.rotationY += rotationY;
		this.rotationX -= rotationX;
		this.rotationX = Mathf.Clamp(this.rotationX, -60f, 60f);
		transform.rotation = Quaternion.Euler(this.rotationX, this.rotationY, 0);
	}	

	public void SetFOV(int fov)
	{
		Camera.fieldOfView = fov;
	}

	public void SetOffset(Vector3 offset)
	{
		cameraOffset = offset;
	}

	public void SetSensitivity(float sensitivty) {
		lookSensitivity = sensitivty;
	}
}