using UnityEngine;

public class ChasePlayer : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public float speed = 5f; // Speed at which the enemy chases the player

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player not assigned!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!player) return;

        // Calculate the direction from the enemy to the player
        Vector3 direction = (player.position - transform.position).normalized;
        // Move the enemy towards the player
        transform.position += direction * speed * Time.deltaTime;

    }
}
