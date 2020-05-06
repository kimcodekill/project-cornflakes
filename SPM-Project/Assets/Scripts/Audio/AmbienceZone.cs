using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceZone : MonoBehaviour {
    
    //[SerializeField] private float volume;
    private AudioSource parentAudioSource;
    private AudioSource audioSource;

    public void Start() {
        audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.volume = 0.2f;
        parentAudioSource = transform.parent.gameObject.GetComponentInParent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && audioSource.isPlaying == false) {
            audioSource.time = Random.Range(0, audioSource.clip.length);
            audioSource.Play();
            parentAudioSource.Stop();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            audioSource.Stop();
            parentAudioSource.time = Random.Range(0, parentAudioSource.clip.length);
            parentAudioSource.Play();
        }
    }
}