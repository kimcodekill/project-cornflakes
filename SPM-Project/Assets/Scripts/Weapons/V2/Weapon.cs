using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
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

	#region Parameters

	/// <summary>
	/// Whether or not the trigger is being pulled.
	/// </summary>
	public bool TriggerDown { get { return Input.GetKeyDown(KeyCode.Mouse0) || OverrideTriggerDown; } }

	/// <summary>
	/// Whether or not input is required to fire.
	/// </summary>
	public bool OverrideTriggerDown { get; set; }

	/// <summary>
	/// Whether or not the weapon is being aimed down sights with.
	/// </summary>
	public bool AimingDownSights { get { return Input.GetKeyDown(KeyCode.Mouse1); } }

	/// <summary>
	/// Whether or not the reload key is being pressed.
	/// </summary>
	public bool RequestedReload { get { return Input.GetKeyDown(KeyCode.Mouse0) || OverrideRequestedReload; } }

	/// <summary>
	/// Whether or not input is required to reload.
	/// </summary>
	public bool OverrideRequestedReload { get; set; }

	/// <summary>
	/// The type of ammunition the weapon uses.
	/// </summary>
	public EAmmoType AmmoType { get => ammoType; protected set => ammoType = value; }

	/// <summary>
	/// Whether or not the weapon is in full auto mode.
	/// </summary>
	public bool FullAuto { get => fullAuto; protected set => fullAuto = value; }

	/// <summary>
	/// The size of the magazine.
	/// </summary>
	public int MagazineSize { get => magazineSize; protected set => magazineSize = value; }

	/// <summary>
	/// The fire rate of the weapon.
	/// </summary>
	public float FireRate { get => fireRate; protected set => fireRate = value; }
	
	/// <summary>
	/// The amount of ammunition remaining in the magazine.
	/// </summary>
	public int AmmoInMagazine { get => ammoInMagazine; protected set => ammoInMagazine = value; }

	/// <summary>
	/// The amount of ammunition remaining in the reserve.
	/// </summary>
	public int AmmoInReserve { get => reserve; private set => reserve = value; }

	#endregion

	#region Serialized

	[Header("Attributes")]
	[SerializeField] private EAmmoType ammoType;
	[SerializeField] private bool fullAuto;
	[SerializeField] private int magazineSize;
	[SerializeField] private float fireRate;
	[SerializeField] private int ammoInMagazine;
	[SerializeField] private int reserve;
	[Header("States")]
	[SerializeField] private State[] states;

	#endregion

	private StateMachine weaponMachine;

	private void Start() {
		weaponMachine = new StateMachine(this, states);	
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
		return AmmoInReserve - MagazineSize;
	}

	/// <summary>
	/// Decides whether or not there remains ammunition in the magazine.
	/// </summary>
	/// <returns>Whether or not there remains ammunition in the magazine.</returns>
	public bool HasAmmoInMagazine() {
		return AmmoInMagazine > 0;
	}

	/// <summary>
	/// Decides whether or not there remains ammunition in the reserve.
	/// </summary>
	/// <returns>Whether or not there remains ammunition in the reserve.</returns>
	public bool HasAmmoInReserve() {
		return AmmoInReserve > 0;
	}


	/// <summary>
	/// Calculates the time between shots as according to the RPM of the weapon.
	/// </summary>
	/// <returns></returns>
	public float GetTimeBetweenShots() {
		return 60.0f / FireRate;
	}

	/// <summary>
	/// Called when the weapon state machine enters the WeaponFiringState state.
	/// </summary>
	public abstract void Fire();

	/// <summary>
	/// Reloads the weapon.
	/// </summary>
	public void Reload() {
		int usedBullets = MagazineSize - AmmoInMagazine;
		AmmoInReserve -= usedBullets;
		AmmoInMagazine = AmmoInReserve > MagazineSize ? MagazineSize : AmmoInReserve;
	}

}