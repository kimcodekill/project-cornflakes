using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootable {

	/// <summary>
	/// Here everything the implementor can drop and how it may drop it needs to be specified.
	/// To create a loot table, simply declare a <c>new LootTable() { [lootType, (subType)] = dropChance, ... };</c>
	/// </summary>
	/// <returns>A loot table.</returns>
	LootTable GetLootTable();

}

/// <summary>
/// The loot table structure.
/// </summary>
public struct LootTable {

	public struct LootTableItem {
		public string lootObjectPath;
		public float dropChance;
	}

	private List<LootTableItem> lootTableItems;

	/// <summary>
	/// The indexer used to add loot table items to the loot table.
	/// </summary>
	/// <param name="lootObjectPath">What exactly should be dropped.</param>
	/// <param name="subType">The desired subtype, if one exists.</param>
	/// <returns>Nothing, actually.</returns>
	public float this[string lootObjectPath] {
		set {
			if (lootTableItems == null) lootTableItems = new List<LootTableItem>();
			lootTableItems.Add(new LootTableItem() {
				lootObjectPath = lootObjectPath,
				dropChance = value
			});
		}
	}

	/// <summary>
	/// Returns a randomly selected loot table item according to the specified table weights.
	/// </summary>
	/// <returns>The resource path of the thing the loot table item wants to spawn.</returns>
	public string Roll() {
		float chanceSum = 0f;
		float[] range = new float[lootTableItems.Count];
		for (int i = 0; i < lootTableItems.Count; i++) {
			chanceSum += lootTableItems[i].dropChance;
			range[i] = chanceSum;
		}
		float winningValue = Random.Range(0f, chanceSum);
		for (int i = 0; i < range.Length; i++) {
			if (range[i] >= winningValue) return lootTableItems[i].lootObjectPath;
		}
		return null;
	}

}