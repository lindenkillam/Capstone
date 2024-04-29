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

    [Header("Text")]
    public TextMeshProUGUI interactionText;
    public string doorLockedText = "Door locked, requires key";
    public string openDoorText = "E to open door";

    private Coroutine showTextCoroutine;
    private RaycastHit hitInfo; // Store hit information for later use

    void Update()
    {
        DoorCheck();
    }

    void DoorCheck()
    {
        // Reset hit door state each frame
        isDoorHit = false;

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(interactKey))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance, targetMask))
            {
                isDoorHit = true;
                hitInfo = hit; // Store hit information
                if (Input.GetMouseButtonDown(0))
                {
                    ShowInteractionText(openDoorText, 1.5f);
                }
                else if (Input.GetKeyDown(interactKey))
                {
                    InteractWithDoor();
                }
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
                return;
            }

            if (door.requireKey)
            {
                if ((door.guestRoomDoor && playerHasGuestKey) ||
                    (door.specialRoomDoor && playerHasSpecialKey))
                {
                    OpenDoor(hitInfo.transform);
                }
                else
                {
                    ShowInteractionText(doorLockedText, 1.5f);
                }
            }
            else
            {
                OpenDoor(hitInfo.transform);
            }
        }
    }

    private void OpenDoor(Transform doorTransform)
    {
        Door door = doorTransform.GetComponent<Door>();
        door.isOpened = true;
        Quaternion targetRotation = doorTransform.rotation * Quaternion.Euler(0, 90, 0);
        StartCoroutine(RotateDoor(targetRotation, doorTransform));
    }

    IEnumerator RotateDoor(Quaternion targetRotation, Transform target)
    {
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
        }

        yield return new WaitForSeconds(2f);

        targetRotation = target.rotation * Quaternion.Euler(0, -90, 0);
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            target.rotation = Quaternion.Slerp(target.rotation, targetRotation, elapsed / duration);
            yield return null;
        }
    }

    IEnumerator ShowInteractionText(string message, float delay)
    {
        interactionText.gameObject.SetActive(true);
        interactionText.text = message;

        yield return new WaitForSeconds(delay);

        interactionText.gameObject.SetActive(false);
    }
}
