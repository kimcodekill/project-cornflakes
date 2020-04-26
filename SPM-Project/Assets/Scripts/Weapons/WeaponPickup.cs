using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour {

	private void Start() {
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

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.CompareTag("Player")) {
			EventSystem.Current.FireEvent(new PickUpEvent() {
				Description = this + " was picked up by " + other.gameObject,
				Source = gameObject,
				Target = other.gameObject,
			});
		}
	}

}