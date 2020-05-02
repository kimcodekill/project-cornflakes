using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler
{
	[SerializeField] private AudioClip[] audioClips = new AudioClip[2];
	private AudioSource audioSource;

	private void Start() {
		audioSource = GetComponent<AudioSource>();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		audioSource.PlayOneShot(audioClips[0], 1);
	}
}
