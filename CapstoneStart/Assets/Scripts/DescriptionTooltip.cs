using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DescriptionTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI descriptionText;
    public string description;

    public void OnPointerEnter(PointerEventData eventData)
    {
        descriptionText.gameObject.SetActive(true);
        descriptionText.text = description;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        descriptionText.gameObject.SetActive(false);
    }

}
