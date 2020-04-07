using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] [Range(0, 100)] private float distance;

    private Vector3 localPos;

    private string name;

    private void Awake()
    {
        localPos = new Vector3(transform.localPosition.x, transform.localPosition.y, distance);
        name = gameObject.name;
    }

    private void Update()
    {
        //let target have variable distance
        if (transform.localPosition != (localPos = new Vector3(transform.localPosition.x, transform.localPosition.y, distance)))
        {
            transform.localPosition = localPos;
            gameObject.name = string.Format("{0} [{1} u]", name, distance);
        }
    }
}
