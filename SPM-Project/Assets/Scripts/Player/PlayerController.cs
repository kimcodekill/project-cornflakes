using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stateMachine;
	
	[SerializeField] private State[] states;
	[SerializeField] private float playerMaxHealth;
	private float playerCurrentHealth;

	public PhysicsBody PhysicsBody { get; private set; }

	private void Start() {
		PhysicsBody = GetComponent<PhysicsBody>();
		stateMachine = new StateMachine(this, states) { ShowDebugInfo = false };
	}

	private void Update() {
		stateMachine.Run();
	}

	public Vector3 GetInput() {
		return new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
	}

	public void HealthRegen(float healAmount) {
		playerCurrentHealth = playerCurrentHealth + healAmount > playerMaxHealth ? playerMaxHealth : playerCurrentHealth + healAmount;

	}
}