using UnityEngine;
using UnityEngine.Video;

public class WelcomeVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    //[SerializeField] Raycasting raycasting;

    public float maxDistance = 25f;
    public float minDistance = 5f;
    GameObject player;
    AudioSource AS; 

    void Start()
    {
        AS = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
        videoPlayer = this.GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        float _volume = Mathf.Clamp01(1f - Mathf.InverseLerp(minDistance, maxDistance, distance));

        _volume = Mathf.Min(_volume, 0.5f);

        AS.volume = _volume;

        if (distance >= maxDistance)
        {
            AS.volume = 0f;
        }
        else if (distance <= minDistance)
        {
            AS.volume = 0.5f;
        }
    }

    private void OnTriggerStay(Collider other)
    { 
        if(!videoPlayer.isPlaying && other.gameObject.CompareTag("Player"))
        {
            videoPlayer.Play();
        }
    }
}