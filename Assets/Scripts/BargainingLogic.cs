using UnityEngine;
using System.Collections;
using TMPro;

public class BargainingSequence : MonoBehaviour
{
    private enum State
    {
        Intro,
        Choice1,
        AfterChoice1,
        Choice2,
        End
    }

    private State currentState;

    [Header("Choice UI Objects (SETS)")]
    public GameObject[] choiceObjects;
    // [0] = Choice Set 1
    // [1] = Choice Set 2

    [Header("Choice Labels")]
    public TMP_Text choiceAText;
    public TMP_Text choiceBText;
    public string[] choiceALabels; // text for Q option per choice set
    public string[] choiceBLabels; // text for E option per choice set

    [Header("Refs")]
    public CutsceneManager cutsceneManager;

    [Header("Choices")]
    public bool[] correctChoices;
    // true = Q, false = E

    [Header("Cutscenes")]
    public string introCutscene;
    public string[] successCutscenes;
    public string[] failureCutscenes;

    private bool isTransitioning;

    // ----------------------------
    // INIT SAFETY
    // ----------------------------
    void Awake()
    {
        HideAllChoices();
    }

    void Start()
    {
        StartSequence();
    }

    // ----------------------------
    // UPDATE
    // ----------------------------
    void Update()
    {
        if (isTransitioning) return;

        if (currentState == State.Choice1)
            HandleChoice(0, State.AfterChoice1);

        else if (currentState == State.Choice2)
            HandleChoice(1, State.End);

        else
            HideAllChoices(); // brute-force: hide every frame when no choice is active
    }

    void LogStatus(string context)
    {
        string choiceStatus = "";
        if (choiceObjects != null)
            for (int i = 0; i < choiceObjects.Length; i++)
                choiceStatus += $" | choiceObjects[{i}]={(choiceObjects[i] == null ? "NULL" : choiceObjects[i].activeSelf.ToString())}";
        else
            choiceStatus = " | choiceObjects array is NULL";

        Debug.Log($"[Bargaining] {context} | State={currentState} | isTransitioning={isTransitioning} | videoPlaying={cutsceneManager.videoPlayer.isPlaying}{choiceStatus}");
    }

    // ----------------------------
    // ENTRY
    // ----------------------------
    public void StartSequence()
    {
        currentState = State.Intro;

        HideAllChoices();

        GameManager.Instance.SetCutsceneState(true);
        cutsceneManager.Play(introCutscene);

        LogStatus("StartSequence — after Play()");

        StartCoroutine(WaitThenShowChoice(0, State.Choice1));
    }

    // ----------------------------
    // CORE FLOW
    // ----------------------------
    IEnumerator WaitThenShowChoice(int index, State nextState)
    {
        yield return null;
        LogStatus($"WaitThenShowChoice[{index}] — after 1 frame");

        // Wait for video to START (give it up to 1s), then wait for it to END
        float timeout = 1f;
        float elapsed = 0f;
        while (!cutsceneManager.videoPlayer.isPlaying && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        LogStatus($"WaitThenShowChoice[{index}] — video started (or timed out), waiting for end");

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);
        LogStatus($"WaitThenShowChoice[{index}] — video done, about to show choice");

        GameManager.Instance.SetCutsceneState(false);

        HideAllChoices();
        ShowChoice(index);

        currentState = nextState;
        isTransitioning = false;
        LogStatus($"WaitThenShowChoice[{index}] — choice shown, state={nextState}");
    }

    IEnumerator WaitThenRepeatChoice(int index, State repeatState)
    {
        yield return null;

        float timeout = 1f;
        float elapsed = 0f;
        while (!cutsceneManager.videoPlayer.isPlaying && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        HideAllChoices(); // safety
        ShowChoice(index);

        currentState = repeatState;
        isTransitioning = false;
    }

    IEnumerator EndSequence()
    {
        yield return null;

        float timeout = 1f;
        float elapsed = 0f;
        while (!cutsceneManager.videoPlayer.isPlaying && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        HideAllChoices();

        GameManager.Instance.AdvanceStage();
    }

    // ----------------------------
    // CHOICE HANDLING
    // ----------------------------
    void HandleChoice(int index, State successState)
    {
        if (isTransitioning) return;

        bool choseQ = Input.GetKeyDown(KeyCode.Q);
        bool choseE = Input.GetKeyDown(KeyCode.E);

        if (!(choseQ || choseE)) return;

        bool correct = (choseQ == correctChoices[index]);

        isTransitioning = true;
        GameManager.Instance.SetCutsceneState(true);

        HideAllChoices(); // ALWAYS hide immediately on input

        if (correct)
        {
            currentState = successState;

            cutsceneManager.Play(successCutscenes[index]);

            if (successState == State.AfterChoice1)
            {
                StartCoroutine(WaitThenShowChoice(1, State.Choice2));
            }
            else
            {
                StartCoroutine(EndSequence());
            }
        }
        else
        {
            cutsceneManager.Play(failureCutscenes[index]);
            State repeatState = (index == 0) ? State.Choice1 : State.Choice2;
            StartCoroutine(WaitThenRepeatChoice(index, repeatState));
        }
    }

    // ----------------------------
    // UI CONTROL
    // ----------------------------
    void ShowChoice(int index)
    {
        if (choiceObjects == null || index >= choiceObjects.Length) return;
        if (choiceObjects[index] == null) return;

        // Update labels for this choice set
        if (choiceAText != null && choiceALabels != null && index < choiceALabels.Length)
        {
            choiceAText.text = choiceALabels[index];
            choiceAText.gameObject.SetActive(true);
        }
        if (choiceBText != null && choiceBLabels != null && index < choiceBLabels.Length)
        {
            choiceBText.text = choiceBLabels[index];
            choiceBText.gameObject.SetActive(true);
        }

        choiceObjects[index].SetActive(true);
    }

    void HideAllChoices()
    {
        if (choiceObjects != null)
            for (int i = 0; i < choiceObjects.Length; i++)
                if (choiceObjects[i] != null)
                    choiceObjects[i].SetActive(false);

        if (choiceAText != null) choiceAText.gameObject.SetActive(false);
        if (choiceBText != null) choiceBText.gameObject.SetActive(false);
    }
}