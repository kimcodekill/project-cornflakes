using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameOverEvent : Event
{
    
}

public class LevelEndEvent : GameOverEvent
{
    public int NextLevel;
}

public class PlayerDeadEvent : GameOverEvent
{

}

public class ObjectiveFailedEvent : GameOverEvent
{

}
