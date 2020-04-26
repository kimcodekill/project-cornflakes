using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void NextScene()
    {
        Debug.Log("something");
        EventSystem.Current.FireEvent(new LevelEndEvent() {
        Description = "PlayerEndedLevel",
        NextLevel = 1 });
    }
}
