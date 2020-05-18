using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Viktor Dahlberg
public class FixZWrite : MonoBehaviour {

	private int count;

	private List<string> warnings = new List<string>();

	private void Start() {
		GameObject[] allGameObjects = FindObjectsOfType<GameObject>();
		for (int i = 0; i < allGameObjects.Length; i++) {
			if (allGameObjects[i].activeInHierarchy) EnableZWrite(allGameObjects[i].GetComponent<MeshRenderer>());
		}
		Debug.Log("Changed _ZWrite for " + count + " objects.");
	}

	private void EnableZWrite(MeshRenderer mr) {
		if (mr == null) return;
		switch (mr.sharedMaterial.shader.name) {
			case "Standard":
				if (mr.sharedMaterial.GetFloat("_Mode") == 3f) {;
					mr.material.SetInt("_ZWrite", 1);
					count++;
				}
				break;
			default:
				if (!warnings.Contains(mr.sharedMaterial.shader.name)) {
					Debug.LogWarning("Unsupported shader: " + mr.sharedMaterial.shader.name);
					warnings.Add(mr.sharedMaterial.shader.name);
				}
				break;
		}
	}

}