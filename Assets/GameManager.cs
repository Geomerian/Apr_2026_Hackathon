using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public GameObject currentPlayer;

    [Header("State")]
    public bool inCutscene = false;
    public bool hasGilbertSoul = false;

    public int currentStage = 0;
    // 0 = intro (cutscene)
    // 1 = denial
    // 2 = anger
    // 3 = bargaining (cutscene + sequence)
    // 4 = depression
    // 5 = acceptance
    // 6 = void (cutscene)

    [Header("Managers")]
    public CutsceneManager cutseneManager;
    public LevelStates levelStates;
    public BargainingSequence bargainingSequence;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        ApplyStage(currentStage);
    }

    // ----------------------------
    // CUTSCENE STATE (NO PLAYER DISABLE)
    // ----------------------------
    public void SetCutsceneState(bool state)
    {
        inCutscene = state;

        // IMPORTANT:
        // Player is NOT disabled anymore.
        // Systems must respect inCutscene instead.
    }

    // ----------------------------
    // STAGE CONTROL
    // ----------------------------
    public void SetStage(int stage)
    {
        currentStage = Mathf.Clamp(stage, 0, 6);
        ApplyStage(currentStage);
    }

    public void AdvanceStage()
    {
        SetStage(currentStage + 1);
    }

    // ----------------------------
    // CORE ROUTING
    // ----------------------------
    void ApplyStage(int stage)
    {
        switch (stage)
        {
            // ---------------- INTRO ----------------
            case 0:
                SetCutsceneState(true);
                cutseneManager.Play("Intro");
                break;

            // ---------------- DENIAL ----------------
            case 1:
                SetCutsceneState(false);
                levelStates.currentState = LevelState.Denial;
                levelStates.ResetLevel();
                break;

            // ---------------- ANGER ----------------
            case 2:
                SetCutsceneState(false);
                levelStates.currentState = LevelState.Anger;
                levelStates.ResetLevel();
                break;

            // ---------------- BARGAINING ----------------
            case 3:
                SetCutsceneState(true);

                levelStates.currentState = LevelState.Bargaining;
                levelStates.ResetLevel();

                if (bargainingSequence != null)
                    bargainingSequence.StartSequence();
                else
                    Debug.LogWarning("BargainingSequence not assigned in GameManager");

                break;

            // ---------------- DEPRESSION ----------------
            case 4:
                SetCutsceneState(false);
                levelStates.currentState = LevelState.Depression;
                levelStates.ResetLevel();
                break;

            // ---------------- ACCEPTANCE ----------------
            case 5:
                SetCutsceneState(false);
                levelStates.currentState = LevelState.Acceptance;
                levelStates.ResetLevel();
                break;

            // ---------------- VOID ----------------
            case 6:
                SetCutsceneState(true);
                cutseneManager.Play("Void");
                break;
        }
    }

    // ----------------------------
    // RESPAWN
    // ----------------------------
    public void RespawnPlayer()
    {
        levelStates.ResetLevel();
    }
}