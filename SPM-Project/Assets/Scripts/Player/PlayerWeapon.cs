using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

	public Weapon CurrentWeapon;

	[SerializeField] private State[] states;

	private List<Weapon> weapons = new List<Weapon>();

	private StateMachine weaponStateMachine;

	private void Update() {
		if (weaponStateMachine != null) {
			weaponStateMachine.Run();
			DebugManager.UpdateRows("WeaponSTM", new int[] { 1, 2, 3 }, CurrentWeapon.ToString(), "Magazine: " + CurrentWeapon.AmmoInMagazine, "Reserve: " + CurrentWeapon.GetRemainingAmmoInReserve());
		}
	}

	public void SwitchTo(int index) {
		CurrentWeapon = weapons[index];
	}

	public void PickUpWeapon(Weapon weapon) {
		weapons.Add(weapon);
		if (weapons.Count == 0) CurrentWeapon = weapon;
		if (weaponStateMachine == null) {
			weaponStateMachine = new StateMachine(this, states);
		}
	}



}