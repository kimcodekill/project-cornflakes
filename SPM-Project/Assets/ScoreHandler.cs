using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;

    private float score;

    void Start()
    {
        EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnEnemyDied);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        DisplayScore();
    }

    private void OnEnemyDied(Event e)
    {
        EnemyDeathEvent ede = e as EnemyDeathEvent;

        score += ede.ScoreValue;
    }

    private void DisplayScore() { scoreText.text = "Score: " + CalculateScore(); }

    private float CalculateScore() { return score; }
}
