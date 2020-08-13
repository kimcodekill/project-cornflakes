using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Author: Joakim Linna
public class ScoreHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI multiplierText;
    [SerializeField] private float levelParTime = 120.0f;
    [SerializeField] private float levelBonusScale = 10.0f;
    [SerializeField] private float multiplierTime;
    
    //private readonly string scoreKey = "score";

    private int multiplier = 1;
    private float multiplierStartTime;

    public float StartTime { get; set; }

    public float Score { get; set; }

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
        if (!scene.name.Contains("Menu") && !scene.path.Contains("Dev"))
        {
            StartTime = Time.time;
            string path = "Scene/vars" + scene.name;
            SceneVars currentSceneVars = Resources.Load(path) as SceneVars;
            levelParTime = currentSceneVars.GetParTime();
        }
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

        Score += ede.ScoreValue * multiplier;
        multiplier++;
        multiplierStartTime = Time.time;
    }

    private void OnLevelEnd(Event e)
    {
        LevelEndEvent lee = e as LevelEndEvent;

        //this seems decent enough
        // ex: timeBonus = 975 / (124.68 - 98.18) * 10 == 367.90f
        float timeBonus = Score / (levelParTime - (lee.EndTime - StartTime)) * levelBonusScale;
        Score += timeBonus > 0 ? timeBonus : 0;

        string parTimeString = string.Format("Level: {0} | Time: {1}", SceneManager.GetActiveScene().name, lee.EndTime - StartTime);
        //Debug.Log(parTimeString);

        //SaveScore();
    }

    private void DisplayScore() 
    { 
        scoreText.text = "Score: " + Score;
        multiplierText.text = multiplier + "X";
    }

    private void OnDestroy()
    {
        EventSystem.Current.UnRegisterListener<EnemyDeathEvent>(OnEnemyDied);
        EventSystem.Current.UnRegisterListener<LevelEndEvent>(OnLevelEnd);
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
