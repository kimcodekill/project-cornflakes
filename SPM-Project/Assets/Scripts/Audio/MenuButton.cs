using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerEnterHandler {

	private Button button;

	public void Start() {
		button = gameObject.GetComponent<Button>();
		button.onClick.AddListener(delegate { ClickButton(); });
	}

	public void OnPointerEnter(PointerEventData eventData) {
		transform.root.gameObject.GetComponent<MenuScript>().PlayAudio(0, 1f);
	}

	public void ClickButton() {
		transform.root.gameObject.GetComponent<MenuScript>().PlayAudio(1, 1f);
	}
}