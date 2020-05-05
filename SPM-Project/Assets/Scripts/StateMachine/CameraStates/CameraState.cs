using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

public abstract class CameraState : State
{
	public int CameraFOV;
	public Vector3 CameraOffset;
	public float CameraSensitivity;

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
