using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SadBoiScript : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Vector3 nextLocation;
    private float wanderDistance = 15f;
    public AudioClip[] SadBoiSounds;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        nextLocation = this.transform.position;
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        Wander();
    }

    private void Wander()
    {
        if(!audioSource.isPlaying)
        {
            int i = Random.Range(0, SadBoiSounds.Length);
            audioSource.clip = SadBoiSounds[i];
            audioSource.Play();
        }
        //If close, choose next location
        if(navAgent.remainingDistance < 1f)
        {
            Vector3 random = Random.insideUnitSphere * wanderDistance;
            random.y = 1f;
            nextLocation = this.transform.position + random;

            if(NavMesh.SamplePosition(nextLocation, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                nextLocation = hit.position;
                navAgent.SetDestination(nextLocation);
            }            
        }
    }
}