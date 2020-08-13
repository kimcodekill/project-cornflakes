using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class Despawner {

	private readonly GameObject toDespawn;

	private readonly MeshRenderer toDespawnMeshRenderer;

	private readonly float startTime;

	private readonly float lifeTime;

	private readonly int flickerSpan;

	private readonly float flickerTimePercentage;

	private int currentFlicker;

	/// <summary>
	/// Creates a new Despawner that will destroy the specified GameObject
	/// after the specified amount of time has elapsed.
	/// </summary>
	/// <param name="toDespawn">Object to eventually destroy.</param>
	/// <param name="lifeTime">How long the object should be alive from the point of instantiation.</param>
	public Despawner(GameObject toDespawn, float lifeTime, int flickerSpan = 3, float flickerTimePercentage = 0.1f) {
		startTime = Time.time;
		this.toDespawn = toDespawn;
		this.lifeTime = lifeTime;
		this.flickerSpan = flickerSpan;
		this.flickerTimePercentage = flickerTimePercentage;
		toDespawnMeshRenderer = toDespawn.GetComponent<MeshRenderer>();
	}

	/// <summary>
	/// Progresses the despawn processes.
	/// Should be called from an <c>Update()</c> function.
	/// </summary>
	public void Run() {
		float delta = Time.time - startTime;
		if (delta > lifeTime) Object.Destroy(toDespawn);
		else if (delta > lifeTime - (lifeTime * flickerTimePercentage)) Flicker();
	}

	private void Flicker() {
		if (++currentFlicker == flickerSpan) {
			toDespawnMeshRenderer.enabled = !toDespawnMeshRenderer.enabled;
			currentFlicker = 0;
		}
	}

}