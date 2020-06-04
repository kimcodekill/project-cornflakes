using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A script that will play an audio once on trigger and only once. 

public class AudioPlayOnce : MonoBehaviour
{
    public AudioClip SoundClip;
    //public float Volume;
    private AudioSource audio;
    private bool played = false;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            if (!played)
            {
                audio.PlayOneShot(SoundClip);
                played = true;
            }
        }
    }
}
