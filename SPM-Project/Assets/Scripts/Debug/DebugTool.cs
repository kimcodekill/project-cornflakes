using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTool : MonoBehaviour
{
    public bool enabled;

    private void Update()
    {
        if(enabled) { OnUpdate(); }
    }

    private void FixedUpdate()
    {
        if(enabled) { OnFixedUpdate(); }
    }

    protected virtual void OnUpdate() { }

    protected virtual void OnFixedUpdate() { }
}
