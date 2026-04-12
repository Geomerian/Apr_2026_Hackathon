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
    // 3 = bargaining (cutscene)
    // 4 = depression
    // 5 = acceptance 
    // 6 = void (cutscene)

    [Header("Cutscene Manager")]
    public CutsceneManager cutseneManager;
    public LevelStates levelStates;

    void Awake()
    {
        // Singleton setup
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        if(isInCutsceneStage())
        {
            // play appropriate cutscene
        }
    }

    void Start()
    {
        
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
        if(!isInCutsceneStage())
        {
            inCutscene = false;
            levelStates.NextLevel();
        } else 
        {
            inCutscene = true;
            if(currentStage == 3)
            {
                levelStates.NextLevel();
            }
            // make appropriate calls to CutsceneManager
        }
    }

    // respawn system
    public void RespawnPlayer()
    {
        levelStates.ResetLevel();
    }

    bool isInCutsceneStage()
    {
        if(currentStage == 0 || currentStage == 3 || currentStage == 6)
        {
            return true;
        }
        return false;
    }

}