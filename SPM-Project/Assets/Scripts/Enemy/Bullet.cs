using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float projectileSpeed;
    private Vector3 movementDirection;
    //[SerializeField] private float maxLifeTime;
    //private float lifeTime;
    [SerializeField] LayerMask layerMask;
    float distanceToTravel;
    float totalDistanceTravelled;
    

    public void Initialize(Vector3 shootDir, float distanceToTarget) {
        movementDirection = shootDir;
        distanceToTravel = distanceToTarget;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckDistance(totalDistanceTravelled, distanceToTravel);
        totalDistanceTravelled += projectileSpeed * Time.deltaTime;
        transform.position += movementDirection.normalized * projectileSpeed * Time.deltaTime;
    }

    private void CheckDistance(float distance, float maxDistance) {
        if (distance >= maxDistance)
            Destroy(gameObject);
    }
}
