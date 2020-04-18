using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketV2 : MonoBehaviour, IDamaging {

	#region Properties

	public float Damage { get; set; }

	public float Speed { get; set; }

	public float AreaOfEffect { get; set; }

	public Vector3 Target { get; set; }

	#endregion

	private bool hasExploded = false;

	private void Update() {
		if (!hasExploded) transform.position = Vector3.MoveTowards(transform.position, Target, Speed);
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
				HitPoint = transform.position
			});
		}
		hasExploded = true;
	}

	private void OnTriggerEnter(Collider other) {
		Explode();
	}

}