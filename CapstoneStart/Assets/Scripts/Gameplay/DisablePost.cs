using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePost : MonoBehaviour
{
    public bool deletePost;
    public GameObject postEffect;
    
    public void PostDisable()
    {
        postEffect.SetActive(false); 
    }
}
