using UnityEngine;
using UnityEngine.Video;

public class WelcomeVideoPlayer : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    //[SerializeField] Raycasting raycasting;

    public float maxDistance = 25f;
    public float minDistance = 5f;
    GameObject player;
    public AudioSource AS;
    public Collider col;
    bool on;
    public Renderer rd;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
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

        if (on)
        {
            rd.enabled = true;
        }
        else
        {
            rd.enabled = false;
        }
    }

    public void PlayVideo()
    {
        if (!on)
        {
            videoPlayer.Play();
            on = true;
        }
        else
        {
            videoPlayer.Stop();
            on = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    { 
        if(!videoPlayer.isPlaying && other.gameObject.CompareTag("Player"))
        {
            videoPlayer.Play();
            on = true; 
            col.enabled = false; 
        }
    }
}