using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullshotter : MonoBehaviour {

	[SerializeField] private Material materialToAssign;
	
	void Start() {
		GameObject[] all = FindObjectsOfType<GameObject>();
		for (int i = 0; i < all.Length; i++) {
			MeshRenderer mr = all[i].GetComponent<MeshRenderer>();
			if (mr && mr.sharedMaterial.name.Equals("Default-Material")) {
				mr.sharedMaterial = materialToAssign;
			}
		}
	}

}