using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraState : State
{
    private PlayerCamera camera;

    public PlayerCamera Camera => camera = camera != null ? camera : (PlayerCamera)Owner;
}
