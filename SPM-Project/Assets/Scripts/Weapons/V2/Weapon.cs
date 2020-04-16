using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour {
	
	/// <summary>
	/// The possible types of ammunition the weapon may use.
	/// </summary>
	public enum EAmmoType {
		Primary,
		Secondary,
		Special
	}

	#region Properties

	/// <summary>
	/// Whether or not the trigger is being pulled.
	/// </summary>
	public bool TriggerDown { get { return Input.GetKey(KeyCode.Mouse0) || OverrideTriggerDown; } }

	/// <summary>
	/// Whether or not input is required to fire.
	/// </summary>
	public bool OverrideTriggerDown { get; set; } = false;

	/// <summary>
	/// Whether or not the reload key is being pressed.
	/// </summary>
	public bool RequestedReload { get { return Input.GetKeyDown(KeyCode.R) || OverrideRequestedReload; } }

	/// <summary>
	/// Whether or not input is required to reload.
	/// </summary>
	public bool OverrideRequestedReload { get; set; } = false;

	/// <summary>
	/// The type of ammunition the weapon uses.
	/// </summary>
	public EAmmoType AmmoType { get => ammoType; protected set => ammoType = value; }

	/// <summary>
	/// Whether or not the weapon is in full auto mode.
	/// </summary>
	public bool FullAuto { get => fullAuto; protected set => fullAuto = value; }

	/// <summary>
	/// The damage the weapon deals per bullet.
	/// </summary>
	public float Damage { get => damage; protected set => damage = value; }

	/// <summary>
	/// The fire rate of the weapon.
	/// </summary>
	public float FireRate { get => fireRate; protected set => fireRate = value; }

	/// <summary>
	/// The size of the magazine.
	/// </summary>
	public int MagazineSize { get => magazineSize; protected set => magazineSize = value; }

	/// <summary>
	/// The amount of ammunition remaining in the magazine.
	/// </summary>
	public int AmmoInMagazine { get => ammoInMagazine; protected set => ammoInMagazine = value; }

	/// <summary>
	/// The amount of ammunition remaining in the reserve.
	/// </summary>
	public int AmmoInReserve { get => ammoInReserve; protected set => ammoInReserve = value; }

	/// <summary>
	/// The amount of time it takes to reload.
	/// </summary>
	public float ReloadTime { get => reloadTime; protected set => reloadTime = value; }

	/// <summary>
	/// The amount of recoil caused by the firing of the weapon.
	/// </summary>
	public float Recoil { get => recoil; protected set => reloadTime = value; }

	/// <summary>
	/// The amount of variance in the bullet path.
	/// </summary>
	public float Spread { get => spread; protected set => spread = value; }

	/// <summary>
	/// What the weapon should be able to hit.
	/// </summary>
	public LayerMask BulletHitMask { get => bulletHitMask; protected set => bulletHitMask = value; }

	/// <summary>
	/// The transform of the muzzle of the weapon.
	/// </summary>
	public Transform Muzzle { get => muzzle; protected set => muzzle = value; }

	#endregion

	#region Serialized

	[Header("States")]
	[SerializeField] private State[] states;
	[Header("Misc. Attributes")]
	[SerializeField] private LayerMask bulletHitMask;
	[SerializeField] private Transform muzzle;
	[Header("Base Attributes")]
	[SerializeField] private EAmmoType ammoType;
	[SerializeField] private bool fullAuto;
	[SerializeField] private float damage;
	[SerializeField] private float fireRate;
	[SerializeField] private int magazineSize;
	[SerializeField] private int ammoInMagazine;
	[SerializeField] private int ammoInReserve;
	[SerializeField] private float reloadTime;
	[SerializeField] private float recoil;
	[SerializeField] private float spread;

	#endregion

	private StateMachine weaponMachine;

	private PlayerCamera playerCamera;

	private Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

	private void Start() {
		weaponMachine = new StateMachine(this, states);
		playerCamera = Camera.main.GetComponent<PlayerCamera>();
	}

	private void Update() {
		weaponMachine.Run();
		DebugManager.UpdateRows("WeaponSTM" + gameObject.GetInstanceID(), new int[] { 1, 2 }, "Magazine: " + AmmoInMagazine, "Reserve: " + GetRemainingAmmoInReserve());
	}

	/// <summary>
	/// Calculates the amount of ammunition remaining in the magazine.
	/// </summary>
	/// <returns>The amount of ammunition remaining in the magazine.</returns>
	public int GetRemainingAmmoInReserve() {
		return ammoInReserve;
	}

	/// <summary>
	/// Decides whether or not there remains ammunition in the magazine.
	/// </summary>
	/// <returns>Whether or not there remains ammunition in the magazine.</returns>
	public bool HasAmmoInMagazine() {
		return ammoInMagazine > 0;
	}

	/// <summary>
	/// Decides whether or not there remains ammunition in the reserve.
	/// </summary>
	/// <returns>Whether or not there remains ammunition in the reserve.</returns>
	public bool HasAmmoInReserve() {
		return ammoInReserve > 0;
	}


	/// <summary>
	/// Calculates the time between shots as according to the RPM of the weapon.
	/// </summary>
	/// <returns></returns>
	public float GetTimeBetweenShots() {
		return 60.0f / fireRate;
	}

	/// <summary>
	/// Calculates the point the crosshair is currently looking at.
	/// </summary>
	/// <returns>The point the crosshair is looking at.</returns>
	public RaycastHit GetCrosshairHit() {
		Ray cameraRay = playerCamera.Camera.ScreenPointToRay(screenCenter);
		Physics.Raycast(cameraRay, out RaycastHit cameraHit, float.MaxValue, bulletHitMask);
		return cameraHit;
	}

	/// <summary>
	/// Returns the normalized direction from a point to another.
	/// </summary>
	/// <param name="origin">The first point.</param>
	/// <param name="target">The second point.</param>
	/// <returns>The direction from <c>origin</c> to <c>target</c>.</returns>
	public Vector3 GetDirectionToPoint(Vector3 origin, Vector3 target) {
		Vector3 direction = target - origin;
		return direction /= direction.magnitude;
	}

	/// <summary>
	/// Reloads the weapon.
	/// </summary>
	public void Reload() {
		int usedBullets = magazineSize - ammoInMagazine;
		int canTakeAmount = (-Mathf.Abs(ammoInReserve - usedBullets - Mathf.Abs(ammoInReserve - usedBullets)) + (2 * usedBullets)) / 2;
		ammoInReserve -= canTakeAmount;
		ammoInMagazine += canTakeAmount;
	}

	/// <summary>
	/// Adds recoil by adjusting camera X rotation.
	/// </summary>
	public virtual void AddRecoil() {
		playerCamera.InjectRotation(Mathf.Lerp(playerCamera.transform.rotation.x,  recoil, 0.01f), 0);
	}

	/// <summary>
	/// Adds a random variance to a vector in the range of the negative to positive <c>Spread</c> property.
	/// </summary>
	/// <param name="direction">The direction to adjust.</param>
	/// <returns>The input vector with added variance.</returns>
	public virtual Vector3 AddSpread(Vector3 direction) {
		return new Vector3(Random.Range(-spread, spread) + direction.x, Random.Range(-spread, spread) + direction.y, Random.Range(-spread, spread) + direction.z).normalized;
	}

	/// <summary>
	/// Called when the weapon state machine enters the WeaponFiringState state and is cleared to fire.
	/// </summary>
	public virtual void Fire() {
		AmmoInMagazine--;
	}

}