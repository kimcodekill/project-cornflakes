using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Atlas {

	public class AtlasEntry {
		public AudioClip HitSound;
		public ParticleSystem HitSystem;
		public Texture HitDecal;
	}

	public static AtlasEntry defaultEntry = new AtlasEntry();

	private static Dictionary<Material, AtlasEntry> atlas = new Dictionary<Material, AtlasEntry>();

	/// <summary>
	/// Returns sound that should be played when the specified material is hit.
	/// </summary>
	/// <param name="key">The specified material.</param>
	/// <returns>The audioclip to be played.</returns>
	public static AudioClip GetHitSound(Material key) {
		return atlas[key].HitSound;
	}

	/// <summary>
	/// Returns the particle system to be created when the specified material is hit.
	/// </summary>
	/// <param name="key">The specified material.</param>
	/// <returns>The particle system to be created.</returns>
	public static ParticleSystem GetHitSystem(Material key) {
		return atlas[key].HitSystem;
	}
	
	/// <summary>
	/// Returns the decal to be placed when the specified material is hit.
	/// </summary>
	/// <param name="key">The specified material.</param>
	/// <returns>The decal to be placed.</returns>
	public static Texture GetHitDecal(Material key) {
		return atlas[key].HitDecal;
	}

	/// <summary>
	/// Adds an entry to the materal atlas, so that the event listeners know what sound to play when something is hit, etc.
	/// </summary>
	/// <param name="key">The material to be associated with the following parameters.</param>
	/// <param name="hitSound">The sound that should be played when the material is hit.</param>
	/// <param name="hitSystem">The particle system that should be created when the material is hit.</param>
	/// <param name="hitDecal">The decal that should be placed when the material is hit.</param>
	public static void AddEntry(Material key, AudioClip hitSound, ParticleSystem hitSystem, Texture hitDecal) {
		atlas.Add(key, new AtlasEntry() { HitSound = hitSound, HitSystem = hitSystem, HitDecal = hitDecal });
	}

	/// <summary>
	/// Adds an AtlasEntry to the material atlas.
	/// </summary>
	/// <param name="key">The material to be associated with the passed AtlasEntry.</param>
	/// <param name="atlasEntry">The parameters associated with the material.</param>
	public static void AddEntry(Material key, AtlasEntry atlasEntry) {
		atlas.Add(key, atlasEntry);
	}

}