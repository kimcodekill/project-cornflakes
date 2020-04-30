using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

	//Singleton
	//Needed (atm) for PlayerCamera StateMachine to get the current weapon
	public static PlayerWeapon Instance;

	/// <summary>
	/// The weapon the player is currently using.
	/// </summary>
	public Weapon CurrentWeapon { get; private set; }

	/// <summary>
	/// Whether or not the weapon is active.
	/// </summary>
	public bool WeaponIsActive { get; private set; } = false;
	
	public bool SwitchWeapon { get { return CheckInputs(); } }

	[SerializeField] private State[] states;
	
	//Where the weapons will shoot from;
	[SerializeField] private Transform muzzleTransform;

	private List<Weapon> weapons = new List<Weapon>();

	private StateMachine weaponStateMachine;

	private void OnEnable()
	{
		//Just sets the static instance to this if it's null
		if (Instance == null) { Instance = this; }
	}

	private void Update() {
		if (weaponStateMachine != null) {
			weaponStateMachine.Run();
			DebugManager.UpdateRows("WeaponSTM", new int[] { 1, 2, 3 }, CurrentWeapon.ToString(), "Magazine: " + CurrentWeapon.AmmoInMagazine, "Reserve: " + CurrentWeapon.GetRemainingAmmoInReserve());
		}
	}

	private bool CheckInputs() {
		for (int i = 0; i < weapons.Count; i++)
			if (Input.GetKeyDown((i + 1).ToString()))
			{
				SwitchTo(i);

				return true;
			}

		return false;
	}

	private void SwitchTo(int index) {
		CurrentWeapon = weapons[index];
	}

	/// <summary>
	/// Adds the specified weapon to the weapon list, sets its muzzle location and equips it if no other weapons are equipped.
	/// TODO: APPROPRIATE MUZZLE LOCATIONS
	/// </summary>
	/// <param name="weapon">The weapon to pick up.</param>
	public void PickUpWeapon(Weapon weapon) {
		if (weapons.Count == 0)
		{
			CurrentWeapon = weapon;
			WeaponIsActive = true;
		}
		weapons.Add(weapon);
		weapon.Muzzle = muzzleTransform;
		if (weaponStateMachine == null) {
			weaponStateMachine = new StateMachine(this, states);
		}
	}

	/// <summary>
	/// Toggles whether or not the player is using a weapon.
	/// Set to false if you don't want the gun to fire when Mouse0 is pressed, and so on.
	/// TODO: MESH SWITCHING/CAMERA TOGGLING
	/// </summary>
	/// <param name="isActive">Whether or not the gun should be active.</param>
	public void SetWeaponActive(bool isActive) {
		WeaponIsActive = isActive;
	}

	/// <summary>
	/// Adds the specified amount of ammunition to the specified ammo pool.
	/// </summary>
	/// <param name="ammoType">The type of ammunition to add.</param>
	/// <param name="amount">The amount of ammunition to add.</param>
	public void AddAmmo(Weapon.EAmmoType ammoType, int amount) {
		for (int i = 0; i < weapons.Count; i++) {
			if (weapons[i].AmmoType == ammoType) weapons[i].AmmoInReserve += amount;
		}
	}

}