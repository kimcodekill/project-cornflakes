using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ReverbZone : MonoBehaviour {

    //[SerializeField] private AudioReverbPreset reverbPreset;
    [SerializeField] private AudioMixerSnapshot reverbSnapshot;
    [SerializeField] private AudioMixerSnapshot defaultSnapshot;

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerCamera.Instance.reverbZoned++;
            //other.gameObject.GetComponentInChildren<AudioReverbFilter>().reverbPreset = reverbPreset;
            //other.gameObject.GetComponentInChildren<AudioReverbFilter>().enabled = true;
            reverbSnapshot.TransitionTo(2);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            PlayerCamera.Instance.reverbZoned--;
            //if (other.gameObject.GetComponentInChildren<PlayerCamera>().reverbZoned == 0) other.gameObject.GetComponentInChildren<AudioReverbFilter>().enabled = false;
            if (PlayerCamera.Instance.reverbZoned == 0) defaultSnapshot.TransitionTo(2);
        }
    }
}