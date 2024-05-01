using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Video;

public class LectureVideoPlayerScript : MonoBehaviour
{
  private VideoPlayer videoPlayer;
  public VideoManagerScript vm;
  int lastLecture = -1, newLecture;
  public bool hasKey; 

  // Start is called before the first frame update
  void Awake()
  {
    videoPlayer = this.GetComponent<UnityEngine.Video.VideoPlayer>();

    newLecture = Random.Range(0, vm.lectures.Length);
    Debug.Log("New lecture chosen: Lecture #" + newLecture);
    lastLecture = newLecture;

    videoPlayer.clip = vm.lectures[newLecture];
    videoPlayer.Play();
  }

  void Update()
  {
    
  }

  public void PlayVideo()
  {
        if (!videoPlayer.isPlaying && videoPlayer.isPrepared)
        {
            newLecture = Random.Range(0, vm.lectures.Length);

            if (newLecture == lastLecture)
                return;

            /*
            do
            {
                newLecture = Random.Range(0, vm.lectures.Length);
            } while(newLecture == lastLecture);
            */

            Debug.Log("New lecture chosen: Lecture #" + newLecture);
            lastLecture = newLecture;

            videoPlayer.clip = vm.lectures[newLecture];
            videoPlayer.Play();
        }
        else if (videoPlayer.isPlaying)
        {
            return; 
        }
  }
}