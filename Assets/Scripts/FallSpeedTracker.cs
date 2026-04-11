using UnityEngine;

public class FallSpeedTracker : MonoBehaviour
{
    private float downwardSpeed;

    private Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        downwardSpeed = Mathf.Max(0f, -rb.linearVelocity.y);
    }

    public float GetDownwardSpeed()
    {
        return downwardSpeed;
    }
}
