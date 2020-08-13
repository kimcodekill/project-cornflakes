﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Co-Authors: Erik Pilström, Viktor Dahlberg, Joakim Linna
public class PlayerController : MonoBehaviour, IEntity {

	[SerializeField] [Tooltip("The player's possible states.")] private State[] states;
	[SerializeField] [Tooltip("The player's HUD.")] private PlayerHud playerHud;
	[SerializeField] private AudioClip[] audioClips;
	[SerializeField] [Tooltip("Audio Source component #1")] private AudioSource audioSourceMain;
	[SerializeField] [Tooltip("Audio Source component #3")] public AudioSource audioPlayerSteps;
	[SerializeField] [Tooltip("Audio Source component #4")] private AudioSource audioPlayerIdle;
	[SerializeField] public GameObject thrust1, thrust2, thrust3, dash1, dash2, dash3;
	[SerializeField] [Tooltip("How long the player should be dead before game reloads")] private float deathTime = 3f;
	public Animator playerAnimator;
	private float animHorizontal, animVertical;
	[Header("Debug")]
	[SerializeField] private bool godMode;

	/// <summary>
	/// Singleton
	/// </summary>
	public static PlayerController Instance;

	private StateMachine stateMachine;
	private PlayerCamera cam;

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
		if (Instance == null) 
		{ 
			Instance = this;
			DontDestroyOnLoad(gameObject);
			SceneManager.sceneLoaded += OnSceneLoaded;

			if (cam == null) { CreateCamera(); }
		}
		else if (Instance != this) Destroy(gameObject);

	}

	private void Start() {
		if (PlayerCurrentHealth == -1) PlayerCurrentHealth = PlayerMaxHealth;
		PhysicsBody = GetComponent<PhysicsBody>();
		Input = new CurrentInput();
		stateMachine = new StateMachine(this, states);

		/*audioPlayerIdle = gameObject.AddComponent<AudioSource>();
		audioPlayerIdle.loop = true;
		audioPlayerIdle.clip = audioClips[0];*/
		audioPlayerIdle.Play();

		DebugManager.AddSection("Input", "Jump: ", "Dash: ");
	}

	private void FixedUpdate() {
		if (PauseMenu.GameRunning)
		{
			if (!playerAnimator.GetBool("Dying")) stateMachine.Run();
			animVertical = UnityEngine.Input.GetAxis("Vertical");
			animHorizontal = UnityEngine.Input.GetAxis("Horizontal");
			playerAnimator.SetFloat("Speed", animVertical == 0f ? Mathf.Abs(animHorizontal) : animVertical);
			Input.doJump = false;
			Input.doDash = false;
		}
	}

	private void Update() {
		if (PauseMenu.GameRunning)
		{
			if (UnityEngine.Input.GetKeyDown(KeyCode.Space)) Input.doJump = true;
			if (UnityEngine.Input.GetKeyDown(KeyCode.LeftShift)) Input.doDash = true;

			DebugManager.UpdateAll("Input", "Jump: " + Input.doJump, "Dash: " + Input.doDash);
		}
	}

	private void LateUpdate()
	{
		if (PauseMenu.GameRunning)
		{
			//K: Moved these here so it's not as choppy
			float yRot = cam.transform.rotation.eulerAngles.y;
			if (!playerAnimator.GetBool("Dying")) transform.rotation = Quaternion.Euler(0, yRot, 0);
			//Debug.Log("Mesh: " + transform.rotation.eulerAngles.y);
		}
	}

	private void CreateCamera()
	{
		cam = Instantiate(Resources.Load("Player/PlayerCamera") as GameObject, transform.position, Quaternion.identity).GetComponent<PlayerCamera>();
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
	public float TakeDamage(float amount, DamageType damageType) {
		playerHud.FlashColor(new Color(1, 0, 0, 0.5f));
		PlayAudioPitched(Random.Range(5, 7), 0.5f, 0.8f, 1.3f);
		if (!godMode) PlayerCurrentHealth -= amount;
		if (PlayerCurrentHealth <= 0)
            if (!playerAnimator.GetBool("Dying")) Die();
		return PlayerCurrentHealth;
	}

	private void Die() {
        playerAnimator.SetBool("Dying", true);
		Invoke("DoDie", deathTime);//playerAnimator.GetCurrentAnimatorClipInfo(5).Length);
	}

    private void DoDie() {
        //Changed so player death now fires deathevent which (currently) reloads the scene instantly /K
        EventSystem.Current.FireEvent(new PlayerDeadEvent()
        {
            Description = "Player fricking died, yo."
        });
	}

	public void PlayAudioMain(int clipIndex, float volume) {
		audioSourceMain.pitch = 1;
		audioSourceMain.PlayOneShot(audioClips[clipIndex], volume);
	}

	public void PlayAudioPitched(int clipIndex, float volume, float minPitch, float maxPitch) {
		audioSourceMain.pitch = Random.Range(minPitch, maxPitch);
		audioSourceMain.PlayOneShot(audioClips[clipIndex], volume);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		playerAnimator.SetBool("Dying", false);
		if (PlayerSpawn.Instance != null && !CaptureKeeper.LevelHasBeenCaptured)
		{
			transform.position = PlayerSpawn.Instance.Position;

			if(cam == null) { CreateCamera(); }

			cam.InjectSetRotation(PlayerSpawn.Instance.Rotation.eulerAngles.x, PlayerSpawn.Instance.Rotation.eulerAngles.y);
		}
	}

	void OnDestroy()
	{
		if (Instance == this)
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Instance = null;

			if (cam != null) { Destroy(cam.gameObject); }
		}
	}
}