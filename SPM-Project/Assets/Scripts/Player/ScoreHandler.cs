using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Joakim Linna
public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private float levelParTime = 120.0f;
    [SerializeField] private float multiplierTime;
    
    //private readonly string scoreKey = "score";

    private float score;
    private int multiplier = 1;
    private float multiplierStartTime;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        //LoadScore();
        

        EventSystem.Current.RegisterListener<EnemyDeathEvent>(OnEnemyDied);
        EventSystem.Current.RegisterListener<LevelEndEvent>(OnLevelEnd);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //temp
        levelParTime = scene.buildIndex * 120.0f;
    }

    void FixedUpdate()
    {
        DisplayScore();
    }

    private void Update()
    {
        if(multiplier != 1 && Time.time - multiplierStartTime > (multiplierTime / multiplier))
        {
            //ex: go from 3x to 1x
            //multiplier = 1;

            //ex: go from 3x to 2x
            multiplier--;
            multiplierStartTime = Time.time;
        }
    }

    private void OnEnemyDied(Event e)
    {
        EnemyDeathEvent ede = e as EnemyDeathEvent;

        score += ede.ScoreValue * multiplier;
        multiplier++;
        multiplierStartTime = Time.time;
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

    private void DisplayScore() 
    { 
        scoreText.text = "Score: " + score;
        multiplierText.text = multiplier + "X";
    }
}
