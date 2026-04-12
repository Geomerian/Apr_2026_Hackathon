using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    public GameObject levelStates;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            levelStates.GetComponent<LevelStates>().ResetLevel();
        }
    }
}
