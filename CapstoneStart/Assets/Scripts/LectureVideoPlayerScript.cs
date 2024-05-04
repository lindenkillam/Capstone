using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Video;

public class LectureVideoPlayerScript : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    public VideoManagerScript vm;
    int lastLecture = -1, newLecture;
    public bool hasKey;
    public float maxDistance = 6f; 
    public float minDistance = 3f;
    GameObject player; 

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 
        videoPlayer = this.GetComponent<UnityEngine.Video.VideoPlayer>();

        newLecture = Random.Range(0, vm.lectures.Length);
        Debug.Log("New lecture chosen: Lecture #" + newLecture);
        lastLecture = newLecture;

        videoPlayer.clip = vm.lectures[newLecture];
        //videoPlayer.Play();
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