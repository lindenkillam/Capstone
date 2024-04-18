using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyDollController : MonoBehaviour
{
    Animator anim;
    public bool playerIsClose;
    public bool caughtByPlayer;
    public bool soulsRelease;
    private States currentState;
    private States previousState;

    public enum States
    {
        Idle,
        Walk,
        Release,
        Caught
    }
   
    void Start()
    {
        anim = GetComponent<Animator>();
        currentState = States.Idle;
        previousState = currentState;
    }

    States GetStates()
    {
        if (playerIsClose)
            return States.Walk;

        if (soulsRelease)
            return States.Release;

        if (caughtByPlayer)
            return States.Caught;

        return States.Idle;  // Default state
    }

    void Update()
    {
        currentState = GetStates();

        if (currentState != previousState)
        {
            // Reset all trigger parameters
            anim.ResetTrigger("Idle");
            anim.ResetTrigger("Walk");
            anim.ResetTrigger("Release");
            anim.ResetTrigger("Caught");

            // Set the trigger based on the current state
            switch (currentState)
            {
                case States.Idle:
                    anim.SetTrigger("Idle");
                    break;

                case States.Walk:
                    anim.SetTrigger("Walk");
                    break;

                case States.Release:
                    anim.SetTrigger("Release");
                    break;

                case States.Caught:
                    anim.SetTrigger("Caught");
                    break;
            }

            previousState = currentState;  // Update the previous state
        }
    }
}
