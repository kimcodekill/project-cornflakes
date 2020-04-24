using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverListener : MonoBehaviour
{
    public static string RootSceneName;

    //Would rather not have this as a string but not quite sure what i want either
    [SerializeField] private string nextSceneName;
    
    [Header("Unused, check script")]
    [SerializeField] private Scene inspectorSceneExample; //this is unused due to me not knowing what the hell the inspector wants. I'll look at it in the future.

    private void OnEnable()
    {
        // I bet there's a way to deal with Scenemanagement by actually using scenes but idk it seems messy.
        if (RootSceneName == null) { RootSceneName = SceneManager.GetSceneAt(0).name; }
    }

    void Start()
    {
        EventSystem.Current.RegisterListener<LevelEndEvent>(OnLevelEnd);
        EventSystem.Current.RegisterListener<PlayerDeadEvent>(OnPlayerDead);
        EventSystem.Current.RegisterListener<ObjectiveFailedEvent>(OnObjectiveFailed);
    }

    private void OnLevelEnd(Event e)
    {
        
        Debug.Log("Level ended, start transition to next scene");
        Debug.Log(e.Description);

        //Not sure if this should be done here, probably should be sent to some other class that likes taking care of scenes
        LoadNextScene();
    }

    private void OnPlayerDead(Event e)
    {
        Debug.Log("Player is dead, start Respawn logic");

        ReloadCurrentScene();
    }

    private void OnObjectiveFailed(Event e)
    {
        Debug.Log("Objective failed, start Respawn logic");

        ReloadCurrentScene();
    }

    private void LoadNextScene()
    {
        if (!nextSceneName.Equals(string.Empty)) { SceneManager.LoadScene(nextSceneName); }
        else                                     { SceneManager.LoadScene(RootSceneName); }
    }

    //This just wraps the code inside it for prettifyness
    private void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
