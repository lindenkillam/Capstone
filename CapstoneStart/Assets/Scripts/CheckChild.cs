using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CheckChild : MonoBehaviour
{
    public bool isPlayerTainted;
    public float fadeDuration = 5f;
    public GameObject ps;
    GameObject particle; 

    void Update()
    {
        if (transform.childCount > 0)
        {
            isPlayerTainted = true;
        }
        else
        {
            isPlayerTainted = false;
        }
    }

    public void CleansingTexts()
    {
        StartCoroutine(FadeCanvasGroup()); 
    }

    IEnumerator FadeCanvasGroup()
    {
        Transform randomChild = transform.GetChild(Random.Range(0, transform.childCount));

        CanvasGroup canvasGroup = randomChild.GetComponent<CanvasGroup>();

        TextMeshProUGUI textMeshProUGUI = randomChild.GetComponent<TextMeshProUGUI>();

        if (textMeshProUGUI != null)
        {
            RectTransform rectTransform = textMeshProUGUI.rectTransform;

            Vector3 position = rectTransform.position;

            Vector3 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, position);

            particle = Instantiate(ps, screenPosition, Quaternion.identity);
        }

        if (canvasGroup != null)
        {
            float elapsedTime = 0f;
            float startAlpha = 1f;
            float targetAlpha = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;

            Destroy(particle); 

            Destroy(randomChild.gameObject);
        }
    }

}
