using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<HitEvent>(OnHit);
		EventSystem.Current.RegisterListener<DamageEvent>(OnDamage);
	}

	private void OnHit(Event e) {
		HitEvent he = e.GetReal();
		if (he.Target.GetComponent<IPawn>() != null) {
			EventSystem.Current.FireEvent(new DamageEvent() {
				Description = "blah",
				Source = he.Source,
				Target = he.Target,
				Damage = he.Source.GetComponent<Weapon>().Damage
			});
		}
	}

	private void OnDamage(Event e) {
		DamageEvent de = e.GetReal();
		de.Target.GetComponent<IPawn>().TakeDamage(de.Damage);
	}

}