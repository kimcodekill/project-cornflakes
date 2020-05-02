using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : Enemy, ILootable {

	public LootTable GetLootTable() {
		return new LootTable {
			[typeof(HealthPack)] = 0.25f,
			[typeof(AmmoPickup), Weapon.EAmmoType.Rockets] = 0.25f,
			[typeof(AmmoPickup), Weapon.EAmmoType.Shells] = 0.25f,
			[typeof(AmmoPickup), Weapon.EAmmoType.Special] = 0.25f,
		};

	}

}