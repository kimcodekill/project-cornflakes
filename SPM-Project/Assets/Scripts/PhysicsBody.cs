using UnityEngine;

public class PhysicsBody : MonoBehaviour {

	private Collider collider;

	private Rigidbody rigidBody;
	
	[SerializeField] private LayerMask mask;

	/// <summary>
	/// The position of the left foot of the PhysicsBody relative to the gameObject transform.
	/// </summary>
	public Vector3 LeftFoot { get; set; } = new Vector3(-0.5f, 0, 0);
	
	/// <summary>
	/// The position of the left foot of the PhysicsBody relative to the gameObject transform.
	/// </summary>
	public Vector3 RightFoot { get; set; } = new Vector3(0.5f, 0, 0);
	
	/// <summary>
	/// The distance to which to determine if the PhysicsObject is grounded or not.
	/// Initialized to the extents of the collider once it has been retrieved.
	/// </summary>
	public float GroundedDistance { get => groundedDistance + GroundedDistanceOffset; set => groundedDistance = value; }
	private float groundedDistance;

	/// <summary>
	/// The extra margin of collider distance to account for any rigidbody imperfections or inaccuracies.
	/// </summary>
	public float GroundedDistanceOffset { get; set; } = 0.01f;

	private void Start() {
		collider = GetComponent<Collider>();
		GroundedDistance = collider.bounds.extents.y;

		rigidBody = gameObject.AddComponent<Rigidbody>();
		rigidBody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

	/// <summary>
	/// Adds a certain amount of force to the PhysicsBody.
	/// </summary>
	/// <param name="impulse">The desired impulse.</param>
	/// <param name="mode">The force mode to use.</param>
	public void AddForce(Vector3 impulse, ForceMode mode = ForceMode.Impulse) {
		rigidBody.AddForce(impulse, mode);
	}

	/// <summary>
	/// Determines whether or not the PhysicsBody is grounded, by checking if the <c>leftFoot</c> or <c>rightFoot</c> positions are in contact with the ground.
	/// </summary>
	/// <returns><c>true</c> if the PhysicsBody is grounded, <c>false</c> if it is not.</returns>
	public bool IsGrounded() {
		return Physics.Raycast(GetPositionWithOffset(LeftFoot), Vector3.down, GroundedDistance, mask) || Physics.Raycast(GetPositionWithOffset(RightFoot), Vector3.down, GroundedDistance, mask);
	}

	/// <summary>
	/// Sets the velocity and angular velocity of the PhysicsBody to <c>Vector3.0</c>.
	/// </summary>
	public void ResetVelocity() {
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
	}

	/// <summary>
	/// Prevents the PhysicsBody from exceeding the specified velocity.
	/// </summary>
	/// <param name="topSpeed">The speed limit.</param>
	public void CapVelocity(float topSpeed) {
		rigidBody.velocity = rigidBody.velocity.magnitude > topSpeed ? rigidBody.velocity.normalized * topSpeed : rigidBody.velocity;
		rigidBody.angularVelocity = rigidBody.angularVelocity.magnitude > topSpeed ? rigidBody.angularVelocity.normalized * topSpeed : rigidBody.angularVelocity;
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
		Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, GroundedDistance, mask);
		return hit.normal;
	}

	private Vector3 GetPositionWithOffset(Vector3 offset) {
		return transform.position + (transform.right * offset.x) + (transform.up * offset.y) + (transform.forward * offset.z);
	}

}