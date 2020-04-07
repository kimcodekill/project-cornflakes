using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsBody : MonoBehaviour {

	private Collider collider;
	private Rigidbody rigidBody;
	[SerializeField] private LayerMask mask;

	/// <summary>
	/// The position of the left foot of the PhysicsBody relative to the gameObject transform.
	/// </summary>
	public Vector3 LeftFoot { get; set; } = new Vector3(0, 0, 0);
	
	/// <summary>
	/// The position of the left foot of the PhysicsBody relative to the gameObject transform.
	/// </summary>
	public Vector3 RightFoot { get; set; } = new Vector3(0, 0, 0);
	
	/// <summary>
	/// The distance to which to determine if the PhysicsObject is grounded or not.
	/// Initialized to the extents of the collider once it has been retrieved.
	/// </summary>
	public float GroundedDistance { get; set; }

	private void Start() {
		collider = GetComponent<Collider>();
		GroundedDistance = collider.bounds.extents.y;
		rigidBody = gameObject.AddComponent<Rigidbody>();
	}

	/// <summary>
	/// Determines whether or not the PhysicsBody is grounded.
	/// </summary>
	/// <returns><c>true</c> if the PhysicsBody is grounded, <c>false</c> if it is not.</returns>
	public bool IsGrounded() {
		return Physics.Raycast(GetPositionWithOffset(LeftFoot), Vector3.down, GroundedDistance, mask) || Physics.Raycast(GetPositionWithOffset(RightFoot), Vector3.down, GroundedDistance, mask);
	}

	private Vector3 GetPositionWithOffset(Vector3 offset) {
		return transform.position + (transform.right * offset.x) + (transform.up * offset.y) + (transform.forward * offset.z);
	}

}