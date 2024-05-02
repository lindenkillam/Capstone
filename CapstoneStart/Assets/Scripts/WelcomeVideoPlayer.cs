using UnityEngine;
using UnityEngine.Video;

public class WelcomeVideoPlayer : MonoBehaviour
{
  private VideoPlayer videoPlayer;
  //[SerializeField] Raycasting raycasting;

  // Start is called before the first frame update
  void Start()
  {
    videoPlayer = this.GetComponent<UnityEngine.Video.VideoPlayer>();
  }

  private void OnTriggerStay(Collider other)
  {
    if(!videoPlayer.isPlaying && other.gameObject.CompareTag("Player"))
    {
            Debug.Log("ss"); 
        videoPlayer.Play();
    }
  }
}