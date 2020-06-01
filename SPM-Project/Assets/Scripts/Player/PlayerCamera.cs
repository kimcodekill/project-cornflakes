using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class PlayerCamera : MonoBehaviour {

	public static PlayerCamera Instance;

	[SerializeField] [Tooltip("The camera's possible states")] private State[] states;

	[Header("Camera attributes")]
	[SerializeField] [Tooltip("Layers the camera collides with.")] private LayerMask collisionLayer;
	[SerializeField] [Tooltip ("The layer that the player belongs to.")] private LayerMask playerLayer;
	[SerializeField] [Tooltip("The speed with which the camera moves.")] private float lookSensitivity = 1f;
	[SerializeField] [Tooltip("Camera radius for collision detection.")] private float camRadius = 0.25f; 
	[SerializeField] [Tooltip("Minimum allowed distance between camera and objects behind it.")] private float minCollisionDistance;
	[SerializeField] [Tooltip("The Player transform the camera attaches to.")] private Transform playerMesh;
	private PlayerRenderer pr;

	// Since the StateMachine is responsible for cameraOffset now, 
	// the value gets set through CameraState assetmenu instances /K
	/// <summary>
	/// Cameras offset from the player, determining camera position. /E
	/// </summary>
	private Vector3 cameraOffset;

	/// <summary>
	/// Rotation values for camera around the X and Y axes, determines where the player is "looking". /E
	/// </summary>
	private float rotationX, rotationY;
	/// <summary>
	/// The camera's StateMachine instance (hereafter STM).
	/// </summary>
	private StateMachine stateMachine;

	[HideInInspector] public int reverbZoned;

	/// <summary>
	/// The actual camera attached to the PlayerCamera game object in scene view. /E
	/// </summary>
	public Camera Camera { get; private set; }

	private void Start() {
		if (Instance == null)
			Instance = this;

		Camera = GetComponent<Camera>();
		DebugManager.AddSection("CameraState", "");
		pr = GetComponent<PlayerRenderer>();
		stateMachine = new StateMachine(this, states);
	}

	private void Update()
	{
		stateMachine.Run();
		AccumulateRotation();
	}

	//Camera updates rotation in LateUpdate so that we don't try to change the rotation while the player is moving in a previous direction during the same frame.
	//This can otherwise lead to wonky movement effects. /E
	private void LateUpdate() {
		RotateCamera();
		transform.position = playerMesh.position + GetAdjustedCameraPosition(transform.rotation * cameraOffset);
		Debug.DrawRay(transform.position, transform.forward * 10);
	}

	private Vector3 GetAdjustedCameraPosition(Vector3 relationVector) {
		pr.SetRenderMode(RenderMode.Opaque);
		if (Physics.SphereCast(playerMesh.position, camRadius, relationVector.normalized, out RaycastHit hit, relationVector.magnitude + camRadius, collisionLayer)) {
			if (hit.distance > minCollisionDistance) {
				if (IsPlayerInFrontOfCamera()) {
					pr.SetRenderMode(RenderMode.Transparent);
				}
				return relationVector.normalized * (hit.distance - camRadius);
			}
			else {
				pr.SetRenderMode(RenderMode.Transparent);
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

	private void AccumulateRotation() {
		rotationY += lookSensitivity * Input.GetAxis("Mouse X");
		rotationX -= lookSensitivity * Input.GetAxis("Mouse Y");
		rotationX = Mathf.Clamp(rotationX, -60f, 60f);
	}

	private void RotateCamera() {
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

	/// <summary>
	/// Sets rotation values to the camera that won't be overwritten by the internal rotation script.
	/// </summary>
	/// <param name="rotationX">The desired rotation around the X axis.</param>
	/// <param name="rotationY">The desired rotation around the Y axis.</param>
	public void InjectSetRotation(float rotationX, float rotationY) {
		this.rotationY = rotationY;
		this.rotationX = rotationX;
		this.rotationX = Mathf.Clamp(this.rotationX, -60f, 60f);
		transform.rotation = Quaternion.Euler(this.rotationX, this.rotationY, 0);
	}

	/// <summary>
	/// Needs to be public to allow for setting camera FoV in the STM. /E
	/// </summary>
	/// <param name="fov">The cameras field of view value.</param>
	public void SetFOV(int fov) {
		Camera.fieldOfView = fov;
	}

	/// <summary>
	/// Needs to be public to allow for setting camera offset in the STM. /E
	/// </summary>
	/// <param name="offset">The camera's relative position to the player.</param>
	public void SetOffset(Vector3 offset)
	{
		cameraOffset = offset;
	}

	/// <summary>
	/// Needs to be public to allow for setting camera sensitivity in the STM. /E
	/// </summary>
	/// <param name="sensitivty">How fast the camera rotates.</param>
	public void SetSensitivity(float sensitivty) {
		lookSensitivity = sensitivty;
	}
}