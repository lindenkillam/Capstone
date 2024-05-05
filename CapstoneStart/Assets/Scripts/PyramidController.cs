using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class PyramidController : MonoBehaviour
{
    public GameObject textPrefab;
    public Canvas canvas;
    public Renderer buttonRenderer;
    public Color emissionColor;
    public Light pyramidLight;

    public void ChangeLightIntensity()
    {
        StartCoroutine(ChangeIntensityOverTime());
    }

    IEnumerator ChangeIntensityOverTime()
    {
        float elapsedTime = 0f;
        float duration = 5f;
        float startIntensity = 0f;
        float targetIntensity = 4f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            pyramidLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        pyramidLight.intensity = targetIntensity;
    }

    public void InstantiateTextPrefab()
    {
        GameObject winText = Instantiate(textPrefab, canvas.transform, false);
        winText.GetComponent<TextMeshProUGUI>().text = "You escaped!!";

        RectTransform rectTransform = winText.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.zero;
    }
}
