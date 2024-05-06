using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChild : MonoBehaviour
{
    public bool isPlayerTainted;
    public float fadeDuration = 5f;

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

            Destroy(randomChild.gameObject);
        }
    }

}
