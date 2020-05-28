using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	[SerializeField] private GameObject mainPanel;
	[SerializeField] private GameObject howToPlayPanel;
	[SerializeField] private GameObject settingsPanel;

	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] audioClips = new AudioClip[2];
	[SerializeField] private TMP_Dropdown resolutionDropdown;
	[SerializeField] private Toggle fullscreenToggle;
	private Resolution[] resolutions;
	private int savedResolution = 0;
	private bool isFullscreen = true;

	public void Start() {
		howToPlayPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(false);

		resolutions = Screen.resolutions.Reverse().ToArray();

		if (Screen.fullScreen == false) {
			fullscreenToggle.isOn = false;
			isFullscreen = false;
		}

		List<string> options = new List<string>();
		int dropdownValue = 0;

		for (int i = 0; i < resolutions.Length; i++) {
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
				dropdownValue = i;
				resolutionDropdown.RefreshShownValue();
			}
		}

		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = dropdownValue;
	}

	public void NewGame() {
		EventSystem.Current.FireEvent(new LevelEndEvent()
		{
			Description = "Starting New Game"
		});
	}

	public void GoToControls() {
		mainPanel.gameObject.SetActive(false);
		howToPlayPanel.gameObject.SetActive(true);
	}

	public void GoToSettings() {
		mainPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(true);
	}

	public void SetResolution(int index) {
		Resolution resolution = resolutions[index];
		Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
		if (isFullscreen == false) {
			savedResolution = index;
			resolutionDropdown.value = savedResolution;
		}
	}

	public void ToggleFullscreen(bool toggleValue) {
		isFullscreen = toggleValue;
		Screen.fullScreen = isFullscreen;

		if (isFullscreen == true) {
			SetResolution(0);
			resolutionDropdown.value = 0;
		}
		else SetResolution(savedResolution);
	}

	public void BackToMain() {
		mainPanel.gameObject.SetActive(true);
		howToPlayPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(false);
	}

	public void ExitGame() {
		Application.Quit();
	}

	public void PlayAudio(int clipIndex, float volume) {
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
	}
}