using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public LevelStates levelStates;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelStates.NextLevel();
        }
    }
}
