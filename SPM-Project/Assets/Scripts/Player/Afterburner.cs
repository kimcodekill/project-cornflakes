using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class Afterburner : MonoBehaviour {
	
	/// <summary>
	/// The amount of heat currently on the afterburner.
	/// </summary>
	public float Heat { get; set; }

	/// <summary>
	/// The maximum amount of heat the afterburner can sustain.
	/// </summary>
	public float MaxHeatLevel { get => maxHeatLevel; set => maxHeatLevel = value; }

	/// <summary>
	/// The time delay before heat starts dissipating.
	/// </summary>
	public float HeatDispersalDelay { get => heatDispersalDelay; set => heatDispersalDelay = value; }
	
	/// <summary>
	/// The rate at which heat is dissipated.
	/// </summary>
	public float HeatDispersalRate { get => heatDispersalRate; set => heatDispersalRate = value; }

	[SerializeField] private float maxHeatLevel;
	[SerializeField] private float heatDispersalDelay;
	[SerializeField] private float heatDispersalRate;

	private float heatDispersalStartTime;

	private void Start() {
		DebugManager.AddSection("Afterburner" + gameObject.GetInstanceID(), "", maxHeatLevel.ToString());
	}

	private void Update() {
		if ((heatDispersalStartTime += Time.deltaTime) > heatDispersalDelay) {
			Heat = Mathf.MoveTowards(Heat, 0f, heatDispersalRate * Time.deltaTime);
			DebugManager.UpdateRow("Afterburner" + gameObject.GetInstanceID(), Heat.ToString());
		}
	}

	/// <summary>
	/// Checks if the afterburner is cold enough to use.
	/// </summary>
	/// <param name="heatIncrement">The requested heat increment.</param>
	/// <returns>Whether or not the afterburner is cold enough to use.</returns>
	public bool CanFire(float heatIncrement = 3f) {
		return Heat + heatIncrement <= maxHeatLevel;
	}

	/// <summary>
	/// Adds heat to the afterburner.
	/// </summary>
	/// <param name="heatIncrement">The amount of heat to be added to the afterburner.</param>
	public void Fire(float heatIncrement = 3f) {
		if (!CanFire(heatIncrement)) throw new System.ArgumentException("fuck you, check if firing is allowed first");
		else {
			Heat += heatIncrement;
			heatDispersalStartTime = 0f;
			DebugManager.UpdateRows("Afterburner" + gameObject.GetInstanceID(), new int[] { 0, 1 }, Heat.ToString(), maxHeatLevel.ToString());
		}
	}

}