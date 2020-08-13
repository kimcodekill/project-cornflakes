using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentalOneshots : MonoBehaviour {

	[SerializeField] private AudioClip[] audioClips;
	private AudioSource currentlyPlaying;
	private List<Transform> children = new List<Transform>();

    private int minSoundDelay = 5;
    private int maxSoundDelay = 10;

	private void Start() {
		foreach (Transform child in transform) {
			children.Add(child);
		}
		currentlyPlaying = children[0].GetComponent<AudioSource>();
	}

	private void Update() {
		if (currentlyPlaying.isPlaying == false) {
			currentlyPlaying.transform.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
			currentlyPlaying = transform.GetChild(Random.Range(0, children.Count)).GetComponent<AudioSource>();
			currentlyPlaying.clip = audioClips[Random.Range(0, audioClips.Length)];
			//audioSource.volume = Random.Range(0.1f, 0.4f);
			currentlyPlaying.PlayDelayed(Random.Range(minSoundDelay, maxSoundDelay));
			currentlyPlaying.transform.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
		}
	}
}