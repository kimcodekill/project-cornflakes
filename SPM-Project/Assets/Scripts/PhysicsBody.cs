﻿using System;
using UnityEngine;

//Author: Viktor Dahlberg
public class PhysicsBody : MonoBehaviour {

	#region Properties

	/// <summary>
	/// The position of the left foot of the PhysicsBody relative to the gameObject transform.
	/// </summary>
	public Vector3 LeftFoot { get; set; } = new Vector3(-0.5f, 0, 0);
	
	/// <summary>
	/// The position of the left foot of the PhysicsBody relative to the gameObject transform.
	/// </summary>
	public Vector3 RightFoot { get; set; } = new Vector3(0.5f, 0, 0);

	/// <summary>
	/// The extra margin of collider distance to account for any rigidbody imperfections or inaccuracies.
	/// </summary>
	public float GroundedDistanceOffset { get; set; } = 0.1f;

	/// <summary>
	/// The maximum dot product allowed for <c>Vector3.Dot(Vector3.down, GetCurrentSurfaceNormal())</c>,
	/// to determine whether or not the player should count as "grounded".
	/// </summary>
	public float DotProductTreshold { get; set; } = -0.5f;

	#endregion

	[SerializeField] private LayerMask mask;

	private Collider collider;

	private Rigidbody rigidBody;

	private void Awake() {
		collider = GetComponent<Collider>();
		rigidBody = gameObject.AddComponent<Rigidbody>();
		rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
	}

	/// <summary>
	/// Sets the drag of the PhysicsBody. 0 is no drag, 10 is usually enough to prevent sliding.
	/// </summary>
	/// <param name="drag">The amount of drag that will affect the PhysicsBody.</param>
	public void SetSlideRate(float drag) {
		rigidBody.drag = drag;
	}

	/// <summary>
	/// Enables or disables the application of gravity to the PhysicsBody.
	/// </summary>
	/// <param name="enabled">Whether or not gravity should be enabled.</param>
	public void SetGravityEnabled(bool enabled) {
		rigidBody.useGravity = enabled;
	}

	/// <summary>
	/// Returns the current velocity of the PhysicsBody.
	/// </summary>
	/// <returns>The velocity as a Vector3.</returns>
	public Vector3 GetCurrentVelocity() {
		return rigidBody.velocity;
	}

	#region Adjust Velocity

	/// <summary>
	/// Adds a certain amount of force to the PhysicsBody.
	/// </summary>
	/// <param name="impulse">The desired impulse.</param>
	/// <param name="mode">The force mode to use.</param>
	public void AddForce(Vector3 impulse, ForceMode mode = ForceMode.Impulse) {
		rigidBody.AddForce(impulse, mode);
	}

	/// <summary>
	/// Sets the velocity and angular velocity of the PhysicsBody to <c>Vector3.zero</c>.
	/// </summary>
	public void ResetVelocity() {
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
	}

	/// <summary>
	/// Changes the direction of velocity.
	/// </summary>
	/// <param name="direction">The new direction the velocity will be heading.</param>
	public void ChangeVelocityDirection(Vector3 direction) {
		float magnitude = rigidBody.velocity.magnitude;
		float angularMagnitude = rigidBody.angularVelocity.magnitude;
		rigidBody.velocity = direction.normalized * magnitude;
		rigidBody.angularVelocity = direction.normalized * angularMagnitude;
	}

	#region Limit Velocity

	/// <summary>
	/// Prevents the PhysicsBody from exceeding the specified velocity.
	/// </summary>
	/// <param name="topSpeed">The speed limit.</param>
	public void CapVelocity(float topSpeed) {
		rigidBody.velocity = rigidBody.velocity.magnitude > topSpeed ? rigidBody.velocity.normalized * topSpeed : rigidBody.velocity;
		rigidBody.angularVelocity = rigidBody.angularVelocity.magnitude > topSpeed ? rigidBody.angularVelocity.normalized * topSpeed : rigidBody.angularVelocity;
	}

	/// <summary>
	/// Prevents the PhysicsBody from exceeding the specified horizontal velocity.
	/// </summary>
	/// <param name="topSpeed">The speed limit.</param>
	public void CapHorizontalVelocity(float topSpeed) {
		float velocityY = rigidBody.velocity.y;
		float angularVelocityY = rigidBody.angularVelocity.y;
		CapVelocity(topSpeed);
		rigidBody.velocity = new Vector3(rigidBody.velocity.x, velocityY, rigidBody.velocity.z);
		rigidBody.angularVelocity = new Vector3(rigidBody.angularVelocity.x, angularVelocityY, rigidBody.velocity.z);
	}

