using System.Collections.Generic;
using UnityEngine;

public enum LevelState
{
    Denial,
    Anger,
    Bargaining,
    Depression,
    Acceptance
}

public class LevelStates : MonoBehaviour
{
    public LevelState currentState;
    public Vector3[] levelPositions;

    public GameObject player;

    public GameObject denialAcceptance;

    //Singleton
    public static LevelStates Instance;

    //public GameObject angerDepression;

    private void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void StartDenial()
    {
        currentState = LevelState.Denial;
        denialAcceptance.GetComponent<WaterRiser>().DenialSetup();
        player.transform.position = levelPositions[(int)currentState];
    }

    public void NextLevel()
    {
        if (levelPositions == null || levelPositions.Length == 0)
        {
            Debug.LogError("Level positions not set!");
            return;
        }

        //if ((int)currentState < levelPositions.Length - 1)
        //{
        //    currentState++;
        //    player.transform.position = levelPositions[(int)currentState];
        //    Rigidbody playerRb = player.GetComponent<Rigidbody>();
        //    if (playerRb) playerRb.angularVelocity = Vector3.zero;
        //    Debug.Log("Moving to level: " + currentState);
        //}
        //else
        //{
        //    Debug.Log("Game Over");
        //}

        switch (currentState)
        {
            case LevelState.Denial:
                currentState = LevelState.Anger;
                // angerdepression
                player.transform.position = levelPositions[(int)currentState];
                break;
            case LevelState.Anger:
                currentState = LevelState.Bargaining;
                // cutscenes
                player.transform.position = levelPositions[(int)currentState]; // player tp'ed to somewhere far
                break;
            case LevelState.Bargaining:
                currentState = LevelState.Depression;
                // angerdepression
                player.transform.position = levelPositions[(int)currentState];
                player.GetComponent<PlayerMovement>().maryPoppinsMode = true;
                break;
            case LevelState.Depression:
                player.GetComponent<PlayerMovement>().maryPoppinsMode = false;
                currentState = LevelState.Acceptance;
                denialAcceptance.GetComponent<WaterRiser>().AcceptanceSetup();
                player.transform.position = levelPositions[(int)currentState];
                break;
            case LevelState.Acceptance:
                Debug.Log("Game Over");
                // cutscenes
                return;
        }
        Debug.Log("Moving to level: " + currentState);
    }


    public void ResetLevel()
    {
        if (currentState == LevelState.Denial)
        {
            StartDenial();
        }
        else
        {
            currentState--;
            NextLevel();
        }
    }
}
