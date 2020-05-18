using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class InteractionListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<HitEvent>(OnHit);
		EventSystem.Current.RegisterListener<DamageEvent>(OnDamage);
	}

	private void OnHit(Event e) {
		HitEvent he = (HitEvent) e;
		if (he.Target.GetComponent<IEntity>() != null) {
			EventSystem.Current.FireEvent(new DamageEvent() {
				Description = he.Source + " damaged " + he.Target,
				Source = he.Source,
				Target = he.Target,
				//I don't like how the explosion checks the distance from the center of the hit object, but no one will notice (probably)
				Damage = he.Source.GetComponent<Rocket>() ? he.Source.GetComponent<IDamaging>().GetExplosionDamage(he.Source.transform.position, he.Target.transform.position) : he.Source.GetComponent<IDamaging>().GetDamage()
			});
		}
	}

	private void OnDamage(Event e) {
		DamageEvent de = (DamageEvent) e;
		de.Target.GetComponent<IEntity>().TakeDamage(de.Damage);
	}
}