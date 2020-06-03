using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUIColor : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Image>().color = transform.root.gameObject.GetComponent<MenuScript>().color;
    }
}
