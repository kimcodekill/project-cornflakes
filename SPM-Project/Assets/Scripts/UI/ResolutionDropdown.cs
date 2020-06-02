using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ResolutionDropdown : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

	[SerializeField] private Sprite[] sprites;
	[SerializeField] private GameObject mouseContainer;
	private bool dropdownOpen = false;
	
	public void OnPointerDown(PointerEventData eventData) {
		transform.root.gameObject.GetComponent<MenuScript>().PlayAudio(1, 1f);
		gameObject.GetComponent<Image>().sprite = sprites[2];
	}

	public void OnPointerUp(PointerEventData eventData) {
		gameObject.GetComponent<Image>().sprite = sprites[1];
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (dropdownOpen == false) {
			transform.root.gameObject.GetComponent<MenuScript>().PlayAudio(0, 1f);
			gameObject.GetComponent<Image>().sprite = sprites[1];
		}
	}

	public void OnPointerExit(PointerEventData eventData) {
		if (dropdownOpen == false) gameObject.GetComponent<Image>().sprite = sprites[0];
	}

	public void Update() {
		if (transform.childCount == 4) {
			dropdownOpen = true;
			if (mouseContainer.GetComponent<CanvasGroup>().alpha > 0) mouseContainer.GetComponent<CanvasGroup>().alpha -= 0.1f;
		}
		else {
			if (dropdownOpen == true) {
				gameObject.GetComponent<Image>().sprite = sprites[0];
			}
			dropdownOpen = false;
			if (mouseContainer.GetComponent<CanvasGroup>().alpha < 1) mouseContainer.GetComponent<CanvasGroup>().alpha += 0.05f;
		}
	}
}