using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SoulWanderSeek : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Vector3 nextLocation;
    private float wanderDistance = 15f;
    private float seeDistance = 20f;
    public GameObject player;
    public AudioSource TBSound;
    private bool soundActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindWithTag("Player");
        nextLocation = this.transform.position;
        navAgent = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CanSeePlayer())
        {
            Chase();
        }
        else
        {
            Wander();
        }
    }

    private void Wander()
    {
        if(soundActivated)
        {
            soundActivated = false;
            TBSound.Stop();
        }
        //If close, choose next location
        if(navAgent.remainingDistance < 1f)
        {
            Vector3 random = Random.insideUnitSphere * wanderDistance;
            random.y = 1f;
            nextLocation = this.transform.position + random;

            if(NavMesh.SamplePosition(nextLocation, out NavMeshHit hit, 5f , NavMesh.AllAreas))
            {
                nextLocation = hit.position;
                navAgent.SetDestination(nextLocation);
            }            
        }
    }

    private void Chase()
    {
        if(!soundActivated)
        {
            soundActivated = true;
            TBSound.Play();
        }
        if (navAgent.destination != player.transform.position)
            navAgent.SetDestination(player.transform.position);
    }

    private bool CanSeePlayer()
    {
        float distance = (player.transform.position - this.transform.position).magnitude;
  
        Vector3 direction = (player.transform.position+Vector3.up) - (this.transform.position + Vector3.up);
        Ray ray = new Ray(this.transform.position + Vector3.up, direction);
        Debug.DrawRay(this.transform.position + Vector3.up, direction, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, seeDistance))
        {
            if (hit.collider.gameObject == player)
            {
                Debug.Log("Player found by "+gameObject.name);
                return true;
            }
        }

        return false;
    }
}