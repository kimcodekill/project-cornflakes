using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] Button btn;
	[SerializeField] Canvas menu;
	bool gamePaused = false;

	private void Awake() {
		menu.gameObject.SetActive(false);
	}

	private void Update() {
		if (gamePaused && Input.GetKeyDown(KeyCode.Escape)) UnPauseGame();
	}

	public void PauseGame() {
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0;
		menu.gameObject.SetActive(true);
		gamePaused = true;

	}

	public void UnPauseGame() {
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1;
		menu.gameObject.SetActive(false);
		gamePaused = false;
	}

	public void EndGame() {
		//UnityEditor.EditorApplication.isPlaying = false;
		Application.Quit();
	}
}
