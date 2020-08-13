﻿using System;
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

	private PlayerRenderer pr;
	private Transform player;

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

	private void OnEnable()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start() {
		Camera = GetComponent<Camera>();
		DebugManager.AddSection("CameraState", "");
		pr = PlayerController.Instance.gameObject.GetComponent<PlayerRenderer>();
		player = PlayerController.Instance.transform;
		stateMachine = new StateMachine(this, states);
		lookSensitivity = PlayerPrefs.GetFloat("mouseSensitivity");
	}

	private void Update()
	{
		if (PauseMenu.GameRunning)
		{
			stateMachine.Run();
			AccumulateRotation();

			RotateCamera();
			transform.position = player.transform.position + GetAdjustedCameraPosition(transform.rotation * cameraOffset);
			Debug.DrawRay(transform.position, transform.forward * 10);
		}
	}

	private Vector3 GetAdjustedCameraPosition(Vector3 relationVector) {
		pr.SetRenderMode(RenderMode.Opaque);
		if (Physics.SphereCast(player.position, camRadius, relationVector.normalized, out RaycastHit hit, relationVector.magnitude + camRadius, collisionLayer)) {
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
		//Debug.Log("Camera: " + transform.rotation.eulerAngles.y);
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

	public Vector3 CameraLookPoint(LayerMask hitMask)
    {
		Vector2 screenPoint = new Vector2(Camera.pixelWidth / 2.0f, Camera.pixelHeight / 2.0f);
		Ray cameraRay = Camera.ScreenPointToRay(screenPoint);

		Physics.Raycast(cameraRay, out RaycastHit cameraHit, float.MaxValue, hitMask);

		return cameraHit.point;
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			Instance = null;
		}
	}
}