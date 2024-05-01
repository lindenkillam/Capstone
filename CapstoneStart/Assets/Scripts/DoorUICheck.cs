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
    public bool findDoorLocked; 

    [Header("Text")]
    public TextMeshProUGUI interactionText;
    public string doorLockedText = "Door locked, requires key";
    public string openDoorText = "E to open door";

    private Coroutine showTextCoroutine;
    private RaycastHit hitInfo; // Store hit information for later use

    void Update()
    {
        RaycastHit hit;
        hit = hitInfo;

        if (isDoorHit)
        {
            if (findDoorLocked)
            {
                interactionText.text = doorLockedText;
                StartCoroutine(FalseDelay());
            }
            else
            {
                interactionText.text = openDoorText;
                interactionText.gameObject.SetActive(true);
                DoorCheck();
            }
        }
        else
        {
            interactionText.text = "";
            interactionText.gameObject.SetActive(false);
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
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            doorOpening = true; 
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

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
        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            doorOpening = true;
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

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
    }

    IEnumerator FalseDelay()
    {
        yield return new WaitForSeconds(1f); 
        findDoorLocked = false;   
    }
}