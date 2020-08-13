using UnityEngine;
using System.Collections;

public class AutoDestroyParticle : MonoBehaviour
{
    private ParticleSystem ps;

    public void Start() {
        ps = GetComponent<ParticleSystem>();
    }

    public void Update() {
        if (ps.IsAlive() == false) {
            Destroy(gameObject);
        }
    }
}