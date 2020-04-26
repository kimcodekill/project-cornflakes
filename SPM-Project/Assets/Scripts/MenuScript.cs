using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
    public void NewGame()
    {
        EventSystem.Current.FireEvent(new LevelEndEvent()
        {
            Description = "Starting New Game"
        });
    }
}
