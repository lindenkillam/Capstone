using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightController : MonoBehaviour
{
    Light myLight;
    void Start()
    {
        myLight = GetComponentInChildren<Light>();
    }

    void Update()
    {
        myLight.intensity += Input.mouseScrollDelta.y * 1;
        if (myLight.intensity >= 2)
        {
            myLight.intensity = 2;
        }
    }
}
