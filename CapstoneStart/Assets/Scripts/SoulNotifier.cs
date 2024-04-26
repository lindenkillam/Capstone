using UnityEngine;
using System;

public class SoulNotifier : MonoBehaviour
{
    public static event Action OnTrueBelieverCaptured;
    public static event Action OnSadBoiCaptured;
    public static event Action OnOverworkedCaptured;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("I hit something, ouch.");
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Someone collided with the player!");
            if(this.CompareTag("TrueBeliever") && OnTrueBelieverCaptured != null) //Check whether anyone is subscribed to the event
            {
                OnTrueBelieverCaptured(); //Run the event on all subscribers
            }
            else if(this.CompareTag("SadBoi") && OnSadBoiCaptured != null)
            {
                OnSadBoiCaptured();
            }
            else if(this.CompareTag("Overworked") && OnOverworkedCaptured != null)
            {
                OnOverworkedCaptured();
            }

            Destroy(this.gameObject);
        }
    }
}