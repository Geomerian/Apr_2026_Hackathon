using UnityEngine;

public class KillerBranch : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;

        LevelStates.Instance.ResetLevel();
    }
}
