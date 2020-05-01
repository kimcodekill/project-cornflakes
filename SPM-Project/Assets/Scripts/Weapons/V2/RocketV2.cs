using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketV2 : MonoBehaviour, IDamaging {

	[SerializeField] private float damage;
	[SerializeField] private float speed;
	[SerializeField] private float areaOfEffect;
	[SerializeField] private float lifeTime;
	[SerializeField] private LayerMask collisionLayers;
	
	public Vector3 TargetDir { get; set; }

	private float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update() {
		if (Time.time - startTime < lifeTime){
			if (TargetDir != Vector3.zero)
			{
				transform.position += TargetDir * speed * Time.deltaTime;
			}
		} else { 
			Explode(); 
		}
	}

	public float GetDamage() {
		return damage;
	}

	public float GetExplosionDamage(Vector3 explosionCenter, Vector3 hitPos)
	{
		float distance = (hitPos - explosionCenter).magnitude;

		float damageScale = (areaOfEffect - distance) / areaOfEffect;

		return Mathf.Round(damage * damageScale);
	}

	private void Explode() {
		//Because we're using OverlapSphere we cant get the actual hit point
		//Could be solved using SphereCast, but I cant get that working.
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, areaOfEffect);
		for (int i = 0; i < hitColliders.Length; i++) {
			EventSystem.Current.FireEvent(new HitEvent() {
				Source = gameObject,
				Target = hitColliders[i].gameObject,
				HitPoint = transform.position,
			});
		}

		gameObject.SetActive(false);
	}

	private void OnTriggerEnter(Collider other) {
		//checks if the collided gameobject's layer is in the collisionlayers
		if(collisionLayers == (collisionLayers | (1 << other.gameObject.layer)))
			Explode();
	}
}