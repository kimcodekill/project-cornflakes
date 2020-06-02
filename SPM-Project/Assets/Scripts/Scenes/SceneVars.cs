using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelN",menuName = "SceneVars")]
public class SceneVars : ScriptableObject
{
    [SerializeField] private List<float> times;

    public float GetParTime()
    {
        float sum = 0;
        
        for (int i = 0; i < times.Count; i++)
        {
            sum += times[i];
        }

        return sum / times.Count;
    }
}
