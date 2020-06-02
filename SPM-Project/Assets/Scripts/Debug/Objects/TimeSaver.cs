using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeSaver : MonoBehaviour
{
    private float startTime;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        EventSystem.Current.RegisterListener<LevelEndEvent>(OnLevelEnd);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startTime = Time.time;
    }

    private void OnLevelEnd(Event e)
    {
        string parTimeString = string.Format("Level: {0} | Time: {1}", SceneManager.GetActiveScene().name, Time.time - startTime);
        Debug.Log(parTimeString);
    }
}
