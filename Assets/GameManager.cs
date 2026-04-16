using UnityEngine;
using System.Collections;
using UnityEditor;

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

    [Header("Stage Cutscenes")]
    public string introCutscene;   // plays at stage 0
    public string voidCutscene;    // plays at stage 6

    [Header("Stage Objects")]
    public GameObject bargainingManager; // assigned in Inspector, inactive by default

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
    }

    void Start()
    {
        if (currentStage == 0)
        {
            StartCoroutine(PlayStageCutscene(introCutscene));
        }
        else
        {
            currentStage -= 1;
            int levelStateNum = gameStageToLevelState(currentStage);
            for (int i = 0; i < currentStage; i++)
            {
                levelStates.NextLevel();
            }
            
            AdvanceStage();
        }

        Debug.Log("[GameManager] Entering stage of: " + currentStage);
        SkyManager.Instance.SetSky(gameStageToLevelState(currentStage));
        
    }

    // ----------------------------
    // CUTSCENE PLAYBACK
    // ----------------------------
    IEnumerator PlayStageCutscene(string cutsceneName)
    {
        SetCutsceneState(true);
        cutseneManager.Play(cutsceneName);

        yield return null;

        float timeout = 1f;
        float elapsed = 0f;
        while (!cutseneManager.videoPlayer.isPlaying && elapsed < timeout)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitUntil(() => !cutseneManager.videoPlayer.isPlaying);

        SetCutsceneState(false);

        if (currentStage == 0)
        {
            AdvanceStage();
            levelStates.StartDenial();
        }
    }



    // ----------------------------
    // CUTSCENE STATE
    // ----------------------------
    public void SetCutsceneState(bool state)
    {
        inCutscene = state;
        //if (currentPlayer != null)
        //    currentPlayer.SetActive(!state);
    }

    // ----------------------------
    // STAGE CONTROL
    // ----------------------------
    public void SetStage(int stage)
    {
        currentStage = Mathf.Clamp(stage, 0, 6);
    }

    public void AdvanceStage()
    {
        SetStage(currentStage + 1);

        if (!isInCutsceneStage())
        {
            Debug.Log("Entering Non-Cutscene");
            inCutscene = false;
            levelStates.NextLevel();
            SkyManager.Instance.SetSky(gameStageToLevelState(currentStage));
        }
        else
        {
            inCutscene = true;

            Debug.Log("Entering Cutscene");
            if (currentStage == 3)
            {
                levelStates.NextLevel();
                if (bargainingManager != null)
                    bargainingManager.SetActive(true);
            }
            else if (currentStage == 6)
            {
                Debug.Log("Playing Stage 6");
                StartCoroutine(PlayStageCutscene(voidCutscene));

            }
        }
    }

    // ----------------------------
    // RESPAWN
    // ----------------------------
    public void RespawnPlayer()
    {
        levelStates.ResetLevel();
    }

    bool isInCutsceneStage()
    {
        return currentStage == 0 || currentStage == 3 || currentStage == 6;
    }

    public int gameStageToLevelState(int level) {
        int levelStateNum = 0;

        switch (level) {
            case 0:
                levelStateNum = 1;
                break;
            case 1:
                levelStateNum = 1;
                break;
            case 2:
                levelStateNum = 2;
                break;
            case 3:
                levelStateNum = 3;
                break;
            case 4:
                levelStateNum = 4;
                break;
            case 5:
                levelStateNum = 5;
                break;
            case 6:
                levelStateNum = 5;
                break;
        }
            

        return levelStateNum;
    }
}