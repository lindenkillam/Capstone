using System.Collections;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Door
{
    public bool requireKey = false;
    public bool isOpened = false;
}

public class DoorController : MonoBehaviour
{
    Transform doorTransform;
    public Door door = new Door();
    public KeyCode interactKey = KeyCode.E;
    public string doorLockedText = "Door locked, requires key";
    public string openDoorText = "E to open door";
    private float raycastDistance = 2.5f;
    public LayerMask playerLayer;
    public TextMeshProUGUI interactionText;
    GameObject player; 
    public bool playerHasKey = false;
    private bool isPlayerNear = false;
    private Coroutine hideTextCoroutine;
    public bool forward, right, diagonal;
    bool doorOpened; 

    void Start()
    {
        player = GameObject.Find("Player").gameObject;
        doorTransform = transform;
    }

    void FixedUpdate()
    {
        if (forward)
        {
            ForwardRaycast();
        }

        if (right)
        {
            RightRaycast();
        }

        if (diagonal)
        {
            DiagonalRaycast();
        }

        if (doorOpened)
        {
            ShowInteractionText("Door opened");
        }
        else
        {
            ShowInteractionText(openDoorText); 
        }
    }

    void ForwardRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(doorTransform.position, doorTransform.forward, out hit, raycastDistance, playerLayer) ||
            Physics.Raycast(doorTransform.position, -doorTransform.forward, out hit, raycastDistance, playerLayer))
        {
            
            if (hit.collider.gameObject == player)
            {
                Debug.DrawLine(doorTransform.position, hit.point, Color.green); // Draw the raycast line
                doorOpened = false; 
                if (door.requireKey)
                {
                    if (playerHasKey && Input.GetKeyDown(interactKey) || door.isOpened && Input.GetKeyDown(interactKey))
                    {
                        if (isPlayerNear)
                        {
                            OpenDoor();
                        }   
                    }
                    else if (!playerHasKey && Input.GetKeyDown(interactKey))
                    {
                        ShowInteractionText(doorLockedText);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(interactKey) && isPlayerNear)
                    {
                        OpenDoor();
                    }
                }
            }
        }
    }

    void RightRaycast()
    {
        RaycastHit hit;

        if (Physics.Raycast(doorTransform.position, doorTransform.right, out hit, raycastDistance, playerLayer) ||
            Physics.Raycast(doorTransform.position, -doorTransform.right, out hit, raycastDistance, playerLayer))
        {
            if (hit.collider.gameObject == player)
            {
                Debug.DrawLine(doorTransform.position, hit.point, Color.green); // Draw the raycast line
       
                doorOpened = false;
                if (door.requireKey)
                {
                    if (playerHasKey && Input.GetKeyDown(interactKey) || door.isOpened && Input.GetKeyDown(interactKey))
                    {
                        if (isPlayerNear)
                        {
                            OpenDoor();
                        }
                    }
                    else if (!playerHasKey && Input.GetKeyDown(interactKey))
                    {
                        ShowInteractionText(doorLockedText);
                    }
                }
                else
                {
                    if (Input.GetKeyDown(interactKey) && isPlayerNear)
                    {
                        OpenDoor();
                    }
                }
            }
        }
    }

    void DiagonalRaycast()
    {

    }

    void OpenDoor()
    {
        door.isOpened = true;
        doorOpened = true; 
        Quaternion targetRotation = doorTransform.rotation * Quaternion.Euler(0, 90, 0);
        StartCoroutine(RotateDoor(targetRotation));
    }

    IEnumerator RotateDoor(Quaternion targetRotation)
    {
        float duration = 2f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, targetRotation, elapsed / duration);
            yield return null;
        }

        if (hideTextCoroutine != null)
        {
            StopCoroutine(hideTextCoroutine);
        }
        //hideTextCoroutine = StartCoroutine(HideTextAfterDelay(2f));

        yield return new WaitForSeconds(2f);

        targetRotation = doorTransform.rotation * Quaternion.Euler(0, -90, 0);
        elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            doorTransform.rotation = Quaternion.Slerp(doorTransform.rotation, targetRotation, elapsed / duration);
            yield return null;
        }
    }

    void ShowInteractionText(string message)
    {
        if (interactionText)
        {
            interactionText.text = message;
        }
    }

    IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        interactionText.gameObject.SetActive(false);
    }

    public void SetHasKey(bool value)
    {
        playerHasKey = value;
    }
}
