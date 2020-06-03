using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{

    [SerializeField] private GameObject fadeImage;

    void Start()
    {
        fadeImage.SetActive(true);
        fadeImage.GetComponent<Image>().CrossFadeAlpha(0f, 10.0f, true);
        fadeImage.SetActive(false);
    }

}
