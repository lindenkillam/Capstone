using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class NoteManager : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] private KeyCode closeKey;
    [SerializeField] private CharacterController player;
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] [TextArea] private string noteText;
    MouseLook mouseLook;

    public void ShowNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        openEvent.Invoke();
        //mouseLook.mouseSensitivity = 0f;
        player.enabled = false;
        isOpen = true;
    }

    public void DisableNote()
    {
        if(isOpen)
        {
            noteCanvas.SetActive(false);
            //mouseLook.mouseSensitivity = 500f;
            player.enabled = true;
            isOpen = false;
        }
    }

    public bool isNoteActive()
    {
        return isOpen;
    }

    void DisablePlayer(bool disable)
    {
        player.enabled = !disable;
    }

    // Update is called once per frame
    void Update()
    {
        if(isOpen)
        {
            if(Input.GetKeyDown(closeKey))
            {
                DisableNote();
            }
        }
    }
}
