using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public GameManager gameManager;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.AdvanceStage();
        }
    }
}
