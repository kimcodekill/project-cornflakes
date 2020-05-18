using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SliderVolume : MonoBehaviour {

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string exposedParameter;
    private Slider slider;

    public void Start() {
        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        slider.value = PlayerPrefs.GetFloat(exposedParameter, 1f);
    }

    public void ValueChangeCheck() {
        audioMixer.SetFloat(exposedParameter, Mathf.Log10(slider.value) * 20);
        PlayerPrefs.SetFloat(exposedParameter, slider.value);
    }
}