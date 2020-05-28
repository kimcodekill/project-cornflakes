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
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        startTime = Time.time;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        string parTimeString = string.Format("Level: {0} | Time: {1}", scene.name, Time.time - startTime);
        Debug.Log(parTimeString);
    }
}
