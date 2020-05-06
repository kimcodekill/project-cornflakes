using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy, ILootable {

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/HealthPickup"] = 0.25f,
			["Pickups/Ammo/RocketsPickup"] = PlayerController.Instance.PlayerWeapon.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.25f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerController.Instance.PlayerWeapon.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.25f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerController.Instance.PlayerWeapon.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.25f : 0f,
		};
	}

}