using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour {
	private BoxCollider bc;
	[SerializeField] float healAmount;

	// Start is called before the first frame update
	void Start() {
		bc = GetComponent<BoxCollider>();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			//Debug.Log(other + " picked up healthpack. Healing " + other);
			other.GetComponent<PlayerController>().Heal(healAmount);
			Destroy(gameObject);
		}

	}

}