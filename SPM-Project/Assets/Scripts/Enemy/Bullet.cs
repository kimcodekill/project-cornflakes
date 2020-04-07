using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SphereCollider sc;
    [SerializeField] private float projectileSpeed;
    private Vector3 movementDirection;
    private float maxLifeTime = 1f;
    private float lifeTime;
    [SerializeField] LayerMask layerMask;
    

    public void Initialize(Vector3 shootDir) {
        movementDirection = shootDir;
    }

    // Start is called before the first frame update
    void Start()
    {
        sc = GetComponent<SphereCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += movementDirection * projectileSpeed * Time.deltaTime;
        CheckImpact();
    }

    private void CheckImpact() {
        Physics.SphereCast(transform.position, sc.radius, movementDirection, out RaycastHit hit, 0.01f, layerMask);
        if (hit.collider) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                //do things to player
            }
            Debug.Log(hit.collider.gameObject);
            Destroy(gameObject);
        }
        else if ((lifeTime += Time.deltaTime) > maxLifeTime)
            Destroy(gameObject);
    }
}
