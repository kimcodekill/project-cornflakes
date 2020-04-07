using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManagerDemo : MonoBehaviour {

	private readonly string debugSectionName1 = "MyDebugSectionAboutFloats";
	private readonly string debugSectionName2 = "MyDebugSectionAboutInts";

	private float someFloatIWantToTrack;
	private float someIntIWantToTrack;

	void Start() {
		DebugManager.AddSection(debugSectionName1, someFloatIWantToTrack.ToString());
		DebugManager.AddSection(debugSectionName2, "placeholder", null, someIntIWantToTrack.ToString());
	}

	void Update() {
		someFloatIWantToTrack += Time.deltaTime;
		DebugManager.UpdateRow(debugSectionName1, 0, someFloatIWantToTrack.ToString());

		someIntIWantToTrack += 1;
		DebugManager.UpdateRow(debugSectionName2, 2, someIntIWantToTrack.ToString());
	}
}
