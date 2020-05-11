using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfrqSys = InfrequentUpdateSystem; 

public class InfrequentTest : MonoBehaviour {

	private void Awake() {
		InfrqSys.Current.Register(InfrequentUpdate, 2f);
		InfrqSys.Current.Register(FrequentUpdate, 0.5f);
		InfrqSys.Current.Register(NoMoreUpdates, 10f);
	}

	private void InfrequentUpdate() {
		Debug.Log("I'm an infrequent update!");
	}

	private void FrequentUpdate() {
		Debug.Log("I'm a frequent update!");
	}

	private void NoMoreUpdates() {
		Debug.Log("No more updates!");
		InfrqSys.Current.Unregister(InfrequentUpdate);
		InfrqSys.Current.Unregister(FrequentUpdate);
		InfrqSys.Current.Unregister(NoMoreUpdates);
	}

}