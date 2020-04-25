using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryElevator : MonoBehaviour
{
    public Transform elevatorTarget;
    public GameObject playerMech;

    private void OnTriggerEnter(Collider other)
    {
        playerMech.transform.position = elevatorTarget.transform.position;
    }
}
