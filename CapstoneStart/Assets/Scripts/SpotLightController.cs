using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightController : MonoBehaviour
{
    Light myLight;
    void Start()
    {
        myLight = GetComponent<Light>();
    }

    void Update()
    {
        myLight.intensity += Input.mouseScrollDelta.y * 1;
        if (myLight.intensity >= 3)
        {
            myLight.intensity = 3;
        }
    }
}
