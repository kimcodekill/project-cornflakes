using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;

    private Transform transformTarget;
    private Vector3 vectorTarget;

    private Vector3 currentTargetPos;

    private void Awake()
    {
        ResetTarget();
        Return();
    }

    public void Send(Transform target, Vector3 forward)
    {
        transformTarget = target;
        Send(target.position, forward);
    }

    public void Send(Vector3 target, Vector3 forward)
    {
        Debug.Log(target);
        vectorTarget = target; 
        transform.forward = forward;

        transform.localScale = Vector3.one;
    }

    private void Return()
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one * 0.01f;
        ResetTarget();
    }

    private void FixedUpdate()
    {
        if ((currentTargetPos = GetTargetPos()) != transform.position)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentTargetPos - transform.position, rotationSpeed * Time.fixedDeltaTime, 0.0f));
            transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime; 
            //transform.position = Vector3.MoveTowards(transform.position, currentTargetPos, moveSpeed * Time.fixedDeltaTime) + transform.forward;
        }
    }

    private Vector3 GetTargetPos()
    {
        return transformTarget != transform ? transformTarget.position :
               vectorTarget != transform.position ? vectorTarget :
               transform.position;
    }

    private void ResetTarget()
    {
        transformTarget = transform;
        vectorTarget = transform.position;
        currentTargetPos = transform.position;
    }

    private void Explode()
    {
        Debug.Log("boom");
    }
}
