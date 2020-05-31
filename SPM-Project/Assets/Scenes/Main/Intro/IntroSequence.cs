using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip introClip;
    public Image fadeImage;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introClip;
        StartCoroutine(IntroCoroutine());
    }

    private IEnumerator IntroCoroutine()
    {
        audioSource.Play();
        yield return new WaitForSecondsRealtime(1);
        fadeImage.GetComponent<Image>().CrossFadeAlpha(0, 5.0f, true);
        yield return new WaitForSecondsRealtime(30);
        fadeImage.GetComponent<Image>().CrossFadeAlpha(1f, 3.0f, true);
        yield return new WaitForSecondsRealtime(4);
        EventSystem.Current.FireEvent(new LevelEndEvent()
        {
            Description = "Intro sequence finished"
        });
    }

}
