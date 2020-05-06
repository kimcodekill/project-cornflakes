using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{

	[SerializeField] private Material material;

	public void SetTransparency(float transparency) {
		material.SetColor("_Color", new Color(material.color.r, material.color.g, material.color.b, transparency));
		
	}

	
}
