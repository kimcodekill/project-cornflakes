using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManagerDemo : MonoBehaviour {

	private string debugSectionName1 = "MyDebugSectionAboutFloats";
	private string debugSectionName2 = "MyDebugSectionAboutInts";
	private string debugSectionName3 = "MyDebugSectionAboutBools";

	private float someFloatIWantToTrack;
	private float someIntIWantToTrack;
	private bool someBoolIWantToTrack;

	void Start() {
		debugSectionName1 += gameObject.GetInstanceID();
		debugSectionName2 += gameObject.GetInstanceID();
		debugSectionName3 += gameObject.GetInstanceID();
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
