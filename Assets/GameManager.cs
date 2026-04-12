using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Player")]
    public GameObject playerPrefab;
    public GameObject currentPlayer;

    [Header("State")]
    public bool inCutscene = false;
    public int currentStage = 0; // 0 - 3 (4 stages total)

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
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        SpawnPlayerAtStage(currentStage);
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
        currentStage = Mathf.Clamp(stage, 0, 3);
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

        SpawnPlayerAtStage(currentStage);
    }

    void SpawnPlayerAtStage(int stage)
    {
        if (playerPrefab == null) return;

        Transform spawnPoint = stageRespawns[stage];

        currentPlayer = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}