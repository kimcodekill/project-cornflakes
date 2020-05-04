using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using UnityEngine;

public class RocketV2 : MonoBehaviour, IDamaging
{


    [SerializeField] private float damage;
    [SerializeField] private float speed;
    [SerializeField] private float areaOfEffect;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private GameObject explosion;

    public Vector3 TargetDir { get; set; }

    private float startTime;
	private GameObject actualTrail;

    private void Start()
    {
        startTime = Time.time;
		actualTrail = Instantiate(trail, transform.position, Quaternion.identity) as GameObject;
		actualTrail.transform.rotation = transform.rotation;
		trail.SetActive(false);
		explosion.SetActive(false);
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

    public float GetExplosionDamage(Vector3 explosionCenter, Vector3 hitPos)
    {
        float distance = (hitPos - explosionCenter).magnitude;

        float damageScale = (areaOfEffect - distance) / areaOfEffect;

        float actualDamage = Mathf.Round(damage * damageScale);

        return actualDamage < 0 ? 0 : actualDamage;
    }

    private void Explode()
    {
        //Because we're using OverlapSphere we cant get the actual hit point
        //Could be solved using SphereCast, but I cant get that working.
        Collider[] nearColliders = Physics.OverlapSphere(transform.position, areaOfEffect / areaOfEffect);
        Collider[] farColliders = Physics.OverlapSphere(transform.position, areaOfEffect);

        //Makes sure the colliders within 1 unit of the explosion get f'd in the b
        for (int i = 0; i < nearColliders.Length; i++)
        {
            EventSystem.Current.FireEvent(new HitEvent()
            {
                Source = gameObject,
                Target = nearColliders[i].gameObject,
                HitPoint = nearColliders[i].transform.position,
            });
        }

        //Smooches the other ones
        for (int i = 0; i < farColliders.Length; i++)
        {
            EventSystem.Current.FireEvent(new HitEvent()
            {
                Source = gameObject,
                Target = farColliders[i].gameObject,
                HitPoint = transform.position,
            });
        }
		GameObject expl = Instantiate(explosion, transform.position, Quaternion.identity) as GameObject;
		expl.gameObject.SetActive(true);
		expl.transform.localScale *= areaOfEffect;

		ParticleSystem[] actualTrailParticleSystems = actualTrail.gameObject.GetComponentsInChildren<ParticleSystem>();
		foreach (ParticleSystem childPS in actualTrailParticleSystems) {
			ParticleSystem.EmissionModule childPSEmissionModule = childPS.emission;
			childPSEmissionModule.rateOverDistance = 0;
		}

        EventSystem.Current.FireEvent(new ExplosionEffectEvent()
        {
            ExplosionEffect = explosion,
            WorldPosition = transform.position,
            Rotation = Quaternion.identity,
            Scale = areaOfEffect
        }) ;

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks if the collided gameobject's layer is in the collisionlayers
        if (collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
            Explode();
    }
}