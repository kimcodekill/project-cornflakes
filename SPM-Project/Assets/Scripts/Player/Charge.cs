using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Charge : MonoBehaviour {

	[SerializeField] private float rechargeTime;
	[SerializeField] private Slider slider;

	private float startTime = -1f;

	private void Start() {
		DebugManager.AddSection("Charge", "");
		slider.maxValue = rechargeTime;
	}

	private void Update() {
		if (!IsReady()) {
			float elapsed = Time.time - startTime;
			DebugManager.UpdateAll("Charge", "Time remaining: " + (rechargeTime - (elapsed)));
			if (slider.transform.localScale == Vector3.zero) ToggleUI(true);
			slider.value = elapsed;
		}
		else ToggleUI(false);
	}

	public void Trigger() {
		startTime = Time.time;
	}

	public bool IsReady() {
		return startTime == -1f || Time.time - startTime >= rechargeTime;
	}

	private void ToggleUI(bool enabled) {
		if (enabled) slider.transform.localScale = Vector3.one;
		else slider.transform.localScale = Vector3.zero;
	}

}