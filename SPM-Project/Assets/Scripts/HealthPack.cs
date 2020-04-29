using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour {
	private BoxCollider bc;
	[SerializeField] float healAmount;

	// Start is called before the first frame update
	void Start() {
		bc = GetComponent<BoxCollider>();
		ParticleSystemRenderer psr = gameObject.AddComponent<ParticleSystemRenderer>();
		ParticleSystem ps = gameObject.AddComponent<ParticleSystem>();
		ps.startLifetime = 1f;
		ps.startSize = 0.25f;
		var ns = ps.shape;
		ns.shapeType = ParticleSystemShapeType.Cone;
		ns.rotation = new Vector3(-90f, 0f, 0f);
		psr.material = new Material(gameObject.GetComponent<MeshRenderer>().material);
		psr.material.shader = Shader.Find("Unlit/Color");
		psr.material.SetColor("_Color", psr.material.GetColor("_Color") / 2f);
	}

	private void FixedUpdate() {
		this.transform.Rotate(0f, 1f, 0f, Space.Self);
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			//Debug.Log(other + " picked up healthpack. Healing " + other);
			other.GetComponent<PlayerController>().Heal(healAmount);
			other.GetComponentInChildren<PlayerHud>().PickupText("health", healAmount);
			Destroy(gameObject);
		}
	}

}