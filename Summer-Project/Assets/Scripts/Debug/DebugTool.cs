﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

public class DebugTool : MonoBehaviour
{
    public bool enabled;
    
    private void Awake() { if (enabled) { OnAwake(); } }

    private void Update() { if (enabled) { OnUpdate(); } }

    private void FixedUpdate() { if (enabled) { OnFixedUpdate(); } }

    protected virtual void OnAwake() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnFixedUpdate() { }
}
