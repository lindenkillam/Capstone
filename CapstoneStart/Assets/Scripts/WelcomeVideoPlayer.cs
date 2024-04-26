using UnityEngine;
using UnityEngine.Video;

public class WelcomeVideoPlayer : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<UnityEngine.Video.VideoPlayer>();
    }

  private void OnTriggerStay(Collider other)
  {
    if(!videoPlayer.isPlaying && other.gameObject.CompareTag("Player"))
    {
        videoPlayer.Play();
    }
  }
}