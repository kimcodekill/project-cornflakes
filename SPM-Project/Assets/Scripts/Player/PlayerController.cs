using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerController : MonoBehaviour, IEntity {

	[SerializeField] [Tooltip("The player's possible states.")] private State[] states;
	[SerializeField] [Tooltip("The player's camera.")] private PlayerCamera cam;
	[SerializeField] [Tooltip("The player's HUD.")] private PlayerHud playerHud;
	[SerializeField] private AudioClip[] audioClips;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] public AudioSource audioPlayerSteps;
	[SerializeField] private AudioSource audioPlayerIdle;

	//private AudioSource[] audioSources;

	/// <summary>
	/// Singleton
	/// </summary>
	public static PlayerController Instance;

	private StateMachine stateMachine;

	/// <summary>
	/// Returns the player's current health, but can never be set from outside the player script.
	/// </summary>
	public float PlayerCurrentHealth { get; set; } = -1;

	/// <summary>
	/// Returns the player's max health, but can never be set outside of the player script.
	/// </summary>
	public float PlayerMaxHealth { get; private set; } = 100;

	/// <summary>
	/// Returns the PhysicsBody attached to the player, but can never be set outside the player script.
	/// </summary>
	public PhysicsBody PhysicsBody { get; private set; }

	/// <summary>
	/// The inputs the player decided on for the current fixed update interval.
	/// </summary>
	public CurrentInput Input { get; private set; }

	/// <summary>
	/// The container class for all the input parameters.
	/// If <c>discard</c> is <c>true</c>, the input set needs to be refreshed.
	/// </summary>
	public class CurrentInput {
		public bool doJump;
		public bool doDash;
	}

	private void OnEnable() {
		if (Instance == null) Instance = this;
	}

	private void Start() {
		if (PlayerCurrentHealth == -1) PlayerCurrentHealth = PlayerMaxHealth;
		PhysicsBody = GetComponent<PhysicsBody>();
		Input = new CurrentInput();
		stateMachine = new StateMachine(this, states);
		audioPlayerIdle = gameObject.AddComponent<AudioSource>();
		audioPlayerIdle.loop = true;
		audioPlayerIdle.clip = audioClips[0];
		audioPlayerIdle.Play();

		DebugManager.AddSection("Input", "Jump: ", "Dash: ");
	}

	private void FixedUpdate() {
		stateMachine.Run();
		Input.doJump = false;
		Input.doDash = false;
	}

	private void Update() {
		if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) Input.doJump = true;
		if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)) Input.doDash = true;

		DebugManager.UpdateAll("Input", "Jump: " + Input.doJump, "Dash: " + Input.doDash);
	}

	/// <summary>
	/// Gets input from the user, normalizes it if the user is using a controller, adjusts for camera rotation so player walks in the direction the camera is looking, and then projects it along the ground/plane.
	/// </summary>
	/// <returns>The projected vector along the player's current ground surface.</returns>
	public Vector3 GetInput() {
		Vector3 movementInput = new Vector3(UnityEngine.Input.GetAxisRaw("Horizontal"), 0, UnityEngine.Input.GetAxisRaw("Vertical"));
		movementInput = movementInput.magnitude > 1 ? movementInput.normalized : movementInput;
		Vector3 planarProjection = Vector3.ProjectOnPlane(cam.transform.rotation * movementInput, PhysicsBody.IsGrounded() ? PhysicsBody.GetCurrentSurfaceNormal().normalized : Vector3.up);
		return planarProjection;
	}

	/// <summary>
	/// Regenerates the player's health. Implements <c>IEntity.TakeDamage()</c>
	/// </summary>
	/// <param name="amount"> The amount the player should heal.</param>
	public float Heal(float amount) {
		PlayerCurrentHealth = PlayerCurrentHealth + amount > PlayerMaxHealth ? PlayerMaxHealth : PlayerCurrentHealth + amount;
		return PlayerCurrentHealth;
	}

	/// <summary>
	/// Makes the player take damage. Implements <c>IEntity.TakeDamage()</c>.
	/// </summary>
	/// <param name="amount">The amount of damage the player will take.</param>
	public float TakeDamage(float amount) {
		playerHud.FlashColor(new Color(1, 0, 0, 0.5f));
		PlayAudioPitched(Random.Range(5, 7), 0.5f, 0.8f, 1.3f);
		PlayerCurrentHealth -= amount;
		if (PlayerCurrentHealth <= 0)
			Die();
		return PlayerCurrentHealth;
	}

	private void Die() {

		//Changed so player death now fires deathevent which (currently) reloads the scene instantly
		EventSystem.Current.FireEvent(new PlayerDeadEvent()
		{
			Description = "Player fricking died, yo."
		});
	}

	public void PlayAudioMain(int clipIndex, float volume) {
		audioSource.pitch = 1;
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
	}

	public void PlayAudioPitched(int clipIndex, float volume, float minPitch, float maxPitch) {
		audioSource.pitch = Random.Range(minPitch, maxPitch);
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
	}
}