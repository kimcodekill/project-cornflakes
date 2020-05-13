using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventExample : MonoBehaviour {

	[SerializeField] private Audio someShitIWannaPlay;

	private void OnTriggerEnter(Collider other) {
		EventSystem.Current.FireEvent(new SFXEvent() {
			Audio = someShitIWannaPlay
		});
	}

}