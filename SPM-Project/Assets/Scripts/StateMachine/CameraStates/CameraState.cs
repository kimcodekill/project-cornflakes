using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna
//Secondary author: Erik Pilström

public abstract class CameraState : State
{
	public int CameraFOV;
	public Vector3 CameraOffset;
	public float CameraSensitivity;
	//^^^ Do these need to be public? private Serialized should be enough, no? /E

    private PlayerCamera camera;

    public PlayerCamera Camera => camera = camera != null ? camera : (PlayerCamera)Owner;

	public override void Enter()
	{
		Camera.SetFOV(CameraFOV);
		Camera.SetOffset(CameraOffset);
		Camera.SetSensitivity(CameraSensitivity);
	}

	public override void Run()
	{
		DebugManager.UpdateRow("CameraState", StateMachine.GetCurrentState().ToString());
	}
}
