using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stateMachine;
	
	[SerializeField] private State[] states;

	public PhysicsBody PhysicsBody { get; private set; }

	private void Start() {
		PhysicsBody = GetComponent<PhysicsBody>();
		stateMachine = new StateMachine(this, states) { ShowDebugInfo = true };
	}

	private void Update() {
		stateMachine.Run();
	}

	public Vector3 GetInput() {
		return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
	}

}