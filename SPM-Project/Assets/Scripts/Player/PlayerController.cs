using UnityEngine;

public class PlayerController : MonoBehaviour {



	[SerializeField] [Tooltip("The player's possible states.")] private State[] states;
	[SerializeField] [Tooltip("The player's camera.")] private PlayerCamera cam;
	[SerializeField] [Tooltip("The player's HUD.")] private PlayerHud playerHud;

	private AbilityTrigger weaponArray;
	private StateMachine stateMachine;

	/// <summary>
	/// Returns the player's current health, but can never be set from outside the player script.
	/// </summary>
	public float PlayerCurrentHealth { get; private set; }

	/// <summary>
	/// Returns the player's max health, but can never be set outside of the player script.
	/// </summary>
	public float PlayerMaxHealth { get; private set; } = 100;

	/// <summary>
	/// Returns the PhysicsBody attached to the player, but can never be set outside the player script.
	/// </summary>
	public PhysicsBody PhysicsBody { get; private set; }

	/// <summary>
	/// Returns the player's currently equipped weapon.
	/// </summary>
	/// <returns></returns>
	public WeaponBase CurrentPlayerWeapon() { return weaponArray.GetEquippedWeapon(); }

	private void Start() {
		weaponArray = GetComponent<AbilityTrigger>();
		PlayerCurrentHealth = PlayerMaxHealth;
		PhysicsBody = GetComponent<PhysicsBody>();

		stateMachine = new StateMachine(this, states); 

	}

	private void Update() {
		stateMachine.Run();
	}

	/// <summary>
	/// Gets input from the user, normalizes it if the user is using a controller, adjusts for camera rotation so player walks in the direction the camera is looking, and then projects it along the ground/plane.
	/// </summary>
	/// <returns>The projected vector along the player's current ground surface.</returns>
	public Vector3 GetInput() {
		Vector3 movementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
		movementInput = movementInput.magnitude > 1 ? movementInput.normalized : movementInput;
		Vector3 planarProjection = Vector3.ProjectOnPlane(cam.GetRotation() * movementInput, PhysicsBody.GetCurrentSurfaceNormal()).normalized;
		return planarProjection;
	}

	/// <summary>
	/// Regenerats the player's health.
	/// </summary>
	/// <param name="healAmount"> The amount the player should heal.</param>
	public void HealthRegen(float healAmount) {
		PlayerCurrentHealth = PlayerCurrentHealth + healAmount > PlayerMaxHealth ? PlayerMaxHealth : PlayerCurrentHealth + healAmount;

	}

	/// <summary>
	/// Makes the player take damage.
	/// </summary>
	/// <param name="damage">The amount of damage the player will take.</param>
	public void TakeDamage(float damage) {
		playerHud.FlashColor(new Color(1, 0, 0, 0.5f));
		PlayerCurrentHealth -= damage;
		if (PlayerCurrentHealth <= 0)
			Die();
	}

	private void Die() {
		///Unfinished
		//Debug.Log("You died");
		PlayerCurrentHealth = PlayerMaxHealth;
	}
}