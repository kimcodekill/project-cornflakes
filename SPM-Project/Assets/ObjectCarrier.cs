using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCarrier : MonoBehaviour
{
    [SerializeField] private List<GameObject> carriedObjects;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);

        foreach (GameObject g in carriedObjects)
        {
            Debug.Log(g + " was set to not be DestroyedOnLoad");
            DontDestroyOnLoad(g);
        }
    }

    private void OnLevelWasLoaded(int level)
    {
        
    }
}
