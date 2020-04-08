using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour, IPooledObject
{
    [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxLifeTime;
    
    private Vector3 targetPos;
    private Vector3 targetSurfaceNormal;
    private float lifeTime;

    public void SetTarget(Vector3 position, Vector3 surfaceNormal)
    {
        targetPos = position;
        targetSurfaceNormal = -surfaceNormal;

    }

    private void FixedUpdate()
    {
        if (targetPos != Vector3.zero)
        {
            if(Physics.SphereCast(transform.position, 0.2f, transform.forward, out RaycastHit hit, 0.2f, collisionMask) ||
               lifeTime > maxLifeTime)
            { 
                Explode(); 
            }
            else
            {
                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, targetPos - transform.position, rotationSpeed, 0.0f));
                transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
            }
        }

        lifeTime += Time.fixedDeltaTime;
    }

    private void Explode()
    {
        Debug.Log("boom");
        gameObject.SetActive(false);
    }

    public void OnObjectSpawn()
    {
        
    }
}
