using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stateMachine;
	
	[SerializeField] private State[] states;
	[SerializeField] private float playerMaxHealth;
	private float playerCurrentHealth;
	[SerializeField] PlayerCamera cam;

	public PhysicsBody PhysicsBody { get; private set; }

	private void Start() {
		PhysicsBody = GetComponent<PhysicsBody>();
		stateMachine = new StateMachine(this, states) { ShowDebugInfo = true };
	}

	private void Update() {
		stateMachine.Run();
	}

	public Vector3 GetInput() {
		Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		movementInput = movementInput.magnitude > 1 ? movementInput.normalized : movementInput;
		Vector3 planarProjection = Vector3.ProjectOnPlane(cam.GetRotation() * movementInput, Vector3.up).normalized;
		return planarProjection;
	}

	public void HealthRegen(float healAmount) {
		playerCurrentHealth = playerCurrentHealth + healAmount > playerMaxHealth ? playerMaxHealth : playerCurrentHealth + healAmount;

	}

}