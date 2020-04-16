using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<WeaponFiredEvent>(OnWeaponFired);
		EventSystem.Current.RegisterListener<WeaponReloadingEvent>(OnWeaponReloading);
	}

	private void OnWeaponFired(Event e) {
		WeaponFiredEvent wfe = e.GetReal();
		
	}

	private void OnWeaponReloading(Event e) {
		WeaponReloadingEvent wre = e.GetReal();
	}

}