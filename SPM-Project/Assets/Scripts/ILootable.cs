using System.Collections;
using System.Collections.Generic;

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
		public System.Type lootType;
		public object subType;
		public float dropChance;
	}

	private List<LootTableItem> lootTableItems;

	/// <summary>
	/// The indexer used to add loot table items to the loot table.
	/// </summary>
	/// <param name="lootType">What exactly should be dropped.</param>
	/// <param name="subType">The desired subtype, if one exists.</param>
	/// <returns>Nothing, actually.</returns>
	public float this[System.Type lootType, object subType = null] {
		set {
			if (lootTableItems == null) lootTableItems = new List<LootTableItem>();
			lootTableItems.Add(new LootTableItem() {
				lootType = lootType,
				subType = subType,
				dropChance = value
			});
		}
	}

}