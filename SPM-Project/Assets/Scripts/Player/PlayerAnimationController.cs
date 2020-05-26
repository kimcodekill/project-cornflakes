using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private float speed;
    private float direction;
    public Transform pivot;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        pivot = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        speed = Input.GetAxis("Vertical");
        direction = Input.GetAxis("Horizontal");

        anim.SetFloat("Speed", speed);
        anim.SetFloat("Direction", direction);


    }
}
