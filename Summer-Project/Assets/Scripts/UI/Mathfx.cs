﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class Mathfx {
    //Boing
    public static float Berp(float start, float end, float value) {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    public static Vector2 Berp(Vector2 start, Vector2 end, float value) {
        return new Vector2(Berp(start.x, end.x, value), Berp(start.y, end.y, value));
    }

    public static Vector3 Berp(Vector3 start, Vector3 end, float value) {
        return new Vector3(Berp(start.x, end.x, value), Berp(start.y, end.y, value), Berp(start.z, end.z, value));
    }
}