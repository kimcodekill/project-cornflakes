using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixZWrite : MonoBehaviour {
	
	/// <summary>
	/// The mesh renderers whose materials to enable Z-writing on.
	/// </summary>
	public MeshRenderer[] MeshRenderers;
	
	private void Start() {
		for (int i = 0; i < MeshRenderers.Length; i++) {
			EnableZWrite(MeshRenderers[i]);	
		}
	}

	private void EnableZWrite(MeshRenderer mr) {
		Material m = new Material(mr.material);
		mr.material = m;
		m.SetInt("_ZWrite", 1);
	}

}