using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyState/EnemyAttackingState")]
public class EnemyAttackingState : EnemyBaseState
{
	[SerializeField] [Tooltip("Bullet object to instantiate when enemy attacks.")] private Bullet bulletPrefab;
	[SerializeField] [Tooltip("How often/fast the enemy attacks.")] private float attackCooldown;
	[SerializeField] [Tooltip("How much damage the enemy deals per shot.")] private float attackDamage;

	private float internalAttackCD;

	public override void Enter() {
		//Debug.Log("attacking");
		Enemy.StartAttackBehaviour();
	}

	public override void Run() {
		if(!Enemy.TargetIsAttackable()) {
			StateMachine.Pop();
		}

		internalAttackCD += Time.deltaTime;
		if(internalAttackCD > attackCooldown) {
			AttackTarget(Enemy.VectorToTarget);
			internalAttackCD = 0;
		}

	}

	private void AttackTarget(Vector3 v) {
		Bullet instance;
		instance = Instantiate(bulletPrefab, Enemy.gunTransform.position, Enemy.transform.rotation);
		instance.Initialize(v, v.magnitude);
		Enemy.Target.TakeDamage(attackDamage);

	}

	public override void Exit() {
		//Debug.Log("stop attacking");
		Enemy.StopAttackBehaviour();

	}
}

