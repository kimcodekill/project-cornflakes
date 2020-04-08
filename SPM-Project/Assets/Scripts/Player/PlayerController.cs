using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stateMachine;
	
	[SerializeField] private State[] states;
	[SerializeField] PlayerCamera cam;

	public PhysicsBody PhysicsBody { get; private set; }

	private void Start() {
		PhysicsBody = GetComponent<PhysicsBody>();
		stateMachine = new StateMachine(this, states) { ShowDebugInfo = true };
	}

	private void Update() {
		stateMachine.Run();
	}

	/// <summary>
	/// Gets the user input and projects it along the plane the Player is standing on, in the direction of the camera rotation.
	/// Adjusts movement so that forward (W) is always in camera look-direction.
	/// </summary>
	/// <returns></returns>
	public Vector3 GetInput() {
		Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		inputDirection = inputDirection.magnitude > 1 ? inputDirection.normalized : inputDirection;
		Vector3 planarProjection = Vector3.ProjectOnPlane(cam.GetRotation() * inputDirection, PhysicsBody.StandingSurface().normal).normalized;
		Vector3 inputVector = planarProjection;
		return inputVector;

	}

}