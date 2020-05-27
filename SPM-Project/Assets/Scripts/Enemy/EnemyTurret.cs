using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class EnemyTurret : EnemyBase, ILootable {

	public LootTable GetLootTable() {
		return new LootTable {
			["Pickups/Ammo/RocketsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Rockets) ? 0.2f : 0f,
			["Pickups/Ammo/ShellsPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Shells) ? 0.2f : 0f,
			["Pickups/Ammo/SpecialPickup"] = PlayerWeapon.Instance.HasWeaponOfAmmoType(Weapon.EAmmoType.Special) ? 0.2f : 0f,
			[LootTable.Nothing] = 0.8f,
		};
	}

}