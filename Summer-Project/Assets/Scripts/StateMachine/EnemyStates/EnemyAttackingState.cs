﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

//Author: Erik Pilström
[CreateAssetMenu(menuName = "EnemyState/EnemyAttackingState")]
public class EnemyAttackingState : EnemyBaseState
{
	private float internalAttackCD = 0;

	public override void Enter() {
		Enemy.StartAttackBehaviour();
	}

	public override void Run() {
		if(!Enemy.TargetIsAttackable()) {
			StateMachine.TransitionTo<EnemyAlertedState>();
		}

		if(Time.time > internalAttackCD && Enemy.WeaponIsAimed()) {
			internalAttackCD = Time.time + Enemy.EnemyEquippedWeapon.GetFireRate();
			Enemy.EnemyEquippedWeapon.DoAttack();
		}
	}

	public override void Exit() {
		Enemy.StopAttackBehaviour();

	}
}

