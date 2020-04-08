using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private SphereCollider sc;
    [SerializeField] private float projectileSpeed;
    private Vector3 movementDirection;
    [SerializeField] private float maxLifeTime;
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
        CheckImpact();
        transform.position += movementDirection.normalized * projectileSpeed * Time.deltaTime;
    }

    private void CheckImpact() {
        Physics.SphereCast(transform.position, sc.radius/3, movementDirection, out RaycastHit hit, 0.25f, layerMask);
        if (hit.collider != null) {
            if (hit.collider.gameObject.CompareTag("Player")) {
                Debug.Log("hit player");
            }
            Debug.Log(hit.collider.gameObject);
            Destroy(gameObject);
        }
        else if ((lifeTime += Time.deltaTime) > maxLifeTime)
            Destroy(gameObject);
    }
}
