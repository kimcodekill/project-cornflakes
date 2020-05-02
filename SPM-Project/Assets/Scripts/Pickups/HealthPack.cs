using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : Pickup {

	#region Properties

	public float HealAmount { get => healAmount; set => healAmount = value; }

	#endregion

	#region Serialized

	[SerializeField] private float healAmount;

	#endregion

	protected override void OnPickup(Collider other) {
		other.GetComponent<PlayerController>().Heal(healAmount);
		other.GetComponent<PlayerController>().PlayAudioPitched(8, 1, 0.8f, 1.3f);
		Destroy(gameObject);
	}

	protected override bool IsValid(Collider other) {
		return other.gameObject.CompareTag("Player") && PlayerController.Instance.PlayerCurrentHealth < PlayerController.Instance.PlayerMaxHealth;
	}

}