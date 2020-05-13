using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Elevator : MonoBehaviour
{
    public Transform elevatorTarget;
    public GameObject playerMech;
    private AudioSource audioSource;
    public AudioClip elevatorClip;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = elevatorClip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>())
        {
            StartCoroutine(ElevatorTransition());
        }
    }
    private IEnumerator ElevatorTransition()
    {
        Time.timeScale = 0f;
        GameObject fadeImage;
        fadeImage = new GameObject("fadeImage");
        fadeImage.AddComponent<Image>().color = new Color(0, 0, 0, 0.01f);
        fadeImage.transform.parent = GameObject.Find("PlayerHud").transform;
        fadeImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        fadeImage.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, Screen.height);
        fadeImage.GetComponent<Image>().CrossFadeAlpha(255f, 1.0f, true);
        audioSource.Play();
        yield return new WaitForSecondsRealtime(4);
        playerMech.transform.position = elevatorTarget.transform.position;
        Time.timeScale = 1f;
        fadeImage.GetComponent<Image>().CrossFadeAlpha(1f, 1.0f, true);
        yield return new WaitForSecondsRealtime(1);
        Destroy(fadeImage);
    }
}
