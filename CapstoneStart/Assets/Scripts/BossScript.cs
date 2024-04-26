using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class BossScript : MonoBehaviour
{
    public Transform tetaviPlayer;
    private NavMeshAgent navAgent;
    private Vector3 nextLocation;
    private float wanderDistance = 15f;
    private float seeDistance = 50f;
    private float timer = -2.0f;
    const float skipTime = 5.0f;
    private GameObject player;
    public AudioClip[] HoffmanSounds;
    AudioSource audioSource;
    private bool soundActivated = false;
    private bool resetHolo = true;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.GetComponent<AudioSource>();
        player = GameObject.FindWithTag("Player");
        nextLocation = this.transform.position;
        navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.SetDestination(player.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        
            if(CanSeePlayer()) 
            {
                if (timer > 0.3f)
                {
                    ChaseVisible();
                }
                else if (timer < -0.3f)
                {
                    ChaseHidden();
                }
                else
                {
                    tetaviPlayer.gameObject.SetActive(true);
                    tetaviPlayer.localPosition = new Vector3(0,10000,0);
                }
            }
            else
            {
                Wander();
            }

        if (timer > skipTime)
        {
            timer = -2.0f;
        }

    }

    private void Wander()
    {
        Debug.Log("Wandering!");
        tetaviPlayer.gameObject.SetActive(false);
        if(audioSource.isPlaying)
        {
            //soundActivated = false;
            audioSource.Stop();
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

    private void ChaseHidden()
    {

        Debug.Log("Chasing Hidden");
        tetaviPlayer.GetComponent<TetaviPlayer>().FramePlaying = 0;
        tetaviPlayer.gameObject.SetActive(false);
        navAgent.speed = 2f;
        if(!audioSource.isPlaying)
        {
            Debug.Log("Hoffman sound activated!");
            //soundActivated = true;
            int i = Random.Range(0, HoffmanSounds.Length);
            audioSource.clip = HoffmanSounds[i];
            audioSource.Play();
        }

        if (navAgent.destination != player.transform.position)
            navAgent.SetDestination(player.transform.position);
    }

    private void ChaseVisible()
    {
        navAgent.speed = 0.1f;
        tetaviPlayer.localPosition = new Vector3(0,0,0);
        Debug.Log("Chasing Visible");
        if (navAgent.destination != player.transform.position)
            navAgent.SetDestination(player.transform.position);
    }


    private bool CanSeePlayer()
    {
        float distance = (player.transform.position - this.transform.position).magnitude;
  
        Vector3 direction = (player.transform.position + Vector3.up) - (this.transform.position + Vector3.up);
        Ray ray = new Ray(this.transform.position + Vector3.up, direction);
        Debug.DrawRay(this.transform.position + Vector3.up, direction, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, seeDistance))
        {
            if (hit.collider.gameObject == player)
            {
                //Debug.Log("Player found by " + gameObject.name);
                return true;
            }
        }

        return false;
    }
}