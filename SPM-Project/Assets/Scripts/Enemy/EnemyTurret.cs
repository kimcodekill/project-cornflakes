using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy, ILootable {

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.20f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.25f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.25f : 0f,
			[LootTable.Nothing] = 0.8f,
		};
	}

}