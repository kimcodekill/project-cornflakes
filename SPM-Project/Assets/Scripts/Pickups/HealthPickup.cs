using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class HealthPickup : Pickup {

	#region Properties

	public float HealAmount { get => healAmount; set => healAmount = value; }

	#endregion

	#region Serialized

	[SerializeField] private float healAmount;
	[SerializeField] [Tooltip("0-1f")] private Range spawnedHealthPercentageRange;

	#endregion

	protected override void OnPickup(Collider other) {
		other.GetComponent<PlayerController>().Heal(healAmount);
		other.GetComponentInChildren<PlayerHud>().ShowPickupText("health", healAmount);
		other.GetComponent<PlayerController>().PlayAudioPitched(8, 0.7f, 0.8f, 1.3f);
		Destroy(gameObject);
	}

	protected override bool IsValid(Collider other) {
		return other.gameObject.CompareTag("Player") && PlayerController.Instance.PlayerCurrentHealth < PlayerController.Instance.PlayerMaxHealth;
	}

	protected override void OnSpawned() {
		healAmount = PlayerController.Instance.PlayerMaxHealth * Random.Range(spawnedHealthPercentageRange.min, spawnedHealthPercentageRange.max);
	}

	[System.Serializable]
	public struct Range {
		public float min, max;
	}

}