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
		if (he.Target.GetComponentInParent<IEntity>() != null) {
			EventSystem.Current.FireEvent(new DamageEvent() {
				Description = he.Source + " damaged " + he.Target,
				Source = he.Source,
				Target = he.Target,
				Damage = he.Source.GetComponent<IDamaging>().GetDamage()
			});
		}
	}

	private void OnDamage(Event e) {
		DamageEvent de = e.GetReal();
		de.Target.GetComponentInParent<IEntity>().TakeDamage(de.Damage);
	}

}