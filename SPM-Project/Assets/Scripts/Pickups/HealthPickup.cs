﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class HealthPickup : Pickup {

	#region Properties

	public float HealAmount { get => healAmount; set => healAmount = value; }

	#endregion

	#region Serialized

	[SerializeField] private float healAmount;

	#endregion

	protected override void OnPickup(Collider other) {
		other.GetComponent<PlayerController>().Heal(healAmount);
		other.GetComponentInChildren<PlayerHud>().ShowPickupText("health", healAmount);
		Destroy(gameObject);
	}

	protected override bool IsValid(Collider other) {
		return other.gameObject.CompareTag("Player") && PlayerController.Instance.PlayerCurrentHealth < PlayerController.Instance.PlayerMaxHealth;
	}

}
