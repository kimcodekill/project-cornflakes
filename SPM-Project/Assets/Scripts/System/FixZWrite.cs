using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixZWrite : MonoBehaviour {

	private int count;
	
	private void Start() {
		GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
		for (int i = 0; i < allGameObjects.Length; i++) {
			if (allGameObjects[i].activeInHierarchy) EnableZWrite(allGameObjects[i].GetComponent<MeshRenderer>());
		}
		Debug.LogWarning("Changed _ZWrite for " + count + " objects.");
	}

	private void EnableZWrite(MeshRenderer mr) {
		if (mr == null) return;
		if (mr.material.GetFloat("_Mode") == 3f) {
			Material m = new Material(mr.material);
			mr.material = m;
			m.SetInt("_ZWrite", 1);
			count++;
		}
	}

}