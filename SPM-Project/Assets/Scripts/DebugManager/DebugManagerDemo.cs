using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class DebugManagerDemo : MonoBehaviour {

	private string debugSectionName1, debugSectionName2, debugSectionName3;

	private float someFloatIWantToTrack;
	private float someIntIWantToTrack;
	private bool someBoolIWantToTrack1;
	private bool someBoolIWantToTrack2 = true;

	void Start() {

		//If you haven't already, make sure to put a single instance of the DebugManager prefab into your scene hierarchy (that is, if you actually want to use the DebugManager).

		//If you are going to use multiple instances of a debugmanaged gameobject, make sure to make each section header uniquely identifiable,
		//such as using gameObject.GetInstanceID() or someUniqueValue.GetHashCode().
		debugSectionName1 = "MyDebugSectionAboutFloats" + gameObject.GetInstanceID();
		debugSectionName2 = "MyDebugSectionAboutInts" + gameObject.GetInstanceID();
		debugSectionName3 = "MyDebugSectionAboutBools" + gameObject.GetInstanceID();

		//Add the sections to the Debug Manager, with the desired lines. If you want to reserve lines, do so by adding an empty string in the parameter list.
		DebugManager.AddSection(debugSectionName1, someFloatIWantToTrack.ToString());
		DebugManager.AddSection(debugSectionName2, "placeholder", "", someIntIWantToTrack.ToString());
		DebugManager.AddSection(debugSectionName3, someBoolIWantToTrack1.ToString(), someBoolIWantToTrack2.ToString());
	
	}

	void Update() {

		//Update the desired row
		someFloatIWantToTrack += Time.deltaTime;
		DebugManager.UpdateRow(debugSectionName1, someFloatIWantToTrack.ToString());

		someIntIWantToTrack += 1;
		DebugManager.UpdateRow(debugSectionName2, 2, someIntIWantToTrack.ToString());

		//Or the desired rows
		someBoolIWantToTrack1 = someBoolIWantToTrack1 ? false : true;
		someBoolIWantToTrack2 = someBoolIWantToTrack2 ? false : true;
		DebugManager.UpdateRows(debugSectionName3, new int[] { 0, 1 }, someBoolIWantToTrack1.ToString(), someBoolIWantToTrack2.ToString());

		//If you have any questions or find any bugs, feel free to @ me (frog) in Discord.
		//If you do encounter some exceptions, they are usually because you updated a nonexistent row, or tried to add a duplicate section header.
	
	}
}
