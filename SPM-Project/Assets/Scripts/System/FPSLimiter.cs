using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class FPSLimiter : MonoBehaviour {

	[SerializeField] [Tooltip("The FPS limit")] private int FPS;
	[Header("Press F to toggle limit")]
	[SerializeField] private bool limitEnabled = false;

	void Awake() {
		QualitySettings.vSyncCount = 0;
	}

	private void Update() {
		if (Input.GetKeyDown(KeyCode.F)) limitEnabled = !limitEnabled;
		if (limitEnabled) Application.targetFrameRate = FPS;
		else Application.targetFrameRate = -1;
	}

}