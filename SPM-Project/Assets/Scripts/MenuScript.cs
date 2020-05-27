using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	//K: THIS SHOULD NOT FIRE A LEVEL END EVENT
	//   bad.
	public void NewGame() {
		EventSystem.Current.FireEvent(new LevelEndEvent(-1, Time.time));
	}
}