	#endregion

	#region Set Axis Velocity

	/// <summary>
	/// Sets the vertical velocity of the specified axis.
	/// </summary>
	/// <param name="axis">The axis to set.</param>
	/// <param name="velocity">The new velocity of the axis.</param>
	public void SetAxisVelocity(char axis, float velocity) {
		switch (axis) {
			case 'X':
			case 'x':
				SetXVelocity(velocity);
				break;
			case 'Y':
			case 'y':
				SetYVelocity(velocity);
				break;
			case 'Z':
			case 'z':
				SetZVelocity(velocity);
				break;
			default: throw new ArgumentException("Invalid axis");
		}
	}

	private void SetXVelocity(float velocity) {
		rigidBody.velocity = new Vector3(velocity, rigidBody.velocity.y, rigidBody.velocity.z);
		rigidBody.angularVelocity = new Vector3(velocity, rigidBody.velocity.y, rigidBody.velocity.z);
	}

	private void SetYVelocity(float velocity) {
		rigidBody.velocity = new Vector3(rigidBody.velocity.x, velocity, rigidBody.velocity.z);
		rigidBody.angularVelocity = new Vector3(rigidBody.angularVelocity.x, velocity, rigidBody.angularVelocity.z);
	}

	private void SetZVelocity(float velocity) {
		rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, velocity);
		rigidBody.angularVelocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, velocity);
	}

	#endregion

	#endregion

	#region Ground Check

	/// <summary>
	/// Determines whether or not the PhysicsBody is grounded, by checking if the <c>leftFoot</c> or <c>rightFoot</c> positions are in contact with the ground.
	/// </summary>
	/// <returns><c>true</c> if the PhysicsBody is grounded, <c>false</c> if it is not.</returns>
	public bool IsGrounded(bool useNewMethod = true) {
		if (useNewMethod) {
			CapsuleCollider c = (CapsuleCollider) collider;
			Vector3 topCircle = transform.position + c.center + Vector3.up * (c.height / 2 - c.radius);
			Vector3 bottomCircle = transform.position + c.center + Vector3.down * (c.height / 2 - c.radius) + (Vector3.up * GroundedDistanceOffset);
			Physics.CapsuleCast(topCircle, bottomCircle, c.radius, Vector3.down, out RaycastHit hit, GroundedDistanceOffset * 2f, mask);
			return hit.collider && Vector3.Dot(Vector3.down, hit.normal) < -0.5f;
		}
		else return Physics.Raycast(GetPositionWithOffset(LeftFoot), Vector3.down, collider.bounds.extents.y + GroundedDistanceOffset, mask) || Physics.Raycast(GetPositionWithOffset(RightFoot), Vector3.down, collider.bounds.extents.y + GroundedDistanceOffset, mask);
	}

	/// <summary>
	/// Sets feet to collider width so that the player is grounded as long as his side is < 100% off the ground.
	/// </summary>
	public void SetFeetToColliderWidth() {
		LeftFoot = new Vector3(-collider.bounds.extents.x, 0, 0);
		RightFoot = new Vector3(collider.bounds.extents.x, 0, 0);
	}

	/// <summary>
	/// Returns the normal of the current surface the player is standing on.
	/// </summary>
	/// <returns>The normal of the hit surface.</returns>
	public Vector3 GetCurrentSurfaceNormal() {
		CapsuleCollider c = (CapsuleCollider) collider;
		Vector3 topCircle = transform.position + c.center + Vector3.up * (c.height / 2 - c.radius) + (Vector3.up * GroundedDistanceOffset);
		Vector3 bottomCircle = transform.position + c.center + Vector3.down * (c.height / 2 - c.radius) + (Vector3.up * GroundedDistanceOffset);
		Physics.CapsuleCast(topCircle, bottomCircle, c.radius, Vector3.down, out RaycastHit hit, GroundedDistanceOffset * 2f, mask);
		Debug.DrawRay(hit.point, hit.normal, Color.red);
		return hit.collider ? hit.normal : Vector3.up;
	}

	private Vector3 GetPositionWithOffset(Vector3 offset) {
		return transform.position + (transform.right * offset.x) + (transform.up * offset.y) + (transform.forward * offset.z);
	}

	#endregion

}