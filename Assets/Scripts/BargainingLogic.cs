using UnityEngine;
using System.Collections;

public class BargainingSequence : MonoBehaviour
{
    private enum State
    {
        Intro,
        Choice1,
        Choice2,
        End
    }

    private State currentState;
    private bool isRunning = false;

    [Header("Refs")]
    public CutsceneManager cutsceneManager;

    [Header("Images")]
    public Texture[] choiceImages;

    [Header("Correct Choices")]
    public bool[] correctChoices;

    [Header("Cutscenes")]
    public string introCutscene;
    public string[] successCutscenes;
    public string[] failureCutscenes;

    public void StartSequence()
    {
        Debug.Log("[BARGAINING] StartSequence CALLED");

        if (isRunning)
        {
            Debug.Log("[BARGAINING] Already running, abort");
            return;
        }

        isRunning = true;

        if (cutsceneManager == null)
        {
            Debug.LogError("[BARGAINING] CutsceneManager is NULL");
            return;
        }

        Debug.Log("[BARGAINING] Playing intro cutscene: " + introCutscene);

        currentState = State.Intro;

        GameManager.Instance.SetCutsceneState(true);
        cutsceneManager.Play(introCutscene);

        StartCoroutine(WaitIntroThenChoice1());
    }

    void Update()
    {
        if (!isRunning) return;

        if (currentState == State.Choice1)
            HandleChoice(0);

        else if (currentState == State.Choice2)
            HandleChoice(1);
    }

    // ----------------------------
    // INTRO → CHOICE 1
    // ----------------------------
    IEnumerator WaitIntroThenChoice1()
    {
        Debug.Log("[BARGAINING] Waiting for intro cutscene to finish...");

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        Debug.Log("[BARGAINING] Intro finished");

        GameManager.Instance.SetCutsceneState(false);

        if (choiceImages == null || choiceImages.Length < 1)
        {
            Debug.LogError("[BARGAINING] choiceImages[0] missing!");
            yield break;
        }

        if (choiceImages[0] == null)
        {
            Debug.LogError("[BARGAINING] choiceImages[0] is NULL");
            yield break;
        }

        Debug.Log("[BARGAINING] Showing Choice 1 image");

        cutsceneManager.ShowImage(choiceImages[0]);
        currentState = State.Choice1;
    }

    // ----------------------------
    // CHOICE 2 ENTRY
    // ----------------------------
    IEnumerator WaitChoice2()
    {
        Debug.Log("[BARGAINING] Waiting for success cutscene (choice 1)...");

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        if (choiceImages.Length < 2 || choiceImages[1] == null)
        {
            Debug.LogError("[BARGAINING] choiceImages[1] missing/null");
            yield break;
        }

        Debug.Log("[BARGAINING] Showing Choice 2 image");

        cutsceneManager.ShowImage(choiceImages[1]);
        currentState = State.Choice2;
    }

    // ----------------------------
    // INPUT HANDLING
    // ----------------------------
    void HandleChoice(int index)
    {
        bool choseQ = Input.GetKeyDown(KeyCode.Q);
        bool choseE = Input.GetKeyDown(KeyCode.E);

        if (!(choseQ || choseE)) return;

        Debug.Log("[BARGAINING] Input detected at index " + index + " Q=" + choseQ + " E=" + choseE);

        if (correctChoices == null || correctChoices.Length <= index)
        {
            Debug.LogError("[BARGAINING] correctChoices missing index " + index);
            return;
        }

        bool correct = (choseQ == correctChoices[index]);

        Debug.Log("[BARGAINING] Choice " + index + " correct? " + correct);

        if (correct)
        {
            GameManager.Instance.SetCutsceneState(true);

            if (successCutscenes == null || successCutscenes.Length <= index)
            {
                Debug.LogError("[BARGAINING] successCutscenes missing index " + index);
                return;
            }

            Debug.Log("[BARGAINING] Playing SUCCESS cutscene: " + successCutscenes[index]);

            cutsceneManager.Play(successCutscenes[index]);

            if (index == 0)
                StartCoroutine(WaitChoice2());
            else
                StartCoroutine(EndSequence());
        }
        else
        {
            GameManager.Instance.SetCutsceneState(true);

            if (failureCutscenes == null || failureCutscenes.Length <= index)
            {
                Debug.LogError("[BARGAINING] failureCutscenes missing index " + index);
                return;
            }

            Debug.Log("[BARGAINING] Playing FAILURE cutscene: " + failureCutscenes[index]);

            cutsceneManager.Play(failureCutscenes[index]);

            if (index == 0)
                StartCoroutine(ReturnToChoice1());
            else
                StartCoroutine(ReturnToChoice2());
        }
    }

    IEnumerator ReturnToChoice1()
    {
        Debug.Log("[BARGAINING] Returning to Choice 1...");

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        cutsceneManager.ShowImage(choiceImages[0]);
        currentState = State.Choice1;
    }

    IEnumerator ReturnToChoice2()
    {
        Debug.Log("[BARGAINING] Returning to Choice 2...");

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        cutsceneManager.ShowImage(choiceImages[1]);
        currentState = State.Choice2;
    }

    IEnumerator EndSequence()
    {
        Debug.Log("[BARGAINING] Ending sequence...");

        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        cutsceneManager.HideImage();

        GameManager.Instance.AdvanceStage();
    }
}