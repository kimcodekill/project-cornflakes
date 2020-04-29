using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRenderer : MonoBehaviour
{

	private Material material;
    // Start is called before the first frame update
    void Start()
    {
		material = GetComponent<MeshRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void SetTransparency(float transparency) {
		material.SetColor("_Color", new Color(material.color.r, material.color.g, material.color.b, transparency));
	}
}
