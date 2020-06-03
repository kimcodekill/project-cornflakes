using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GamePaused;

    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject exitWarning;

    private void Start()
    {
#if UNITY_EDITOR
        CursorVisibility.Instance.IgnoreInput = true;
#endif

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
        if(GamePaused) { Resume(); }
        else { Pause(); }
    }

    private void Pause()
    {
        CursorVisibility.Instance.EnableCursor();
        Time.timeScale = 0f;
        pauseMenuPanel.SetActive(true);
        GamePaused = true;
    }

    private void Resume()
    {
        CursorVisibility.Instance.DisableCursor();
        Time.timeScale = 1f;
        pauseMenuPanel.SetActive(false);
        GamePaused = false;
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

    public void ExitToMenu()
    {
        Resume();
        SceneManager.LoadScene(0);
    }

    public void ExitToDesktop()
    {
#if UNITY_EDITOR
        Debug.Log("Exited to desktop");
#endif
        Application.Quit();
    }
}