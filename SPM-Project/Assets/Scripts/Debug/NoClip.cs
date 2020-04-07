using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClip : DebugTool
{
    [Header("NoClip Settings")]
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float rotationCap = 89.0f;
    [SerializeField] [Range(0.5f, 2.0f)] private float moveSpeed;

    private Vector2 lookInput = Vector2.zero;

    protected override void OnUpdate()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * moveSpeed * Time.deltaTime;
    }
}
