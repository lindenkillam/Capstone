using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using TMPro; 

public class CheckChild : MonoBehaviour
{
    public bool isPlayerTainted;
    public float fadeDuration = 5f;
    Canvas myCanvas; 

    void Start()
    {
        myCanvas = GetComponent<Canvas>(); 
    }

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
        StartCoroutine(FadeCanvasGroups()); 
    }

    IEnumerator FadeCanvasGroups()
    {
        if (transform.childCount < 2)
        {
            Transform onlyChild = transform.GetChild(0);
            StartCoroutine(FadeAndDestroy(onlyChild)); 
        }

        if (transform.childCount < 1)
        {
            Debug.LogWarning("Not enough children to select.");
            yield break;
        }

        int firstIndex = Random.Range(0, transform.childCount);
        int secondIndex;
        do
        {
            secondIndex = Random.Range(0, transform.childCount);
        } while (secondIndex == firstIndex);

        Transform firstChild = transform.GetChild(firstIndex);
        Transform secondChild = transform.GetChild(secondIndex);

        StartCoroutine(FadeAndDestroy(firstChild));
        StartCoroutine(FadeAndDestroy(secondChild));
    }

    IEnumerator FadeAndDestroy(Transform child)
    {
        CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
        TextMeshProUGUI textMeshProUGUI = child.GetComponent<TextMeshProUGUI>();
        ParticleSystem ps = textMeshProUGUI?.transform.GetComponentInChildren<ParticleSystem>();

        if (canvasGroup != null)
        {
            float elapsedTime = 0f;
            float startAlpha = 1f;
            float targetAlpha = 0f;

            //play the particle system if it's not null
            ps?.Play();

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            canvasGroup.alpha = targetAlpha;

            yield return new WaitForSeconds(0.5f);
            Destroy(child.gameObject);
        }
        else
        {
            Debug.LogWarning("The child does not have a CanvasGroup component.");
        }
    }
}
