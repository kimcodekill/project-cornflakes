using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

public class Rocket : MonoBehaviour, IDamaging
{

    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float areaOfEffect;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private GameObject hitDecal;
    [SerializeField] private GameObject trail;
    [SerializeField] private GameObject explosion;

    public Vector3 TargetDir { get; set; }

    private float startTime;
	private AudioSource audioSource;
    private CapsuleCollider collider;

    private void Start()
    {
		startTime = Time.time;
		trail = Instantiate(trail, transform.position, Quaternion.identity) as GameObject;
		trail.transform.rotation = transform.rotation;
        collider = GetComponent<CapsuleCollider>();

        audioSource = GetComponent<AudioSource>();
		audioSource.Play();
        
    }

    private void Update()
    {
        if (Time.time - startTime < lifeTime)
        {
            if (TargetDir != Vector3.zero)
            {
                transform.position += TargetDir * speed * Time.deltaTime;
            }
        }
        else
        {
            Explode();
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetDamage(float distanceRadius)
    {
        float damageScale = (areaOfEffect - distanceRadius) / areaOfEffect;

        float actualDamage = Mathf.Round(damage * damageScale);

        return actualDamage < 0 ? 0 : actualDamage;
    }

    private void Explode()
    {
        //Because we're using OverlapSphere we cant get the actual hit point
        //Could be solved using SphereCast, but I cant get that working.

        //Collider[] nearColliders = Physics.OverlapSphere(transform.position, areaOfEffect / areaOfEffect);
        //Collider[] farColliders = Physics.OverlapSphere(transform.position, areaOfEffect);

        Collider[] colliders = Physics.OverlapSphere(transform.position, areaOfEffect);

        for (int i = 0; i < colliders.Length; i++)
        {
            EventSystem.Current.FireEvent(new ExplosiveDamageEvent(colliders[i].GetComponent<IEntity>(), this, GetDamageScale(colliders[i].transform)));
        }

        //Makes sure the colliders within 1 unit of the explosion get f'd in the b
        //for (int i = 0; i < nearColliders.Length; i++)
        //{
        //    EventSystem.Current.FireEvent(new ExplosiveDamageEvent(nearColliders[i].gameObject.GetComponent<IEntity>(), this, ));
        //}

        //Smooches the other ones
        //for (int i = 0; i < farColliders.Length; i++)
        //{
        //    EventSystem.Current.FireEvent(new ExplosiveDamageEvent(farColliders[i].gameObject.GetComponent<IEntity>(), this));
        //}

        //Makes the trail stop emitting particles
		ParticleSystem[] trailParticleSystems = trail.gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem childPS in trailParticleSystems)
        {
			ParticleSystem.EmissionModule childPSEmissionModule = childPS.emission;
			childPSEmissionModule.rateOverDistance = 0;
		}

        EventSystem.Current.FireEvent(new ExplosionEffectEvent(explosion, transform.position, Quaternion.identity, areaOfEffect));
        EventSystem.Current.FireEvent(new BulletHitEffectEvent(hitDecal, transform.position, Quaternion.identity, areaOfEffect));

        gameObject.SetActive(false);
    }

    private float GetDamageScale(Transform other)
    {
        if (Physics.Raycast(transform.position, other.position - transform.position, out RaycastHit hit, areaOfEffect, collisionLayers) )
        {
            float distance = (hit.point - transform.position).magnitude;

            //We're adding collider.height just to get closer to 1.0f scale
            return (areaOfEffect - distance + collider.height) / areaOfEffect;
        }

        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if the collided gameobject's layer is in the collisionlayers
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
            Explode();
    }
}