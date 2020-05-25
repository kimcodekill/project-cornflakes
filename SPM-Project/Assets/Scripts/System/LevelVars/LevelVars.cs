using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Level Variables")]
public class LevelVars : ScriptableObject
{
    [SerializeField] private string levelPath = "Scenes/";
    [SerializeField] private float parTime;

    public string LevelPath { get => levelPath; }
    public float ParTime { get => parTime; }
}
