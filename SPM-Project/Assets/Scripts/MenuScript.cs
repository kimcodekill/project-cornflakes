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
	private Resolution[] resolutions;

	public void Start() {
		howToPlayPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(false);

		Resolution[] resolutionsTemp = Screen.resolutions;
		resolutions = resolutionsTemp.Reverse().ToArray();

		List<string> options = new List<string>();
		for (int i = 0; i < resolutions.Length; i++) {
			string option = resolutions[i].width + " x " + resolutions[i].height;
			options.Add(option);

			if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
				resolutionDropdown.value = i;
				resolutionDropdown.RefreshShownValue();
			}
		}

		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(options);
	}

	public void NewGame() {
		EventSystem.Current.FireEvent(new LevelEndEvent()
		{
			Description = "Starting New Game"
		});
	}

	public void HowToPlay() {
		mainPanel.gameObject.SetActive(false);
		howToPlayPanel.gameObject.SetActive(true);
	}

	public void Settings() {
		mainPanel.gameObject.SetActive(false);
		settingsPanel.gameObject.SetActive(true);

	}

	public void SetResolution(int index) {
		Resolution resolution = resolutions[index];
		Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
	}

	public void ToggleFullscreen(bool toggleValue)
	{
		Screen.fullScreen = toggleValue;
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