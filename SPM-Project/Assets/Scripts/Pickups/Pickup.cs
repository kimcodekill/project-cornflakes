using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Pickup : MonoBehaviour, ICapturable, ISpawnable {

	private bool fromSpawner;

	private ParticleSystem ps;

	private void Start() {
		ParticleSystemRenderer psr = gameObject.AddComponent<ParticleSystemRenderer>();
		ps = gameObject.AddComponent<ParticleSystem>();
		ps.startLifetime = 0.25f;
		ps.startSize = 0.25f;
		ps.gravityModifier = -3f;
		ps.emissionRate = 20f;
		var ns = ps.shape;
		ns.shapeType = ParticleSystemShapeType.Sphere;
		psr.material = new Material(gameObject.GetComponent<MeshRenderer>().material);
		psr.material.shader = Shader.Find("Unlit/Color");
		psr.material.SetColor("_Color", psr.material.GetColor("_Color") / 2f);
	}

	private void OnTriggerEnter(Collider other) {
		if (IsValid(other)) OnPickup(other);
		else if (other.gameObject.CompareTag("Player") && fromSpawner) Destroy(gameObject);
	}

	protected virtual void CheckCompatibility() { }

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

	public void Spawned() {
		fromSpawner = true;
	}
}