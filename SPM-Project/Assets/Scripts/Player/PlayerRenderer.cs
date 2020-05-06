using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Erik Pilström
public class PlayerRenderer : MonoBehaviour
{

	[SerializeField] private Material material;

	/// <summary>
	/// Sets transparency of the material on the player so that the camera can see through.
	/// </summary>
	/// <param name="transparency"></param>
	public void SetTransparency(float transparency) {
		material.SetColor("_Color", new Color(material.color.r, material.color.g, material.color.b, transparency));
	}
}
