using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Video;

public class EventObserver : MonoBehaviour
{
    public QuoteManager qm;
    public Canvas canvas;
    public TMP_FontAsset[] enemyFonts;

    public GameObject textPrefab;
    public GameObject bagUI;
    public VideoPlayer[] videoPlayers;
    public VideoPlayer WelcomeTV;
    public bool canCheckBag; 

    void Start()
    {
        canCheckBag = true; 
        SoulNotifier.OnTrueBelieverCaptured += TrueBelieverCaptured;
        SoulNotifier.OnSadBoiCaptured += SadBoiCaptured;
        SoulNotifier.OnOverworkedCaptured += OverworkedCaptured;
        SoulNotifier.BossGotcha += BossDoneGotcha;
    }

    void Update()
    {
        if (canCheckBag)
        {
            if (Input.GetKey(KeyCode.B))
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        bagUI.SetActive(true);
    }

    void ResumeGame()
    {
        bagUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void TrueBelieverCaptured()
    {
        Debug.Log("TrueBeliever done.");
        for(byte i = 0; i < 4; ++i)
        {
            InstantiateTBText();
        }
    }

    private void SadBoiCaptured()
    {
        Debug.Log("SadBoi done.");
        for(byte i = 0; i < 4; ++i)
        {
            InstantiateSBText();
        }
    }

    private void OverworkedCaptured()
    {
        Debug.Log("Overworked done.");
        for(byte i = 0; i < 4; ++i)
        {
            InstantiateOWText();
        }
    }

    private void BossDoneGotcha()
    {
        GameObject newTextPrefab = Instantiate(textPrefab, canvas.transform, false);
        newTextPrefab.GetComponent<TextMeshProUGUI>().text = "You met the old man himself.";
        newTextPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
        newTextPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
        newTextPrefab.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center
        newTextPrefab.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 300);
    }

    private void InstantiateTBText()
    {
        int quoteNum = Random.Range(0, qm.TrueBelieverQuotes.Length);
        int fontNum = Random.Range(0, enemyFonts.Length);
        GameObject newTextPrefab = Instantiate(textPrefab, canvas.transform, false);
        newTextPrefab.GetComponent<TextMeshProUGUI>().font = enemyFonts[fontNum];
        newTextPrefab.GetComponent<TextMeshProUGUI>().text = qm.TrueBelieverQuotes[quoteNum];
        PositionText(newTextPrefab);
    }

    private void InstantiateSBText()
    {
        int quoteNum = Random.Range(0, qm.SadBoiQuotes.Length);
        int fontNum = Random.Range(0, enemyFonts.Length);
        GameObject newTextPrefab = Instantiate(textPrefab, canvas.transform, false);
        newTextPrefab.GetComponent<TextMeshProUGUI>().font = enemyFonts[fontNum];
        newTextPrefab.GetComponent<TextMeshProUGUI>().text = qm.SadBoiQuotes[quoteNum];
        PositionText(newTextPrefab);
    }

    private void InstantiateOWText()
    {
        int quoteNum = Random.Range(0, qm.OverworkedQuotes.Length);
        int fontNum = Random.Range(0, enemyFonts.Length);
        GameObject newTextPrefab = Instantiate(textPrefab, canvas.transform, false);
        newTextPrefab.GetComponent<TextMeshProUGUI>().font = enemyFonts[fontNum];
        newTextPrefab.GetComponent<TextMeshProUGUI>().text = qm.OverworkedQuotes[quoteNum];
        PositionText(newTextPrefab);        
    }

    private void PositionText(GameObject newTextPrefab)
    {  
        newTextPrefab.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f); // anchor at center
        newTextPrefab.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f); // anchor at center
        newTextPrefab.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f); // pivot at center

        if(Random.Range(0, 2) == 1)
            newTextPrefab.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Random.Range(-900, -240), Random.Range(-600,550)); // Position on left side
        else
            newTextPrefab.GetComponent<RectTransform>().anchoredPosition =
                new Vector2(Random.Range(240, 900), Random.Range(-600,550)); // Position on right side
        
        //Debug.Log(newTextPrefab.GetComponent<RectTransform>().anchoredPosition);
    }
}