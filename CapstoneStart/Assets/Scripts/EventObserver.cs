using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EventObserver : MonoBehaviour
{
    public GameManager gm;
    public Canvas canvas;

    public GameObject textPrefab;

    void Start()
    {
        SoulNotifier.OnTrueBelieverCaptured += TrueBelieverCaptured;
        //TrueBelieverCaptured();
    }

    private void TrueBelieverCaptured()
    {
        for(byte i = 0; i < 6; ++i)
        {
            InstantiateTBText();
        }
    }

    private void InstantiateTBText()
    {
        int quoteNum = Random.Range(0, gm.TrueBelieverQuotes.Length);
        GameObject newHaunting = Instantiate(textPrefab, canvas.transform, false);
        newHaunting.GetComponent<TextMeshProUGUI>().text = gm.TrueBelieverQuotes[quoteNum];
            
        newHaunting.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
        newHaunting.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
        newHaunting.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center

        //bool left = Random.Range(0, 2) == 1;

        if(Random.Range(0, 2) == 1)
            newHaunting.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Random.Range(-1080, -240), Random.Range(-400,400)); // Position on left side
        else
            newHaunting.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Random.Range(240, 1080), Random.Range(-400,400)); // Position on right side
        
        Debug.Log(newHaunting.GetComponent<RectTransform>().anchoredPosition);
        /*
        quoteNum = Random.Range(0, gm.TrueBelieverQuotes.Length);
        newHaunting = Instantiate(textPrefab, canvas.transform, false);
        newHaunting.GetComponent<TextMeshProUGUI>().text = gm.TrueBelieverQuotes[quoteNum];
            
        newHaunting.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
        newHaunting.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
        newHaunting.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center
        newHaunting.GetComponent<RectTransform>().anchoredPosition =
            new Vector2(Random.Range(157, 470), Random.Range(-287,287)); // Position on right side
        */
    }
}