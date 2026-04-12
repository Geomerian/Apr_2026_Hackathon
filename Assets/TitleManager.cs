using System.Collections;
using UnityEngine;
using TMPro;

public class TitleManager : MonoBehaviour
{
    [System.Serializable]
    public class StageTitleEntry
    {
        public int stage;
        public string title;
        public string subtitle;
    }

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;

    [Header("Stage Titles")]
    public StageTitleEntry[] stageTitles;

    [Header("Fade Settings")]
    public float fadeInTime = 0.5f;
    public float holdTime = 2f;
    public float fadeOutTime = 0.5f;

    private Coroutine routine;
    private int currentStage = -1;

    void Start()
    {
        SetAlpha(0);
        StartCoroutine(StageWatcher());
    }

    IEnumerator StageWatcher()
    {
        while (true)
        {
            var gm = GameManager.Instance;

            if (gm != null && gm.currentStage != currentStage)
            {
                currentStage = gm.currentStage;
                ShowForStage(currentStage);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }

    // AUTO: find title by stage
    void ShowForStage(int stage)
    {
        for (int i = 0; i < stageTitles.Length; i++)
        {
            if (stageTitles[i].stage == stage)
            {
                Show(stageTitles[i].title, stageTitles[i].subtitle);
                return;
            }
        }

        // no match = do nothing (intentional)
    }

    // MANUAL CALL STILL WORKS
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
        yield return Fade(0f, 1f, fadeInTime);
        yield return new WaitForSeconds(holdTime);
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