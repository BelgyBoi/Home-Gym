using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 0.5f;

    void Awake()
    {
        if (fadeImage != null)
        {
            // Start fully transparent (black with 0 alpha)
            fadeImage.color = new Color(0, 0, 0, 0);
            fadeImage.raycastTarget = false; // So it doesn’t block UI
        }
    }

    public IEnumerator FadeOutIn(Action onFadeMidpoint)
    {
        yield return StartCoroutine(Fade(0f, 1f)); // Fade to black
        onFadeMidpoint?.Invoke();
        yield return StartCoroutine(Fade(1f, 0f)); // Fade back in
    }

    IEnumerator Fade(float fromAlpha, float toAlpha)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(fromAlpha, toAlpha, elapsed / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(color.r, color.g, color.b, toAlpha);
    }
}
