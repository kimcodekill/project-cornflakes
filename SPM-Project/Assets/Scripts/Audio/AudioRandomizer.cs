using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioRandomizer : MonoBehaviour
{
    private AudioSource source;

    public AudioClip[] SoundClipArray;
    private int clipIndex;

    public float minDelay;
    public float maxDelay;
    private float timeDelay;

    public float volLowRange;
    public float volHighRange;
    public float lowPitchRange;
    public float highPitchRange;

    void Start()
    {
        source = GetComponent<AudioSource>();
        //StartDelay();
        timeDelay = 1;
    }

    void Update()
    {
        if (!source.isPlaying)
        {
            timeDelay -= Time.deltaTime;
            if (timeDelay < 0)
            {
                clipIndex = Random.Range(0, SoundClipArray.Length);
                AudioClip clip = SoundClipArray[clipIndex];
                SoundClipArray[clipIndex] = SoundClipArray[0];
                SoundClipArray[0] = clip;
                source.pitch = Random.Range(lowPitchRange, highPitchRange);
                source.volume = Random.Range(volLowRange, volHighRange);
                source.PlayOneShot(clip);
                StartDelay();
            }
        }
    }

    void StartDelay()
    {
        timeDelay = Random.Range(minDelay, maxDelay);
    }

}