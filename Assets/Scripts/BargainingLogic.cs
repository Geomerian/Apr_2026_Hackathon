using UnityEngine;
using System.Collections;

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

    [Header("Refs")]
    public CutsceneManager cutsceneManager;

    [Header("Images")]
    public Texture[] choiceImages;
    // [0] = choicePNG1
    // [1] = choicePNG2

    [Header("Choices")]
    public bool[] correctChoices;
    // true = Q, false = E

    [Header("Cutscene Names")]
    public string introCutscene;        // "BargainingIntro"
    public string[] successCutscenes;   // [0] = "Bargaining1A", [1] = "Bargaining2A"

    void Start()
    {
        StartSequence();
    }

    void Update()
    {
        if (currentState == State.Choice1)
            HandleChoice(0, State.AfterChoice1);

        else if (currentState == State.Choice2)
            HandleChoice(1, State.End);
    }

    // ----------------------------
    // ENTRY
    // ----------------------------
    public void StartSequence()
    {
        currentState = State.Intro;

        GameManager.Instance.SetCutsceneState(true);
        cutsceneManager.Play(introCutscene);

        StartCoroutine(WaitThenShowChoice(0, State.Choice1));
    }

    // ----------------------------
    // FLOW
    // ----------------------------
    IEnumerator WaitThenShowChoice(int imageIndex, State nextState)
    {
        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        cutsceneManager.ShowImage(choiceImages[imageIndex]);

        currentState = nextState;
    }

    void HandleChoice(int index, State successState)
    {
        bool choseQ = Input.GetKeyDown(KeyCode.Q);
        bool choseE = Input.GetKeyDown(KeyCode.E);

        if (!(choseQ || choseE)) return;

        bool correct = (choseQ == correctChoices[index]);

        if (correct)
        {
            currentState = successState;

            GameManager.Instance.SetCutsceneState(true);
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
            // WRONG → just re-show same image
            cutsceneManager.ShowImage(choiceImages[index]);
        }
    }

    // ----------------------------
    // END
    // ----------------------------
    IEnumerator EndSequence()
    {
        yield return new WaitUntil(() => !cutsceneManager.videoPlayer.isPlaying);

        GameManager.Instance.SetCutsceneState(false);

        cutsceneManager.HideImage();

        GameManager.Instance.AdvanceStage();
    }
}