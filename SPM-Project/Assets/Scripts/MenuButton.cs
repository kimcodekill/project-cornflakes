using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{

	[SerializeField] private AudioClip[] audioClips = new AudioClip[2];
	private AudioSource audioSource;
	private Button theButton;

	private void Start() {
		audioSource = GetComponent<AudioSource>();
		theButton = GetComponent<Button>();
		theButton.onClick.AddListener(ClickButton);
	}

	public void OnPointerEnter(PointerEventData eventData) {
		audioSource.PlayOneShot(audioClips[0], 1);
	}

	public void ClickButton() {
		audioSource.PlayOneShot(audioClips[1], 1);
	}
}