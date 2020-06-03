using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool GameRunning;

    [Header("Panels")]
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [Obsolete("No current GameObject. Will throw nullref")]
    [SerializeField] private GameObject exitWarning; 
    [SerializeField] private GameObject playerHudPanel;
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Toggle muteToggle;
    [SerializeField] private AudioClip buttonPressClip;
    [SerializeField] private AudioClip buttonHoverClip;
    [Header("Controls")]
    [SerializeField] private Slider mouseSensitivity;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.ignoreListenerPause = true;

        CursorVisibility.Instance.IgnoreInput = true;
        muteToggle.SetIsOnWithoutNotify(PlayerPrefs.GetFloat("masterVolume", 0f) == -80f);
        mouseSensitivity.SetValueWithoutNotify(PlayerPrefs.GetFloat("mouseSensitivity", 1f));

        Resume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if(GameRunning) { Pause(); }
        else { Resume(); }
    }

    private void Pause()
    {
        AudioListener.pause = true;
        CursorVisibility.Instance.EnableCursor();
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        playerHudPanel.SetActive(false);
        GameRunning = false;
    }

    private void Resume()
    {
        AudioListener.pause = false;
        CursorVisibility.Instance.DisableCursor();
        Time.timeScale = 1f;
        settingsMenuPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        playerHudPanel.SetActive(true);
        GameRunning = true;
    }

    public void ToggleSettingsMenu()
    {
        settingsMenuPanel.SetActive(!settingsMenuPanel.activeSelf);
    }

    public void ToggleExitWarning(string title)
    {
        TextMeshProUGUI warningTitle = exitWarning.GetComponent<TextMeshProUGUI>();
        warningTitle.text = "Exit to " + title;

    }

    public void SetSensitivity(float sensitivity)
    {
        PlayerPrefs.SetFloat("mouseSensitivity", sensitivity);
    }

    public void SetAudioMuted(bool mute)
    {
        audioMixer.SetFloat("masterVolume", mute ? -80 : 0);
        PlayerPrefs.SetFloat("masterVolume", mute ? -80 : 0);
    }

    public void ExitToMenu()
    {
        if (PlayerController.Instance != null) Destroy(PlayerController.Instance.gameObject);
        CursorVisibility.Instance.EnableCursor();
        Time.timeScale = 1f;
        GameRunning = true;
        SceneManager.LoadScene(0);
    }

    public void ExitToDesktop()
    {
#if UNITY_EDITOR
        Debug.Log("Exited to desktop");
#endif
        Application.Quit();
    }

    public void PlayButtonHover()
    {
        audioSource.PlayOneShot(buttonHoverClip);
    }

    public void PlayButtonClick()
    {
        audioSource.PlayOneShot(buttonPressClip);
    }
}