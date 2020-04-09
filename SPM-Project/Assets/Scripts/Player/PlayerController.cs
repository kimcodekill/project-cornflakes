using UnityEngine;

public class PlayerController : MonoBehaviour {

	private StateMachine stateMachine;
	
	[SerializeField] private State[] states;
	public float PlayerMaxHealth { get; private set; } = 100;
	public float PlayerCurrentHealth { get; private set; }
	[SerializeField] PlayerCamera cam;
	[SerializeField] PlayerHud playerHud;
	private AbilityTrigger weaponArray;

	public PhysicsBody PhysicsBody { get; private set; }
	public WeaponBase CurrentPlayerWeapon() { return weaponArray.GetEquippedWeapon(); }

	private void Start() {
		weaponArray = GetComponent<AbilityTrigger>();
		PlayerCurrentHealth = PlayerMaxHealth;
		PhysicsBody = GetComponent<PhysicsBody>();
		stateMachine = new StateMachine(this, states);
	}

	private void Update() {
		//Debug.Log(PlayerCurrentHealth);
		stateMachine.Run();
	}

	public Vector3 GetInput() {
		Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		movementInput = movementInput.magnitude > 1 ? movementInput.normalized : movementInput;
		Vector3 planarProjection = Vector3.ProjectOnPlane(cam.GetRotation() * movementInput, Vector3.up).normalized;
		return planarProjection;
	}

	public void HealthRegen(float healAmount) {
		PlayerCurrentHealth = PlayerCurrentHealth + healAmount > PlayerMaxHealth ? PlayerMaxHealth : PlayerCurrentHealth + healAmount;

	}

	public void TakeDamage(float damage) {
		playerHud.FlashColor(new Color(1, 0, 0, 0.5f));
		PlayerCurrentHealth -= damage;
		if (PlayerCurrentHealth <= 0)
			Die();
	}

	private void Die() {
		Debug.Log("You died");
		PlayerCurrentHealth = PlayerMaxHealth;
		//GameController respawn player
	}
}