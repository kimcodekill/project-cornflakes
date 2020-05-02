using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioZone : MonoBehaviour {
    
    [SerializeField] private AudioSource parentAudioSource;

    private AudioSource audioSource;

    public void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player") && audioSource.isPlaying == false) {
            audioSource.Play();
            parentAudioSource.Stop();
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            audioSource.Stop();
            parentAudioSource.Play();
        }
    }
}