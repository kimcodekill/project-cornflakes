using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna
public abstract class Weapon : ScriptableObject, IDamaging {
	
	/// <summary>
	/// The possible types of ammunition the weapon may use.
	/// </summary>
	public enum EAmmoType {
		Infinite,
		Shells,
		Rockets,
		Special
	}

	#region Properties

	/// <summary>
	/// Whether or not the trigger is being pulled.
	/// </summary>
	public bool TriggerPulled { get { return Input.GetKeyDown(KeyCode.Mouse0); } }

	/// <summary>
	/// Whether or not the trigger is being held.
	/// </summary>
	public bool TriggerHeld { get { return Input.GetKey(KeyCode.Mouse0); } }

	/// <summary>
	/// Whether or not input is required to fire.
	/// </summary>
	public bool OverrideTriggerDown { get; set; } = false;

	/// <summary>
	/// Whether or not the reload key is being pressed.
	/// </summary>
	public bool RequestedReload { get { return Input.GetKeyDown(KeyCode.R); } }

	///// <summary>
	///// Whether or not input is required to reload.
	///// </summary>
	//public bool OverrideRequestedReload { get; set; } = false;

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
	public int AmmoInMagazine { get => ammoInMagazine; set => ammoInMagazine = value; }

	/// <summary>
	/// The amount of ammunition remaining in the reserve.
	/// </summary>
	public int AmmoInReserve { get => ammoInReserve; set => ammoInReserve = value; }

	/// <summary>
	/// The amount of time it takes to reload.
	/// </summary>
	public float ReloadTime { get => reloadTime; protected set => reloadTime = value; }

	/// <summary>
	/// The amount of recoil caused by the firing of the weapon.
	/// </summary>
	public float Recoil { get => recoil; protected set => recoil = value; }

	/// <summary>
	/// The amount of variance in the bullet path.
	/// </summary>
	public float SpreadAngle { get => spreadAngle; set => spreadAngle = value; }

	/// <summary>
	/// What the weapon should be able to hit.
	/// </summary>
	public LayerMask BulletHitMask { get => bulletHitMask; protected set => bulletHitMask = value; }

	/// <summary>
	/// The transform of the muzzle of the weapon.
	/// </summary>
	public Transform Muzzle { get => PlayerWeapon.Instance.Muzzle;}

	/// <summary>
	/// The sound to be played when reloading.
	/// </summary>
	public AudioClip ReloadAudio { get => reloadAudio; }

	public AudioClip ShotDecayAudio { get => shotDecayAudio; }
	
	public Sprite Reticle { get => reticle; }
	public GameObject Model { get => model; }
	public GameObject HitDecal { get => hitDecal; }

	#endregion

	#region Serialized

	[Header("Graphical Assets")]
	[SerializeField] private Sprite reticle;
	[SerializeField] private GameObject model;
	[SerializeField] private GameObject hitDecal;
	[Header("Misc. Attributes")]
	[SerializeField] private LayerMask bulletHitMask;
	[Header("Sounds")]
	[SerializeField] private AudioClip reloadAudio;
	[SerializeField] private AudioClip shootAudio;
	[SerializeField] private AudioClip shotDecayAudio;
	[SerializeField] private AudioClip switchAudio;
	[Header("Base Attributes")]
	[SerializeField] private EAmmoType ammoType;
	[SerializeField] private bool fullAuto;
	[SerializeField] private float damage;
	[SerializeField] private float fireRate;
	[SerializeField] private int magazineSize;
	[SerializeField] private int maxMagazines;
	[SerializeField] private float reloadTime;
	[SerializeField] private float recoil;
	[SerializeField] private float spreadAngle;

	#endregion

	private int ammoInMagazine;
	private int ammoInReserve;

