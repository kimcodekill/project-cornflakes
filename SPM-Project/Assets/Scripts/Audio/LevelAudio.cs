using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelAudio : MonoBehaviour {

    [SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] audioClips = new AudioClip[7];

    private int minSoundDelay = 5;
    private int maxSoundDelay = 10;
	protected void Update() {
		if (audioSource.isPlaying == false) {
			audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
			audioSource.volume = Random.Range(0.1f, 0.4f);
			audioSource.PlayDelayed(Random.Range(minSoundDelay, maxSoundDelay));
		}
	}
}