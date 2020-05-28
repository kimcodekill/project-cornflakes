using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameEndHandler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] GameObject highScorePanel;
    [SerializeField] TMP_InputField nameInput;

    float score;
    float prevScore;

    void OnEnable()
    {
        prevScore = PlayerPrefs.GetFloat("scoreValue", 0f);
        score = PlayerHud.Instance != null ? PlayerHud.Instance.ScoreHandler.Score : 0;

        scoreText.text = "Score: " + score;

        highScorePanel.SetActive(prevScore < score);

        if (PlayerController.Instance != null) PlayerController.Instance.gameObject.SetActive(false);

        CursorVisibility.Instance.EnableCursor();
    }

    public void SaveNew()
    {
        PlayerPrefs.SetString("scoreName", nameInput.text);
        PlayerPrefs.SetFloat("scoreValue", score);

        MainMenu();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
