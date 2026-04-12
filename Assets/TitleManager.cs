using System.Collections;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    [Header("Fade Settings")]
    public float fadeInTime = 0.5f;
    public float holdTime = 2f;
    public float fadeOutTime = 0.5f;

    Coroutine routine;

    void Start()
    {
        SetAlpha(0);
    }

    public void Show(string title, string subtitle = "")
    {
        if (routine != null)
            StopCoroutine(routine);

        titleText.text = title;
        subtitleText.text = subtitle;

        routine = StartCoroutine(FadeSequence());
    }

    IEnumerator FadeSequence()
    {
        // Fade in
        yield return Fade(0f, 1f, fadeInTime);

        // Hold
        yield return new WaitForSeconds(holdTime);

        // Fade out
        yield return Fade(1f, 0f, fadeOutTime);
    }

    IEnumerator Fade(float start, float end, float duration)
    {
        float t = 0f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(start, end, t / duration);
            SetAlpha(a);
            yield return null;
        }

        SetAlpha(end);
    }

    void SetAlpha(float a)
    {
        Color c1 = titleText.color;
        Color c2 = subtitleText.color;

        c1.a = a;
        c2.a = a;

        titleText.color = c1;
        subtitleText.color = c2;
    }
}