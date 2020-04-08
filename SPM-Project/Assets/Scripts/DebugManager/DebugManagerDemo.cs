using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManagerDemo : MonoBehaviour {

	private readonly string debugSectionName1 = "MyDebugSectionAboutFloats";
	private readonly string debugSectionName2 = "MyDebugSectionAboutInts";
	private readonly string debugSectionName3 = "MyDebugSectionAboutBools";

	private float someFloatIWantToTrack;
	private float someIntIWantToTrack;
	private bool someBoolIWantToTrack;

	void Start() {
		DebugManager.AddSection(debugSectionName1, someFloatIWantToTrack.ToString());
		DebugManager.AddSection(debugSectionName2, "placeholder", null, someIntIWantToTrack.ToString());
		DebugManager.AddSection(debugSectionName3, someBoolIWantToTrack.ToString());
	}

	void Update() {
		someFloatIWantToTrack += Time.deltaTime;
		DebugManager.UpdateRow(debugSectionName1, someFloatIWantToTrack.ToString());

		someIntIWantToTrack += 1;
		DebugManager.UpdateRow(debugSectionName2, 2, someIntIWantToTrack.ToString());

		someBoolIWantToTrack = someBoolIWantToTrack ? false : true;
		DebugManager.UpdateRow(debugSectionName3, someBoolIWantToTrack.ToString());
	}
}
