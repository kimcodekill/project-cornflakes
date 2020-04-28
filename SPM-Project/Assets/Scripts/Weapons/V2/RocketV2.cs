using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketV2 : MonoBehaviour, IDamaging {

	public float Damage { get; set; }
	public float Speed { get; set; }
	public float AreaOfEffect { get; set; }
	public float LifeTime { get; set; }
	public Vector3 TargetDir { get; set; }

	private float startTime;

	private void Start()
	{
		startTime = Time.time;
	}

	private void Update() {
		if (Time.time - startTime < LifeTime){ 
			transform.position += TargetDir * Speed * Time.deltaTime; 
		} else { 
			Explode(); 
		}
	}

	public float GetDamage() {
		return Damage;
	}

	private void Explode() {
		Collider[] hitColliders = Physics.OverlapSphere(transform.position, AreaOfEffect);
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
		if(!other.Equals(GetComponent<Collider>()))
			Explode();
	}

}