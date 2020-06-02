using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AudioListener : MonoBehaviour {
	private void Start() {
		EventSystem.Current.RegisterListener<WeaponReloadingEvent>(PlayReload);
		EventSystem.Current.RegisterListener<WeaponFiredEvent>(PlayWeaponFire);
		EventSystem.Current.RegisterListener<WeaponFireStoppedEvent>(PlayWeaponFireDecay);
		EventSystem.Current.RegisterListener<WeaponSwitchedEvent>(PlayWeaponSwitch);
	}

	private void PlayReload(Event e)
	{
		WeaponReloadingEvent wre = e as WeaponReloadingEvent;

		PlayAlone(wre.AudioSource, wre.AudioClip);
	}

	private void PlayWeaponFire(Event e)
	{
		WeaponFiredEvent wfe = e as WeaponFiredEvent;

		if (wfe.Loop && wfe.AudioSource.clip != wfe.AudioClip)
		{
			Debug.Log("Started Loop");
			wfe.AudioSource.clip = wfe.AudioClip;
			wfe.AudioSource.loop = true;
			wfe.AudioSource.Play();
		}
		else if (!wfe.Loop) {
			PlayAlone(wfe.AudioSource, wfe.AudioClip);
		}
	}

	private void PlayWeaponFireDecay(Event e)
	{
		WeaponFireStoppedEvent wfse = e as WeaponFireStoppedEvent;

		if (wfse.AudioSource.isPlaying) { PlayAlone(wfse.AudioSource, wfse.AudioClip); }
	}

	private void PlayWeaponSwitch(Event e)
	{
		WeaponSwitchedEvent wse = e as WeaponSwitchedEvent;

		PlayAlone(wse.AudioSource, wse.AudioClip);
	}

	private void PlayAlone(AudioSource source, AudioClip clip)
	{
		Debug.Log("Started OneShot");
		source.Stop();
		source.clip = null;
		source.loop = false;
		source.PlayOneShot(clip);
	}
}