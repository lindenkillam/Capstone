using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Video;

public class LectureVideoPlayerScript : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public VideoManagerScript vm;
    public AudioSource AS; 
    int lastLecture = -1, newLecture;
    public bool hasKey;
    public float maxDistance = 25f; 
    public float minDistance = 5f;
    GameObject player;
    bool on;

    public Renderer rd;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player"); 

        newLecture = Random.Range(0, vm.lectures.Length);
        Debug.Log("New lecture chosen: Lecture #" + newLecture);
        lastLecture = newLecture;
        
        videoPlayer.clip = vm.lectures[newLecture];
        //videoPlayer.Play();
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
                on = true;
            }
            else
            {
                videoPlayer.Stop();
                on = false;
            }
        
    }
}