using System.Collections;
using UnityEngine;
using TMPro;

public class DoorUICheck : MonoBehaviour
{
    [Header("Door")]
    public LayerMask targetMask;
    public float raycastDistance = 2.5f;
    public KeyCode interactKey = KeyCode.E;
    public bool playerHasGuestKey, playerHasSpecialKey;
    public bool isDoorHit;
    public bool doorOpening;
    public bool findDoorLocked, doorNeedsCondition; 

    [Header("Text")]
    public TextMeshProUGUI interactionText, altarText;
    public string requireCondText = "Door locked, search for clues in the surrounding area"; 
    public string doorLockedText = "Door locked, requires key";
    public string openDoorText = "E to open door";

    private Coroutine showTextCoroutine;
    private RaycastHit hitInfo;
    bool altarActivateInstruction; 

    public PlayerRaycast PR;
    public GameObject altar, fog;
    bool checkingAltarCondition;
    public GameObject[] keys;
    public GameObject[] ashtrays; 
    public AudioSource doorSound;

    void Update()
    {
        RaycastHit hit;
        hit = hitInfo;
        
        if (isDoorHit)
        {
            if (doorNeedsCondition)
            {
                interactionText.text = requireCondText;
                StartCoroutine(FalseDelayTwo());
                doorSound.Play();
            }
            else
            {
                if (findDoorLocked)
                {
                    interactionText.text = doorLockedText;
                    StartCoroutine(FalseDelayOne());
                    doorSound.Play(); 
                }
                else
                {
                    interactionText.text = openDoorText;
                    interactionText.gameObject.SetActive(true);
                    DoorCheck();
                }
            }           
        }
        else
        {
            interactionText.text = "";
            interactionText.gameObject.SetActive(false);
        }

        if (checkingAltarCondition)
        {
            if (PR.altarCheck)
            {
                if (!altarActivateInstruction)
                {
                    altarText.text = "Press E to put the ashtrays on the altar";
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        for (int i = 0; i < ashtrays.Length; i++)
                        {
                            ashtrays[i].SetActive(true);
                        }

                        for (int i = 0; i < keys.Length; i++)
                        {
                            keys[i].SetActive(true);
                        }

                        fog.SetActive(true);

                        altarActivateInstruction = true;
                    }
                }
                else
                {
                    altarText.text = "You have unlocked the magical eyes <br>now you can see the hidden things";
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                altarText.text = "You need to get something first";
            }
        }
        else
        {
            altarText.text = ""; 
        }
    }
    
    void DoorCheck()
    {
        if (Input.GetKeyDown(interactKey))
        {
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo, raycastDistance, targetMask))
            {
                InteractWithDoor();
            }
        }
    }

    void InteractWithDoor()
    {
        Door door = hitInfo.transform.GetComponent<Door>();
        if (door != null)
        {
            if (door.isOpened)
            {
                if (!door.isWardrobe)
                {
                    OpenDoor(hitInfo.transform, door.rotatePlus90 ? 90f : -90f);
                }
                else
                {
                    OpenWardrobe(hitInfo.transform, door.rotatePlus90 ? 90f : -90f);
                }
            }

            if (door.requireConditionToOpen)
            {
                doorNeedsCondition = true; 
            }
            else
            {
                doorNeedsCondition = false; 
                if (door.requireKey && !doorOpening)
                {
                    if ((door.guestRoomDoor && playerHasGuestKey) ||
                        (door.specialRoomDoor && playerHasSpecialKey))
                    {
                        if (!door.isWardrobe)
                        {
                            OpenDoor(hitInfo.transform, door.rotatePlus90 ? 90f : -90f);
                        }
                        else
                        {
                            OpenWardrobe(hitInfo.transform, door.rotatePlus90 ? 90f : -90f);
                        }
                    }
                    else
                    {
                        findDoorLocked = true;
                    }
                }
                else if (!door.requireKey && !doorOpening)
                {
                    if (!door.isWardrobe)
                    {
                        OpenDoor(hitInfo.transform, door.rotatePlus90 ? 90f : -90f);
                    }
                    else
                    {
                        OpenWardrobe(hitInfo.transform, door.rotatePlus90 ? 90f : -90f);
                    }
                }
            }
        }
    }

    private void OpenDoor(Transform doorTransform, float RotateFloat)
    {
        Door door = doorTransform.GetComponent<Door>();
        door.isOpened = true;
        Quaternion targetRotation = doorTransform.rotation * Quaternion.Euler(0, RotateFloat, 0);
        StartCoroutine(RotateDoor(targetRotation, doorTransform, RotateFloat));
    }

    private void OpenWardrobe(Transform doorTransform, float RotateFloat)
    {
        Door door = doorTransform.GetComponent<Door>();
        door.isOpened = true;
        Quaternion targetRotation = doorTransform.rotation * Quaternion.Euler(RotateFloat, 0, 0);
        StartCoroutine(RotateWardrobe(targetRotation, doorTransform, RotateFloat));
    }

    IEnumerator RotateDoor(Quaternion targetRotation, Transform target, float RotateFloat)
    {
        float duration = 2.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            doorOpening = true; 
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        targetRotation = target.rotation * Quaternion.Euler(0, -RotateFloat, 0);
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
            doorOpening = false; 
        }
    }

    IEnumerator RotateWardrobe(Quaternion targetRotation, Transform target, float RotateFloat)
    {
        float duration = 2.5f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            doorOpening = true;
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        targetRotation = target.rotation * Quaternion.Euler(-RotateFloat, 0, 0);
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
            doorOpening = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "WaterFaucet")
        {
            WaterFaucet WF = col.gameObject.GetComponent<WaterFaucet>();
            WF.playParticle = true;
        }

        if(col.gameObject == altar)
        {
            checkingAltarCondition = true;
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Door")
        {
            isDoorHit = true;
        }      
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Door")
        {
            isDoorHit = false;
        }

        if (col.gameObject.tag == "WaterFaucet")
        {
            WaterFaucet WF = col.gameObject.GetComponent<WaterFaucet>();
            WF.playParticle = false;
        }

        if (col.gameObject == altar)
        {
            checkingAltarCondition = false;
        }
    }

    IEnumerator FalseDelayOne()
    {
        yield return new WaitForSeconds(1f); 
        findDoorLocked = false;   
    }

    IEnumerator FalseDelayTwo()
    {
        yield return new WaitForSeconds(1f);
        doorNeedsCondition = false;
    }
}