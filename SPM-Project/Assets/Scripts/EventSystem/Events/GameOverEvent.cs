using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

public abstract class GameOverEvent : Event
{
    
}

public class LevelEndEvent : GameOverEvent
{
    public LevelEndEvent(int nextLevel, float endTime)
    {
        EndTime = endTime;
        NextLevel = nextLevel;
    }

    public int NextLevel { get; }
    public float EndTime { get; }
}

public class PlayerDeadEvent : GameOverEvent
{

}

public class ObjectiveFailedEvent : GameOverEvent
{

}
