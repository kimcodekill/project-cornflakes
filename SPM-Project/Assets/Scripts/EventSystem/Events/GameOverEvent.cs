using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

public abstract class GameOverEvent : Event
{
    
}

public class LevelEndEvent : GameOverEvent
{
    public int NextLevel;
    public float EndTime;
}

public class PlayerDeadEvent : GameOverEvent
{

}

public class ObjectiveFailedEvent : GameOverEvent
{

}
