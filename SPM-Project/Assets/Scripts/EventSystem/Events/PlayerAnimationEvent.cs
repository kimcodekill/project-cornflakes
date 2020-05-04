using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author Joakim Linna

//I dont know what is inherently different between the different animation events,
//but they are separated like this so it's easier to handle.
//If anything is needed in the listener, add variables for it here.
public abstract class PlayerAnimationEvent : Event
{
    
}

public class PlayerJumpEvent : PlayerAnimationEvent
{

}

public class PlayerDashEvent : PlayerAnimationEvent
{

}