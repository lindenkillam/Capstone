using UnityEngine;
using System.Collections;

public class AudioSwitcher : MonoBehaviour
{
    public AudioClip audioClip1;
    public AudioClip audioClip2; 

    private AudioSource audioSource; 

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        PlayAudioClip(audioClip1);
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            StartCoroutine(AudioSwitchDelay());
        }
    }

    private IEnumerator AudioSwitchDelay()
    {
        yield return new WaitForSeconds(15f);

        if (audioSource.clip == audioClip1)
        {
            PlayAudioClip(audioClip2);
        }
        else if (audioSource.clip == audioClip2)
        {
            PlayAudioClip(audioClip1);
        }
    }

    void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
}