	private Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);

	public void Initialize() {
		ammoInMagazine = magazineSize;
		ammoInReserve = 2 * magazineSize < GetMaxAmmo() ? 2 * magazineSize : GetMaxAmmo() ;
	}

	public void Initialize(int ammoInMagazine, int ammoInReserve) {
		this.ammoInMagazine = ammoInMagazine;
		this.ammoInReserve = ammoInReserve;
	}

	#region Attribute Status Functions

	/// <summary>
	/// Implements <c>IDamaging.GetDamage()</c>.
	/// </summary>
	/// <returns>Returns the damage of the weapon.</returns>
	public float GetDamage() {
		return Damage;
	}

	public float GetDamage(float distanceRadius)
	{
		return Damage;
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
	/// <returns>The time until a shot can be fired after a previous shot.</returns>
	public float GetTimeBetweenShots() {
		return 60.0f / fireRate;
	}

	#endregion

	#region Helper Functions

	/// <summary>
	/// Calculates the point the crosshair is currently looking at.
	/// </summary>
	/// <returns>The point the crosshair is looking at.</returns>
	protected Vector3 GetCrosshairHitPoint() {
		//Ray cameraRay = PlayerCamera.Instance.Camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
		Ray cameraRay = PlayerCamera.Instance.Camera.ScreenPointToRay(new Vector2(PlayerCamera.Instance.Camera.pixelWidth / 2.0f, PlayerCamera.Instance.Camera.pixelHeight / 2.0f));

		//Debug.DrawLine(cameraRay.origin, cameraRay.origin + cameraRay.direction * 10.0f, Color.green, 10.0f);
		Physics.Raycast(cameraRay, out RaycastHit cameraHit, float.MaxValue, bulletHitMask);
		return cameraHit.collider == null ? Muzzle.position + PlayerCamera.Instance.transform.forward: cameraHit.point;
	}

	/// <summary>
	/// Returns the normalized direction from a point to another.
	/// </summary>
	/// <param name="origin">The first point.</param>
	/// <param name="target">The second point.</param>
	/// <returns>The direction from <c>origin</c> to <c>target</c>.</returns>
	protected Vector3 GetDirectionToPoint(Vector3 origin, Vector3 target) {
		return (target - origin).normalized;
	}

	/// <summary>
	/// Casts a ray from the muzzle of the gun to whatever the player is aiming at and returns the result.
	/// </summary>
	/// <returns>The resulting RaycastHit</returns>
	protected RaycastHit MuzzleCast() {
		Vector3 direction = GetDirectionToPoint(Muzzle.position, GetCrosshairHitPoint());
		//Debug.DrawLine(Muzzle.position, Muzzle.position + direction * 10.0f, Color.red, 10.0f);
		Physics.Raycast(Muzzle.position, AddSpread(direction), out RaycastHit hit, float.MaxValue, BulletHitMask);
		return hit;
	}

	/// <summary>
	/// Adds recoil by adjusting camera X rotation.
	/// </summary>
	protected virtual void AddRecoil() {
		PlayerCamera.Instance.InjectRotation(Mathf.Lerp(PlayerCamera.Instance.Camera.transform.rotation.x,  recoil, 0.01f), 0);
	}

	/// <summary>
	/// Adds a random variance to a vector in the range of the negative to positive <c>Spread</c> property.
	/// </summary>
	/// <param name="direction">The direction to adjust.</param>
	/// <returns>The input vector with added variance.</returns>
	protected virtual Vector3 AddSpread(Vector3 direction) {
		return RandomInCone(spreadAngle, direction);
		//return new Vector3(Random.Range(-spread, spread) + direction.x, Random.Range(-spread, spread) + direction.y, Random.Range(-spread, spread) + direction.z).normalized;
	}

	public Vector3 RandomInCone(float spreadAngle, Vector3 axis)
	{
		float radians = Mathf.PI / 180 * spreadAngle / 2;
		float dotProduct = Random.Range(Mathf.Cos(radians), 1);
		float polarAngle = Mathf.Acos(dotProduct);
		float azimuth = Random.Range(-Mathf.PI, Mathf.PI);
		Vector3 yProjection = Vector3.ProjectOnPlane(Vector3.up, axis);
		if (Vector3.Dot(axis, Vector3.up) > 0.9f)
		{
			yProjection = Vector3.ProjectOnPlane(Vector3.forward, axis);
		}
		Vector3 y = yProjection.normalized;
		Vector3 x = Vector3.Cross(y, axis);
		Vector3 pointOnSphericalCap = Mathf.Sin(polarAngle) * (Mathf.Cos(azimuth) * x + Mathf.Sin(azimuth) * y) + Mathf.Cos(polarAngle) * axis;
		return pointOnSphericalCap;
	}

	#endregion

	/// <summary>
	/// Reloads the weapon.
	/// </summary>
	public void Reload() {
		int usedBullets = magazineSize - ammoInMagazine;
		int canTakeAmount = (-Mathf.Abs(ammoInReserve - usedBullets - Mathf.Abs(ammoInReserve - usedBullets)) + (2 * usedBullets)) / 2;
		ammoInReserve -= canTakeAmount;
		ammoInMagazine += canTakeAmount;
	}

	//K: this is a non necessary wrapper method
	/// <summary>
	/// Called when the weapon state machine enters the WeaponFiringState state and is cleared to fire.
	/// Fires an event and then lets the <c>Fire()</c> function assume control.
	/// </summary>
	public void DoFire(bool loop) {
		EventSystem.Current.FireEvent(new WeaponFiredEvent(shootAudio, PlayerWeapon.Instance.WeaponAudio, loop));
		Fire();
	}

	//K: this is a non necessary wrapper method.
	/// <summary>
	/// Logic for what actually happens when a weapon is fired.
	/// </summary>
	protected virtual void Fire() {
		AmmoInMagazine--;
	}

	//K: this really is a stupid wrapper but whatever
	public void SwitchTo() {
		EventSystem.Current.FireEvent(new WeaponSwitchedEvent(switchAudio, PlayerWeapon.Instance.WeaponAudio, this));
	}

	public int GetMaxAmmo()	{
		return maxMagazines * magazineSize;
	}
}