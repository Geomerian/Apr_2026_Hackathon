using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public LevelStates levelStates;

    [Header("Player")]
    public GameObject currentPlayer;

    [Header("State")]
    public bool inCutscene = false;
    public int currentStage = 0;
    // 0 = intro (cutscene)
    // 1 = denial
    // 2 = anger
    // 3 = bargaining (cutscene)
    // 4 = depression
    // 5 = acceptance 
    // 6 = void (cutscene)

    [Header("Respawns")]
    public Transform[] stageRespawns = new Transform[4];

    private MainMenu mainMenu;

    void Awake()
    {
        // Singleton setup
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        mainMenu = FindObjectOfType<MainMenu>();
        currentStage = mainMenu.levelChosen;
        RespawnPlayer(currentStage);
    }

    // cutscene
    public void SetCutsceneState(bool state)
    {
        inCutscene = state;

        if (currentPlayer != null)
            currentPlayer.SetActive(!state);
    }

    // stage control
    public void SetStage(int stage)
    {
        currentStage = Mathf.Clamp(stage, 0, 6);
    }

    public void AdvanceStage()
    {
        SetStage(currentStage + 1);
        if(isCurrentlyCutscene())
        {
            if (currentStage == 1) { 
                levelStates.StartDenial();
            }
            if (currentStage == 3) {
                levelStates.NextLevel();
            }
            SetCutsceneState(true);
        }
        else
        {
            levelStates.NextLevel();
            SetCutsceneState(false);
        }
    }

    // respawn system
    public void RespawnPlayer()
    {
        levelStates.ResetLevel();
    }

    void RespawnPlayer(int stage)
    {
        Transform spawnPoint = stageRespawns[stage];

        // do teleportation here
    }

    bool isCurrentlyCutscene()
    {
        if (currentStage == 0 || currentStage == 3 || currentStage == 6)
        {
            return true;
        }
        return false;
    }
}