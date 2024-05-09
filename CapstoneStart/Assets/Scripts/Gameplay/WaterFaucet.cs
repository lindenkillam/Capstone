using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFaucet : MonoBehaviour
{
    public float maxPower = 20;
    public float minPower = 5;
    public float changeSpeed = 5;
    public ParticleSystem[] hoseWaterSystems;
    private float m_Power;
    public bool washing, playParticle;
    public GameObject waterSound;
    AudioSource audioSource;
    public GameObject hintPaper; 
    public bool spawnHint; 

    private void FixedUpdate()
    {
        audioSource = waterSound.GetComponent<AudioSource>();
        if (playParticle)
        {
            m_Power = Mathf.Lerp(m_Power, Input.GetMouseButton(0) ? maxPower : minPower, Time.deltaTime * changeSpeed);

            if (m_Power > minPower && !audioSource.isPlaying)
            {
                audioSource.Play();
                if (spawnHint)
                {
                    hintPaper.SetActive(true);
                    spawnHint = false;  
                }
            }
            else if (m_Power <= minPower && audioSource.isPlaying)
            {
                audioSource.Stop();
            }

            audioSource.volume = Mathf.Clamp01((m_Power - minPower) / (maxPower - minPower) + 0.1f); 

            washing = (m_Power > minPower);
        }

        foreach (var system in hoseWaterSystems)
        {
            ParticleSystem.MainModule mainModule = system.main;
            mainModule.startSpeed = m_Power;
            var emission = system.emission;
            emission.enabled = (m_Power > minPower * 1.1f);
        }
    }
}
