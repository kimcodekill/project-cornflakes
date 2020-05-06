using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour, ICapturable {

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
		if (IsValid(other)) OnPickup(other);
	}

	/// <summary>
	/// Dictates what should happen when the pickup passes the valid check.
	/// </summary>
	/// <param name="other">The collider interacting with the pickup.</param>
	protected virtual void OnPickup(Collider other) { }

	/// <summary>
	/// Dictates whether or not the pickup should run its pickup logic.
	/// </summary>
	/// <param name="other">The collider interacting with the pickup.</param>
	/// <returns>Whether or not the pickup is valid.</returns>
	protected virtual bool IsValid(Collider other) { return true; }

	public bool InstanceIsCapturable() {
		return true;
	}

	public object GetPersistentCaptureID() {
		return transform.position;
	}
}