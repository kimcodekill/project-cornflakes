using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Joakim Linna

public class LevelExit : MonoBehaviour
{
    [SerializeField] private bool visualizeVolume;
    [SerializeField] private GameObject volume;

    private BoxCollider collider;

    private void Awake()
    {
        collider = GetComponent<BoxCollider>();

        VisualizeExit();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("Player entered exit");
            EventSystem.Current.FireEvent(new LevelEndEvent(-1, Time.time));

            collider.enabled = false;
        }
    }

    private void VisualizeExit()
    {
        volume.SetActive(visualizeVolume);
        volume.transform.localScale = collider.size;
    }
}
