using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class EnemyTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    private TMP_Text thisText;
    const float alphaMin = 0.25f;
 
    private void Awake()
    {
        thisText = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thisText.alpha > alphaMin)
        {
        thisText.color = thisText.color -
            new Color(0, 0, 0, .1f*Time.deltaTime);
        }
    }
}
