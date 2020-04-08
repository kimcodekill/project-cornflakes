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

	private void Awake() {
		collider = GetComponent<Collider>();
		GroundedDistance = collider.bounds.extents.y;

		rigidBody = gameObject.AddComponent<Rigidbody>();
		rigidBody.constraints = RigidbodyConstraints.FreezeRotation;
    
		DebugManager.AddSection("Physics" + gameObject.GetInstanceID(), "", "", "", "");
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
	/// Sets the velocity and angular velocity of the PhysicsBody to <c>Vector3.0</c>.
	/// </summary>
	public void ResetVelocity() {
		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
	}

	/// <summary>
	/// Sets the Y component of the PhysicsBody velocity to 0.
	/// </summary>
	public void ResetVerticalSpeed() {
		DebugManager.UpdateRows("Physics" + gameObject.GetInstanceID(), new int[] { 0, 1 }, "rbvy1"+rigidBody.velocity.y, "rbavy1"+rigidBody.angularVelocity.y);
		rigidBody.velocity.Set(rigidBody.velocity.x, -1f, rigidBody.velocity.z);
		rigidBody.angularVelocity.Set(rigidBody.angularVelocity.x, -1f, rigidBody.angularVelocity.z);
		DebugManager.UpdateRows("Physics" + gameObject.GetInstanceID(), new int[] { 2, 3 }, "rbvy2" + rigidBody.velocity.y, "rbavy2" + rigidBody.angularVelocity.y);
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

	/// <summary>
	/// Determines whether or not the PhysicsBody is grounded, by checking if the <c>leftFoot</c> or <c>rightFoot</c> positions are in contact with the ground.
	/// </summary>
	/// <returns><c>true</c> if the PhysicsBody is grounded, <c>false</c> if it is not.</returns>
	public bool IsGrounded(bool useTempMethod = true) {
		if (useTempMethod) {
			CapsuleCollider c = (CapsuleCollider) collider;
			Vector3 topCircle = transform.position + c.center + Vector3.up * (c.height / 2 - c.radius);
			Vector3 bottomCircle = transform.position + c.center + Vector3.down * (c.height / 2 - c.radius);
			return Physics.CapsuleCast(topCircle, bottomCircle, c.radius, Vector3.down, GroundedDistanceOffset, mask);
		}
		else return Physics.Raycast(GetPositionWithOffset(LeftFoot), Vector3.down, GroundedDistance, mask) || Physics.Raycast(GetPositionWithOffset(RightFoot), Vector3.down, GroundedDistance, mask);
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