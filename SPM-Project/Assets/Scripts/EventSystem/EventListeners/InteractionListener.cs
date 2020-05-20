using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class InteractionListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<DamageEvent>(OnDamage);
		EventSystem.Current.RegisterListener<HitEvent>(OnHit);
		EventSystem.Current.RegisterListener<BulletHitEvent>(OnBulletHit);
	}

	private void OnHit(Event e) {
		HitEvent he = (HitEvent) e;

		IEntity entity;

		if ((entity = he.Target.GetComponent<IEntity>()) != null) {
			IDamaging damager;
			if((damager = he.Source.GetComponent<IDamaging>()) != null)
			{
				EventSystem.Current.FireEvent(new DamageEvent()
				{
					Entity = entity,
					Damager = damager
				});
			}
		}
	}

	private void OnBulletHit(Event e)
	{
		BulletHitEvent bhe = e as BulletHitEvent;

		OnHit(e);

		EventSystem.Current.FireEvent(new BulletEffectEvent()
		{
			HitEffect = bhe.Weapon.HitDecal,
			WorldPosition = bhe.HitPoint,
			Scale = 1.0f,
			Rotation = Quaternion.identity,
		});
	}

	private void OnExplosionHit(Event e)
	{
		ExplosionHitEvent ehe = e as ExplosionHitEvent;
	}

	private void OnDamage(Event e) {
		DamageEvent de = (DamageEvent) e;
		de.Entity.TakeDamage(de.Damager.GetDamage());
	}
}