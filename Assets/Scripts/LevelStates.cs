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
    public LevelState currentState = LevelState.Denial;
    public Vector3[] levelPositions;

    public GameObject player;

    private void Start()
    {
        player.transform.position = levelPositions[(int)currentState];
    }

    public void NextLevel()
    {
        if (levelPositions == null || levelPositions.Length == 0)
        {
            Debug.LogError("Level positions not set!");
            return;
        }

        if ((int)currentState < levelPositions.Length - 1)
        {
            currentState++;
            player.transform.position = levelPositions[(int)currentState];
            Rigidbody playerRb = player.GetComponent<Rigidbody>();
            if (playerRb) playerRb.angularVelocity = Vector3.zero;
            Debug.Log("Moving to level: " + currentState);
        }
        else
        {
            Debug.Log("Game Over");
        }
    }

    public void ResetLevel()
    {
        player.transform.position = levelPositions[(int)currentState];
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb) playerRb.angularVelocity = Vector3.zero;
        Debug.Log("Level reset to: " + currentState);

        //reset hazards/enemies depending on level
    }
}
