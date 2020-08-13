﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
//Co Author: Joakim Linna

[CreateAssetMenu(menuName = "WeaponState/FiringState")]
public class WeaponFiringState : WeaponState {

	private float startTime;

	private bool semiShot;

	public override void Enter() {
		DebugManager.UpdateRow("WeaponSTM", GetType().ToString());

		if (Weapon is AutoRifle) (Weapon as AutoRifle).CurrentCooldownTime = 0f;

		startTime = Time.time;
		semiShot = false;
	}

	public override void Run() {
		if (!Weapon.HasAmmoInMagazine())
		{
			StateMachine.TransitionTo<WeaponIdleState>();
		}
		else
		{
			if (!Weapon.FullAuto)
			{
				if (!semiShot)
				{
					PlayerController.Instance.playerAnimator.SetTrigger("Shooting");
					Weapon.DoFire(false);
					semiShot = true;
				}
				if ((Time.time - startTime) > Weapon.GetTimeBetweenShots())
				{
					StateMachine.TransitionTo<WeaponIdleState>();
				}
			}
			else
			{
				if(Weapon.TriggerHeld)
				{
					if ((Time.time - startTime) > Weapon.GetTimeBetweenShots())
					{
						Weapon.DoFire(true);
						startTime = Time.time;
					}
				}
				else
				{
					//K: wouldn't have to check this if our weapon audio was structured different but hey whatever
					if (Weapon.ShotDecayAudio != null) { EventSystem.Current.FireEvent(new WeaponFireStoppedEvent(Weapon.ShotDecayAudio, PlayerWeapon.Instance.WeaponAudio)); }
					StateMachine.TransitionTo<WeaponIdleState>();
				}
			}
		}

		base.Run();
	}

    public override void Exit()
    {
		Weapon.ToggleBulletLine(false);
		Weapon.ToggleMuzzleFlash(false);
    }
}