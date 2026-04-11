using UnityEngine;

public class BreakableBranch : MonoBehaviour
{
    public float breakForce = 10f; // Force required to break the branch

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has a Rigidbody and is not kinematic
        FallSpeedTracker speed = collision.collider.GetComponent<FallSpeedTracker>();
        if (!speed) return;

        //Debug.Log("Downward Speed: " + speed.GetDownwardSpeed());

        // If the impact force exceeds the break force, destroy the branch
        if (speed.GetDownwardSpeed() > breakForce)
        {
            Destroy(gameObject);
        }

    }
}
