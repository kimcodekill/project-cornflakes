using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berp : MonoBehaviour
{
    private float time = 0;
    private bool reverse;

    private void Start() {
        transform.localScale = new Vector3(0, 0, 0);
    }

    void Update() {
        if (time <= 1) {
            time += Time.deltaTime * 2.5f;
            if (reverse == false) transform.localScale = Vector3.one * Mathfx.Berp(0f, 1f, time);
            else transform.localScale = Vector3.one * Mathfx.Berp(0f, 1f, 1 - time);
        }
        else if (reverse == true) {
            reverse = false;
            gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        time = -1;
    }

    public void ReverseBerp() {
        reverse = true;
        time = 0;
    }
}