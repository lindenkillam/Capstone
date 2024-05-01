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

    private void Update()
    {
        if (playParticle)
        {
            m_Power = Mathf.Lerp(m_Power, Input.GetMouseButton(0) ? maxPower : minPower, Time.deltaTime * changeSpeed);
            if (Input.GetMouseButtonDown(0))
            {
                waterSound.GetComponent<AudioSource>().Play();
            }
            if (m_Power > minPower)
            {
                washing = true;
            }
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
