using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDollController : MonoBehaviour
{
    Animator anim;
    public bool isWalking; 

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isWalking)
        {
            anim.SetBool("walking", true);
        }
        else
        {
            anim.SetBool("walking", false);
        }

    }
}
