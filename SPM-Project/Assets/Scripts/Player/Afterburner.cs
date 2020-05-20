using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Author: Viktor Dahlberg
public class Afterburner : MonoBehaviour {
	
	/// <summary>
	/// The amount of heat currently on the afterburner.
	/// </summary>
	public float Heat { get; private set; }

	[SerializeField] private float firingHeatIncrement;
	[SerializeField] private float maxHeatLevel;
	[SerializeField] private float heatDispersalDelay;
	[SerializeField] private float heatDispersalRate;
	[SerializeField] private Slider slider;

	private float heatDispersalStartTime;

	private void Start() {
		slider.maxValue = maxHeatLevel;
	}

	private void Update() {
		if ((heatDispersalStartTime += Time.deltaTime) > heatDispersalDelay) {
			Heat = Mathf.MoveTowards(Heat, 0f, heatDispersalRate * Time.deltaTime);
			RefreshUI();
		}
	}

	/// <summary>
	/// Checks if the afterburner is cold enough to use.
	/// </summary>
	/// <returns>Whether or not the afterburner is cold enough to use.</returns>
	public bool CanFire() {
		return Heat + firingHeatIncrement <= maxHeatLevel;
	}

	/// <summary>
	/// Adds heat to the afterburner.
	/// </summary>
	/// <param name="heatIncrement">The amount of heat to be added to the afterburner.</param>
	public void Fire(float heatIncrement = 3f) {
		Heat += heatIncrement;
		Heat = Mathf.Ceil(Heat);
		heatDispersalStartTime = 0f;
		RefreshUI();
	}

	private void RefreshUI() {
		slider.value = Heat;
		if (CanFire()) slider.interactable = true;
		else slider.interactable = false;
	}

}