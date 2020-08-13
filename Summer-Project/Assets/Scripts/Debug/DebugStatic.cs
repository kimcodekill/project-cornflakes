using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugStatic
{
    public static string StringifyV2(Vector2 v)
    {
        return string.Format("({0}, {1})", v.x, v.y);
    }

    public static string StringifyV3(Vector3 v)
    {
        return string.Format("({0}, {1}, {2})", v.x, v.y, v.z);
    }
}
