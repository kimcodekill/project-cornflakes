using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KanelbulleEgg : MonoBehaviour, IEntity
{
    [SerializeField] private GameObject kanelbulle;
    [SerializeField] private GameObject sphere;
    [SerializeField] new private ParticleSystem particleSystem;
    
    private AudioSource audioSource;
    private SphereCollider sphereCollider;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    public float Heal(float amount)
    {
        return -1;
    }

    public float TakeDamage(float amount, DamageType damageType)
    {
        if(damageType == DamageType.Bullet) { DoEasterEgg(); }
        
        return -1;
    }

    private void DoEasterEgg()
    {
        sphereCollider.enabled = false;
        sphere.SetActive(false);
        kanelbulle.SetActive(true);
        particleSystem.Play();
        audioSource.Play();
    }
}
