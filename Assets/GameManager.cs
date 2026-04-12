using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
    }

    // respawn system
    public void RespawnPlayer()
    {
        if (currentPlayer != null)
            Destroy(currentPlayer);

        RespawnPlayer(currentStage);
    }

    void RespawnPlayer(int stage)
    {

        Transform spawnPoint = stageRespawns[stage];

        // do teleportation here
    }
}