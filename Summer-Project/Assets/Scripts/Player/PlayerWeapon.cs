﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class PlayerWeapon : MonoBehaviour {

	//Singleton
	//Needed (atm) for PlayerCamera StateMachine to get the current weapon
	public static PlayerWeapon Instance;

	public LineRenderer bulletRenderer;
	public Light muzzleLight;

	#region Properties

	/// <summary>
	/// The weapon the player is currently using.
	/// </summary>
	public Weapon CurrentWeapon { get; private set; }

	//Kim: bruh we aint using this, can we get a woop woop for the
	//     fact that we should remove any reference to it..
	/// <summary>
	/// Whether or not the weapon is active.
	/// </summary>
	public bool WeaponIsActive { get; private set; } = false;
	
	/// <summary>
	/// Returns whether or not the appropriate inputs were input.
	/// </summary>
	public bool SwitchWeapon { get { return CheckInputs(); } }

	public Transform Muzzle { get => muzzle; }

	public AudioSource WeaponAudio { get => weaponAudio; }

	#endregion

	#region Serialized

	[SerializeField] private State[] states;
	[SerializeField] private AudioSource weaponAudio;
	//Where the weapons will shoot from;
	[SerializeField] private Transform muzzle;

	#endregion

	private List<Weapon> weapons = new List<Weapon>();

	private StateMachine weaponStateMachine;

	private void OnEnable()
	{
		//Just sets the static instance to this if it's null
		if (Instance == null) { Instance = this; }
	}

	private void Start() { 
		try { DebugManager.AddSection("WeaponSTM", "", "", "", ""); } catch (System.ArgumentException) { }
		bulletRenderer.enabled = false;
		muzzleLight.gameObject.SetActive(false);
	}

	private void Update() {
		if (PauseMenu.GameRunning)
		{
			if (weaponStateMachine != null)
			{
				weaponStateMachine.Run();
				DebugManager.UpdateRows("WeaponSTM", new int[] { 1, 2, 3 }, CurrentWeapon.ToString(), "Magazine: " + CurrentWeapon.AmmoInMagazine, "Reserve: " + CurrentWeapon.GetRemainingAmmoInReserve());
			}
		}
	}

	/// <summary>
	/// Returns the list weapons the player is in possession of.
	/// </summary>
	/// <returns>The carried weapons.</returns>
	public List<Weapon> GetWeapons() { return weapons; }

	/// <summary>
	/// Removes all weapons from the player, and removes the state machine since there are no weapons to be ran.
	/// </summary>
	public void ResetInventory() {
		weapons = new List<Weapon>();
		CurrentWeapon = null;
		weaponStateMachine = null;
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

	/// <summary>
	/// Switches to the weapon at the specified index.
	/// </summary>
	/// <param name="index">The specified index.</param>
	public void SwitchTo(int index) {
		CurrentWeapon = weapons[index];
		weapons[index].SwitchTo();
		gameObject.GetComponentInChildren<PlayerHud>().UpdateActiveWeapon(index);
	}

	/// <summary>
	/// Adds the specified weapon to the weapon list, sets its muzzle location and equips it if no other weapons are equipped.
	/// </summary>
	/// <param name="weapon">The weapon to pick up.</param>
	public void PickUpWeapon(Weapon weapon) {
		weapons.Add(weapon);
		
		if (weapons.Count == 1)
		{
			SwitchTo(0);
			WeaponIsActive = true;
		}

		//weapon.Muzzle = muzzle;
		if (weaponStateMachine == null) {
			weaponStateMachine = new StateMachine(this, states);
		}
	}

	/// <summary>
	/// Toggles whether or not the player is using a weapon.
	/// Set to false if you don't want the gun to fire when Mouse0 is pressed, and so on.
	/// TODO: MESH SWITCHING
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
		for (int i = 0; i < weapons.Count; i++) {
			if (weapons[i].AmmoType == ammoType) return new AmmoPool(weapons[i].AmmoInReserve, weapons[i].GetMaxAmmo());
		}
		throw new System.Exception("Tried to get ammo for a weapon that the Player doesn't have equipped, which shouldn't be the case.");
	}

	/// <summary>
	/// Checks if the player has a weapon using the specified ammo type.
	/// </summary>
	/// <param name="ammoType">The ammo type to check for.</param>
	/// <returns></returns>
	public bool HasWeaponOfAmmoType(Weapon.EAmmoType ammoType) {
		bool valid = false;
		for (int i = 0; i < weapons.Count; i++) {
			if (weapons[i].AmmoType == ammoType) valid = true;
		}
		return valid;
	}

}