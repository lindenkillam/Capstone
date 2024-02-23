using UnityEngine;
using System;

public class SoulNotifier : MonoBehaviour
{
    public static event Action OnTrueBelieverCaptured;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(this.CompareTag("TrueBeliever") && OnTrueBelieverCaptured != null) //Check whether anyone is subscribed to the event
            {
                OnTrueBelieverCaptured(); //Run the event on all subscribers
            }

            Destroy(this.gameObject);
        }
    }
}