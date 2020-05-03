using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy, ILootable {

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/HealthPickup"] = 0.25f,
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.25f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.25f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.25f : 0f,
		};

	}

}