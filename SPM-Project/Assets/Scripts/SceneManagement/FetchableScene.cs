using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "Fetchable Scene")]
public class FetchableScene : ScriptableObject
{
    public string SceneName;

    public Scene Scene { get { return SceneManager.GetSceneByName(SceneName); } }
}
