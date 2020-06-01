using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	[SerializeField] private AudioMixer audioMixer;
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private AudioClip[] audioClips = new AudioClip[2];
	[SerializeField] private GameObject[] panels;
	[SerializeField] private Toggle muteAllToggle;
	[SerializeField] private Toggle fullscreenToggle;
	[SerializeField] private TMP_Dropdown resolutionDropdown;
	[SerializeField] private Slider mouseSlider;
	[SerializeField] private TextMeshProUGUI highScoreText;

	public Color color;

	private Resolution[] resolutions;
	private List<Resolution> filteredResolutions = new List<Resolution>();
	private int savedResolution = 0;
	private bool isFullscreen = true;
	private float highScore;
	private string name;

	public void Start() {

		panels[0].SetActive(true);
		panels[1].SetActive(false);
		panels[2].SetActive(false);

		if (PlayerPrefs.GetFloat("masterVolume") == -80) muteAllToggle.isOn = true;
		savedResolution = PlayerPrefs.GetInt("savedResolution");

		resolutions = Screen.resolutions.Reverse().ToArray();

		List<string> options = new List<string>();
		int dropdownValue = 0;

		for (int i = 0; i < resolutions.Length; i++) {
			if (resolutions[i].width > Screen.currentResolution.width / 2 && resolutions[i].height > Screen.currentResolution.height / 1.5) {
				filteredResolutions.Add(resolutions[i]);
				string option = resolutions[i].width + " x " + resolutions[i].height;
				options.Add(option);
				
				if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height) {
					dropdownValue = filteredResolutions.Count - 1;
					resolutionDropdown.RefreshShownValue();
				}
			}
		}

		resolutionDropdown.ClearOptions();
		resolutionDropdown.AddOptions(options);
		resolutionDropdown.value = dropdownValue;

		if (Screen.fullScreen == false) {
			fullscreenToggle.isOn = false;
			isFullscreen = false;
		}
		mouseSlider.value = PlayerPrefs.GetFloat("mouseSensitivity", 1) * 5;
	}

	public void SwitchPanel(int index)
	{
		panels[index].SetActive(true);
	}

	//K: THIS SHOULD NOT FIRE A LEVEL END EVENT
	//   bad.
	public void NewGame() {
		CheckPointSystem.NewGame = true;
		EventSystem.Current.FireEvent(new LevelEndEvent(-1, Time.time));
	}

	public void ToggleMuteAll(bool toggleValue) {
		if (toggleValue == true) {
			audioMixer.SetFloat("masterVolume", -80);
			PlayerPrefs.SetFloat("masterVolume", -80);
		}
		else {
			audioMixer.SetFloat("masterVolume", 0);
			PlayerPrefs.SetFloat("masterVolume", 0);
			if (panels[0].gameObject.activeInHierarchy == false) PlayAudio(1, 1);
		}
	}

	public void SetResolution(int index) {
		Resolution resolution = filteredResolutions[index];
		Screen.SetResolution(resolution.width, resolution.height, isFullscreen);
		if (isFullscreen == false) {
			savedResolution = index;
			PlayerPrefs.SetInt("savedResolution", savedResolution);
			resolutionDropdown.value = savedResolution;
		}
		if (resolutionDropdown.transform.childCount == 4) PlayAudio(1, 1);
	}

	public void ToggleFullscreen(bool toggleValue) {
		isFullscreen = toggleValue;
		Screen.fullScreen = isFullscreen;

		if (isFullscreen == true) {
			SetResolution(0);
			resolutionDropdown.value = 0;
		}
		else SetResolution(savedResolution);
		if (panels[0].gameObject.activeInHierarchy == false) PlayAudio(1, 1);
	}

	public void MouseSensitivity(float sliderValue) {
		PlayerPrefs.SetFloat("mouseSensitivity", sliderValue * 0.2f);
	}

	public void ExitGame() {
		Application.Quit();
	}

	public void PlayAudio(int clipIndex, float volume) {
		audioSource.PlayOneShot(audioClips[clipIndex], volume);
	}

	private void OnEnable() {
		name = PlayerPrefs.GetString("scoreName", string.Empty);
		highScore = PlayerPrefs.GetFloat("scoreValue", 0);

		if (!name.Equals(string.Empty) && highScore != 0) {
			highScoreText.text = string.Format("High Score: {0} - {1}", name, (int)highScore);
		}
		else {
			highScoreText.gameObject.SetActive(false);
		}
	}
}