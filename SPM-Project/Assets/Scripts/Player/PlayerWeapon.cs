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

	[SerializeField] private State[] states;

	private List<Weapon> weapons = new List<Weapon>();

	private StateMachine weaponStateMachine;

	private void OnEnable()
	{
		//Just sets the static instance to this if it's null
		if (Instance == null) { Instance = this; }
	}

	private void Update() {
		//Moved CheckInputs here because why wouldn't we do that first
		CheckInputs();

		if (WeaponIsActive) {
			if (weaponStateMachine != null) {
				weaponStateMachine.Run();
				DebugManager.UpdateRows("WeaponSTM", new int[] { 1, 2, 3 }, CurrentWeapon.ToString(), "Magazine: " + CurrentWeapon.AmmoInMagazine, "Reserve: " + CurrentWeapon.GetRemainingAmmoInReserve());
			}
		}
	}

	private void CheckInputs() {
		for (int i = 0; i < weapons.Count; i++)
			if (Input.GetKeyDown((i + 1).ToString()))
			{
				SwitchTo(i);
				//Let player equip weapon by clicking the corresponding button
				WeaponIsActive = true;
				return;
			}

		//Doing this here bc there isnt a better way to do it at this time
		if(CurrentWeapon == null) { WeaponIsActive = false; }
		//else if (Input.GetKeyDown(KeyCode.E)) { WeaponIsActive = !WeaponIsActive; }
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
		weapon.Muzzle = Camera.main.transform;
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

	/// <summary>
	/// Struct used to pass the reserveAmmo and maxAmmo values of a weapon.
	/// </summary>
	public struct AmmoPool {
		public int reserveAmmo;
		public int maxAmmo;
		public AmmoPool(int reserveAmmo, int maxAmmo) {
			this.reserveAmmo = reserveAmmo;
			this.maxAmmo = maxAmmo;
		}
	}

	/// <summary>
	/// Gets the current reserve ammo and max ammo of the weapon of some type.
	/// </summary>
	/// <param name="ammoType">The ammo type to look for.</param>
	/// <returns>A little struct with the ammo values.</returns>
	/// <exception cref="System.Exception">Thrown if no weapon with the specified ammo type exists.</exception>
	public AmmoPool GetAmmoPool(Weapon.EAmmoType ammoType) {
		Debug.LogWarning("AmmoInReserve has its dummy value 1000, change to weapons[i].GetMaxAmmo() in PlayerWeapon.GetAmmoPool()");
		for (int i = 0; i < weapons.Count; i++) {
			if (weapons[i].AmmoType == ammoType) return new AmmoPool(weapons[i].AmmoInReserve, 1000);
		}
		throw new System.Exception("Tried to get ammo for a weapon that the Player doesn't have equipped, which shouldn't be the case.");
	}

}