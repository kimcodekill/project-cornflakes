using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	private AudioSource audiosource;

	public void NewGame() {
		audiosource = GetComponent<AudioSource>();
		audiosource.PlayOneShot(audiosource.clip, 1);
		EventSystem.Current.FireEvent(new LevelEndEvent()
		{
			Description = "Starting New Game"
		});
	}
}
