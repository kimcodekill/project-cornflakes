using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private float levelParTime = 120.0f;
    [SerializeField] private string scoreKey = "score";

    private float score;

    void Start()
    {
        //LoadScore();

        EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnEnemyDied);
        EventSystem.Current.RegisterListener<LevelEndEvent>(OnLevelEnd);
    }

    void FixedUpdate()
    {
        DisplayScore();
    }

    private void OnEnemyDied(Event e)
    {
        EnemyDeathEvent ede = e as EnemyDeathEvent;

        score += ede.ScoreValue;
    }

    private void OnLevelEnd(Event e)
    {
        LevelEndEvent lee = e as LevelEndEvent;

        Debug.Log(lee.EndTime);

        //Dont know what the calc here should be, we'll have to implement the enemies first
        //to see what feels good
        score -=  score / (levelParTime - lee.EndTime);

        //SaveScore();
    }

    private void DisplayScore() { scoreText.text = "Score: " + score; }
}
