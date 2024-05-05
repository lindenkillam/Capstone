using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DescriptionTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI descriptionText;
    public bool isNote;
    bool noteOpen; 
    public string description;
    public GameObject noteCanvas; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.gameObject.SetActive(true);
        descriptionText.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.gameObject.SetActive(false);
    }

    public void OnClick()
    {
        if (isNote && !noteOpen)
        {
            noteCanvas.SetActive(true);
            noteOpen = true; 
        }
        else if (noteOpen)
        {
            noteOpen = false; 
            noteCanvas.SetActive(false);
        }
    }
}
