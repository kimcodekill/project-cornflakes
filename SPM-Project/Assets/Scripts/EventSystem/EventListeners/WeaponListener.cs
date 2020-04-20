using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<PickUpEvent>(OnPickUp);

	private void OnPickUp(Event e) {
		PickUpEvent pue = (PickUpEvent) e;
		Debug.Log(pue.Description);
		Weapon w;
		PlayerWeapon pw;
		if ((w = pue.Source.GetComponent<Weapon>()) != null && (pw = pue.Target.GetComponent<PlayerWeapon>()) != null) {
			pw.PickUpWeapon(w);
			pue.Source.SetActive(false);
		}
	}

}