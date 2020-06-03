using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour {

    public AudioMixer audioMixer;
    public string exposedParameter;
    [HideInInspector] public Slider slider;
    
    public void Awake() {
        MenuScript rootMenu = transform.root.GetComponent<MenuScript>();
        if (rootMenu != null) {rootMenu.volumeSliders.Add(this); }

        slider = gameObject.GetComponent<Slider>();
        slider.onValueChanged.AddListener(delegate { ValueChangeCheck(); });
        SetInitialValue();
    }

    public void SetInitialValue() {
		slider.value = PlayerPrefs.GetFloat(exposedParameter, 1f);
		audioMixer.SetFloat(exposedParameter, Mathf.Log10(slider.value) * 20);
    }

    public void ValueChangeCheck() {
        audioMixer.SetFloat(exposedParameter, Mathf.Log10(slider.value) * 20);
        PlayerPrefs.SetFloat(exposedParameter, slider.value);
    }
}