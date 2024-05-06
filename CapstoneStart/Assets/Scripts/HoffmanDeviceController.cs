using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro; 

public class HoffmanDeviceController : MonoBehaviour
{
    public AudioSource denySound; 
    public GameObject player, hoffmanDeviceDisplay, hoffmanInstruction;
    public TextMeshProUGUI hoffmanText, timeText, readyText, notTaintedText; 
    public float timeRemaining = 301; 
    public DoorUICheck DC;
    public bool canBeUsed;
    public HandleController HC;
    public CheckChild CC;
    bool playerIn;
    public bool cleansingCanStart;
    bool timeIsRunning;
    bool displayingHoffman;
    public Light light; 

    void Start()
    {
        canBeUsed = true; 
    }

    void Update()
    {
        if (playerIn)
        {
                if (canBeUsed)
                {
                    hoffmanText.text = "Press E to use the Hoffman device";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        if (CC.isPlayerTainted)
                        {
                            displayingHoffman = true;
                            GetComponent<Collider>().enabled = false;
                            hoffmanDeviceDisplay.SetActive(true);
                            hoffmanInstruction.SetActive(true);
                            cleansingCanStart = true;
                        }
                        else
                        {
                            //HC.resetEverything = true; 
                            denySound.Play();
                            notTaintedText.text = "The soul does not seem to be tainted <br>There's no need to use the Hoffman device";
                        }
                    }
                }
                else
                {
                    denySound.Play();
                    TurnOffHoffman();
                    
                    hoffmanText.text = "Hoffman device is cooling down, come back later to use it";
                }
        }
        else
        {
            TurnOffHoffman();
            notTaintedText.text = ""; 
            hoffmanText.text = "";
        }

        if (displayingHoffman)
        {
            hoffmanText.text = "";
        }

        if (HC.resetEverything)
        {
            cleansingCanStart = false; 
            HC.Reset();
            light.color = Color.blue; 
            readyText.enabled = false;
            timeText.enabled = true; 
            StartCoroutine(DisplayTime(timeRemaining));
        }
        else
        {
            light.color = Color.green;
            timeText.enabled = false;
            readyText.enabled = true; 
        }

        if (timeIsRunning)
        {
            if (timeRemaining > 0)
            {
                canBeUsed = false; 
                timeRemaining -= Time.deltaTime;
            }
            else 
            {
                timeRemaining = 0;
                timeIsRunning = false; 
                canBeUsed = true;
                HC.resetEverything = false; 
            }
        }
    }

    public void TurnOffHoffman()
    {
        hoffmanDeviceDisplay.SetActive(false);
        hoffmanInstruction.SetActive(false);
        displayingHoffman = false;
        cleansingCanStart = false;
    }

    IEnumerator DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float minutes = Mathf.FloorToInt(timeRemaining / 60);
        float seconds = Mathf.FloorToInt(timeRemaining % 60);
        //62 % 60 = 1min2sec; 125 & 60 = 2min5sec; 46 % 60 = 46sec
        //float milliSeconds = (timeToDisplay % 1) * 1000;

        yield return new WaitForSeconds(0.1f);
        timeIsRunning = true;
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject == player)
        {
            playerIn = true; 
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject == player)
        {
            playerIn = false;
        }
    }
}
