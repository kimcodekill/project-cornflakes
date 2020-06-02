using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroSequence : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private GameObject fadeImage;
    [SerializeField] private AudioClip introClip;
    [SerializeField] private Text dialogText;
    [SerializeField] private GameObject dialogBox;
    //[SerializeField] [TextArea] private string DialogText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introClip;
        StartCoroutine(IntroCoroutine());
        dialogBox.active = false;
        fadeImage.active = true;
    }

    private IEnumerator IntroCoroutine()
    {
        audioSource.Play();
        yield return new WaitForSecondsRealtime(1f);
        fadeImage.GetComponent<Image>().CrossFadeAlpha(0, 5.0f, true);
        yield return new WaitForSecondsRealtime(2f);
        dialogText.GetComponent<Text>().text = "Five years";
        dialogBox.active = true;
        yield return new WaitForSecondsRealtime(1.5f);
        dialogText.GetComponent<Text>().text = "Five long years of war";
        yield return new WaitForSecondsRealtime(2.7f);
        dialogText.GetComponent<Text>().text = "And we're slowly losing";
        yield return new WaitForSecondsRealtime(2f);
        dialogText.GetComponent<Text>().text = "The Scorthaza are ruthless";
        yield return new WaitForSecondsRealtime(2.2f);
        dialogText.GetComponent<Text>().text = "World after world have fallen to their slow but immovable march";
        yield return new WaitForSecondsRealtime(3.8f);
        dialogText.GetComponent<Text>().text = "harvested, and then tossed aside";
        yield return new WaitForSecondsRealtime(2.8f);
        dialogText.GetComponent<Text>().text = "Billions of lives lost";
        yield return new WaitForSecondsRealtime(1.8f);
        dialogText.GetComponent<Text>().text = "For every battle we win, we lose two";
        yield return new WaitForSecondsRealtime(2.9f);
        dialogText.GetComponent<Text>().text = "we tried to hide, we tried to run, we tried to fight";
        yield return new WaitForSecondsRealtime(5f);
        dialogText.GetComponent<Text>().text = "but now they've reached the Inner Colonies";
        yield return new WaitForSecondsRealtime(2.3f);
        //fadeImage.GetComponent<Image>().CrossFadeAlpha(1f, 5.0f, true);
        dialogText.GetComponent<Text>().text = "Arbesus is first on the line";
        yield return new WaitForSecondsRealtime(3f);
        fadeImage.GetComponent<Image>().CrossFadeAlpha(1f, 1.8f, true);
        dialogBox.active = false;
        yield return new WaitForSecondsRealtime(1.8f);
        EventSystem.Current.FireEvent(new LevelEndEvent()
        {
            Description = "Intro sequence finished"
        });
    }

}
