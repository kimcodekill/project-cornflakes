using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<WeaponReloadingEvent>(PlaySound);
		EventSystem.Current.RegisterListener<WeaponFiredEvent>(PlaySound);
		EventSystem.Current.RegisterListener<WeaponSwitchedEvent>(PlaySound);
	}

	private void PlaySound(Event e) {
		EffectEvent ee = e as EffectEvent;
		ee.Audio.AudioSource.PlayOneShot(ee.Audio.AudioClip, ee.Audio.Volume);
	}

}

/// <summary>
/// Note: If the AudioSource belongs to another GameObject family (not parent nor child), set it in code instead.
/// </summary>
[System.Serializable]
public struct Audio {
	public AudioClip AudioClip;
	public AudioSource AudioSource;
	public float Volume;
}