using System.Collections;
using UnityEngine;

public class BreakableBranch : MonoBehaviour
{
    public float breakForce = 10f; // Force required to break the branch

    private void OnCollisionEnter(Collision collision)
    {
        //Check if tag is player
        if (!collision.collider.CompareTag("Player")) return;

        // Check if the colliding object has a Rigidbody and is not kinematic
        FallSpeedTracker speed = collision.collider.GetComponent<FallSpeedTracker>();
        if (!speed) return;

        Debug.Log("Downward Speed: " + speed.GetDownwardSpeed());

        // If the impact force exceeds the break force, destroy the branch
        if (speed.GetDownwardSpeed() > breakForce)
        {
            StartCoroutine(TurnOnAfterDelay(2f)); // Turn on after 2 seconds
        }
    }
    IEnumerator TurnOnAfterDelay(float delay)
    {
        SetVisuals(false); // Hide the branch immediately
        SetColliders(false); // Disable colliders immediately
        yield return new WaitForSeconds(delay);
        SetVisuals(true); // Show the branch again after the delay
        SetColliders(true); // Enable colliders again after the delay
    }

    void SetColliders(bool enabled)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = enabled;
        }
    }

    void SetVisuals(bool enabled)
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
            renderer.enabled = enabled;
        }
    }
}
