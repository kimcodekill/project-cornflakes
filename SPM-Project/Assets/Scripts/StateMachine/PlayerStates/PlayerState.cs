using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : State {
	
	private PlayerController player;
	
	public PlayerController Player => player = player != null ? player : (PlayerController) Owner;

}