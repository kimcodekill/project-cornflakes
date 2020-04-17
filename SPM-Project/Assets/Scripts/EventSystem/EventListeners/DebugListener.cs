using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugListener : MonoBehaviour {

	private void Start() => EventSystem.Current.RegisterListener<HitEvent>(OnEvent);

	private void OnEvent(Event e) {
		Debug.Log(e.Description);
	}

}