using UnityEngine;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class NoteManager : MonoBehaviour
{
    public bool isOpen = false;
    [SerializeField] private KeyCode closeKey;
    [SerializeField] private CharacterController player;
    [SerializeField] private GameObject noteCanvas;
    [SerializeField] private TMP_Text noteTextAreaUI;
    [SerializeField] private UnityEvent openEvent;
    [SerializeField] [TextArea] private string noteText;
    public CanvasGroup cg;
    public GameObject note;
    public TextMeshProUGUI onKeyObtainText;
    bool noteObtained; 

    private bool mFaded = false;
    public float Duration = 1f;
    //[SerializeField] MouseLook mouseLook;
    //[SerializeField] private AudioSource audioSource;

    void Start()
    {
        //audioSource = this.GetComponent<AudioSource>();
    }

    public void ShowNote()
    {
        noteTextAreaUI.text = noteText;
        noteCanvas.SetActive(true);
        //audioSource.Play();
        openEvent.Invoke();
        //mouseLook.mouseSensitivity = 0f;
        player.enabled = false;
        isOpen = true;
    }

    public IEnumerator DisableNote()
    {
        if(isOpen)
        {
            noteCanvas.SetActive(false);
            //mouseLook.mouseSensitivity = 500f;
            player.enabled = true;

            note.SetActive(false);
            if (!noteObtained)
            {
                onKeyObtainText.text = note.name.ToString() + " obtained";
                FadeIn();
                yield return new WaitForSeconds(2f);
                FadeOut();
                noteObtained = true; 
            }
            isOpen = false;
        }
    }

    public void OnNoteImageClick()
    {
        if (!isOpen)
        {
            noteCanvas.SetActive(true);
            isOpen = true;
        }
        else if (isOpen)
        {
            isOpen = false;
            noteCanvas.SetActive(false);
        }
    }

    public void FadeIn()
    {
        StartCoroutine(ActionOne(cg, cg.alpha, mFaded ? 0 : 1));
    }

    public void FadeOut()
    {
        StartCoroutine(ActionOne(cg, cg.alpha, mFaded ? 1 : 0));
    }

    public IEnumerator ActionOne(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        yield return new WaitForSeconds(0.5f);
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
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

    void Update()
    {
        /*if(Input.GetKeyDown(closeKey))
        {
            DisableNote();
        }*/
    }
}
