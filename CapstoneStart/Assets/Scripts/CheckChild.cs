using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChild : MonoBehaviour
{
    public bool isPlayerTainted;

    void Update()
    {
        if (transform.childCount > 0)
        {
            isPlayerTainted = true;
        }
        else
        {
            isPlayerTainted = false;
        }
    }
}
