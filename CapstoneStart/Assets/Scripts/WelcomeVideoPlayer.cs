using UnityEngine;
using UnityEngine.Video;

public class WelcomeVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    //[SerializeField] Raycasting raycasting;

    public float maxDistance = 6f;
    public float minDistance = 3f;
    GameObject player; 

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        videoPlayer = this.GetComponent<UnityEngine.Video.VideoPlayer>();
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);

        float volume = Mathf.Clamp01(1f - Mathf.InverseLerp(minDistance, maxDistance, distance));

        videoPlayer.SetDirectAudioVolume(0, volume);

        if (distance >= maxDistance)
        {
            videoPlayer.SetDirectAudioVolume(0, 0f);
        }
        else if (distance <= minDistance)
        {
            videoPlayer.SetDirectAudioVolume(0, 0f);
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