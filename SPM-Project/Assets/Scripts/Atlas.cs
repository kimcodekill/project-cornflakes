using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Atlas {

	private class AtlasEntry {
		public AudioClip HitSound;
		public ParticleSystem HitSystem;
		public Texture HitDecal;
	}

	private static Dictionary<Material, AtlasEntry> atlas = new Dictionary<Material, AtlasEntry>();

	public static AudioClip GetHitSound(Material key) {
		return atlas[key].HitSound;
	}

	public static ParticleSystem GetHitSystem(Material key) {
		return atlas[key].HitSystem;
	}
	
	public static Texture GetHitDecal(Material key) {
		return atlas[key].HitDecal;
	}

	public static void AddEntry(Material key, AudioClip hitSound, ParticleSystem hitSystem, Texture hitDecal) {
		atlas.Add(key, new AtlasEntry() { HitSound = hitSound, HitSystem = hitSystem, HitDecal = hitDecal });
	}

}