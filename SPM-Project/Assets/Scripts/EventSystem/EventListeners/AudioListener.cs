using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<WeaponReloadingEvent>(PlaySound);
		EventSystem.Current.RegisterListener<WeaponFiredEvent>(PlaySound);
		EventSystem.Current.RegisterListener<WeaponSwitchedEvent>(PlaySound);
	}

	private void PlaySound(Event e) {
		EffectEvent ee = e as EffectEvent;
		ee.AudioSource.PlayOneShot(ee.AudioClip);
	}

}