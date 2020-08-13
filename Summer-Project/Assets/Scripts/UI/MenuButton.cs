using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler {

	[SerializeField] private GameObject panel;
	[SerializeField] private MenuButton[] panelButtons;
	public bool interactable = true;

	public void OnEnable() {
		interactable = true;
		gameObject.GetComponent<Button>().enabled = true;
	}

	public void OnPointerClick(PointerEventData eventData) {
		if (interactable == true) {
			transform.root.gameObject.GetComponent<MenuScript>().PlayAudio(1, 1f);
			panel.GetComponent<Berp>().ReverseBerp();
		}
		foreach (MenuButton button in panelButtons) {
			button.interactable = false;
			button.GetComponent<Button>().enabled = false;
		}
	}

	public void OnPointerEnter(PointerEventData eventData) {
		if (interactable == true) transform.root.gameObject.GetComponent<MenuScript>().PlayAudio(0, 1f);
	}
}