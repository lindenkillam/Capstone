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
        SoulNotifier.OnSadBoiCaptured += SadBoiCaptured;
    }

    private void TrueBelieverCaptured()
    {
        for(byte i = 0; i < 6; ++i)
        {
            InstantiateTBText();
        }
    }

    private void SadBoiCaptured()
    {
        for(byte i = 0; i < 6; ++i)
        {
            InstantiateSBText();
        }
    }

    private void InstantiateTBText()
    {
        int quoteNum = Random.Range(0, gm.TrueBelieverQuotes.Length);
        GameObject newTextPrefab = Instantiate(textPrefab, canvas.transform, false);
        newTextPrefab.GetComponent<TextMeshProUGUI>().text = gm.TrueBelieverQuotes[quoteNum];
        PositionText(newTextPrefab, quoteNum);
    }

    private void InstantiateSBText()
    {
        int quoteNum = Random.Range(0, gm.SadBoiQuotes.Length);
        GameObject newTextPrefab = Instantiate(textPrefab, canvas.transform, false);
        newTextPrefab.GetComponent<TextMeshProUGUI>().text = gm.SadBoiQuotes[quoteNum];
        PositionText(newTextPrefab, quoteNum);
    }

    private void PositionText(GameObject newTextPrefab, int quoteNum)
    {  
        newTextPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
        newTextPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
        newTextPrefab.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center

        if(Random.Range(0, 2) == 1)
            newTextPrefab.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Random.Range(-900, -240), Random.Range(-400,400)); // Position on left side
        else
            newTextPrefab.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Random.Range(240, 900), Random.Range(-400,400)); // Position on right side
        
        Debug.Log(newTextPrefab.GetComponent<RectTransform>().anchoredPosition);
    }
}