using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Afterburner : MonoBehaviour {
	
	public float Heat { get; set; }

	public float MaxHeatLevel { get => maxHeatLevel; set => maxHeatLevel = value; }

	public float HeatDispersalDelay { get => heatDispersalDelay; set => heatDispersalDelay = value; }
	
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

	public bool CanFire(float heatIncrement = 3f) {
		return Heat + heatIncrement <= maxHeatLevel;
	}

	public void Fire(float heatIncrement = 3f) {
		if (!CanFire(heatIncrement)) throw new System.ArgumentException("fuck you, check if firing is allowed first");
		else {
			Heat += heatIncrement;
			heatDispersalStartTime = 0f;
			DebugManager.UpdateRows("Afterburner" + gameObject.GetInstanceID(), new int[] { 0, 1 }, Heat.ToString(), maxHeatLevel.ToString());
		}
	}

}