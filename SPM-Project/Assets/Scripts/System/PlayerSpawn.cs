using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    public static PlayerSpawn Instance;

    public Vector3 Position { get => transform.position; }
    public Quaternion Rotation { get => transform.rotation; }

    private void OnEnable()
    {
        Instance = this;
    }
}
