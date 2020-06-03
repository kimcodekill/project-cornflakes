using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class InteractionListener : MonoBehaviour {

	private void Start() {
		EventSystem.Current.RegisterListener<DamageEvent>(OnDamage);
		EventSystem.Current.RegisterListener<ExplosiveDamageEvent>(OnExplosiveDamage);
		//EventSystem.Current.RegisterListener<HitEvent>(OnHit);
		//EventSystem.Current.RegisterListener<BulletHitEvent>(OnBulletHit);
	}

	//private void OnHit(Event e) {
	//	HitEvent he = (HitEvent) e;

	//	IEntity entity;

	//	if ((entity = he.Target.GetComponent<IEntity>()) != null) {
	//		IDamaging damager;
	//		if((damager = he.Source.GetComponent<IDamaging>()) != null)
	//		{
	//			EventSystem.Current.FireEvent(new DamageEvent(entity, damager));
	//		}
	//	}
	//}

	//private void OnBulletHit(Event e)
	//{
	//	BulletHitEvent bhe = e as BulletHitEvent;

	//	OnHit(e);

	//	EventSystem.Current.FireEvent(new BulletEffectEvent(bhe.Weapon.HitDecal)
	//	{
	//		HitEffect = bhe.Weapon.HitDecal,
	//		WorldPosition = bhe.HitPoint,
	//		Scale = 1.0f,
	//		Rotation = Quaternion.identity,
	//	});
	//}

	private void OnDamage(Event e) {
		DamageEvent de = (DamageEvent) e;
		if (de.Entity != null && de.Damager != null)
		{
			de.Entity.TakeDamage(de.Damager.GetDamage(), DamageType.Bullet);
		}
	}

	private void OnExplosiveDamage(Event e)
	{
		ExplosiveDamageEvent ede = e as ExplosiveDamageEvent;
		if (ede.Entity != null && ede.Damager != null)
		{
			ede.Entity.TakeDamage(ede.Damager.GetDamage() * ede.DamageScale, DamageType.Explosive);
		}
	}
}