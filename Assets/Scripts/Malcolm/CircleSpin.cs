using UnityEngine;

public class CircleSpin : MonoBehaviour
{
    public float radius = 3f;
    public float speed = 50f;

    private Vector3 centerPoint;

    [SerializeField] private bool spinCounterClockwise = false;

    void Start()
    {
        centerPoint = transform.position;

        // Move the object out from its own starting center
        transform.position = centerPoint + new Vector3(radius, 0f, 0f);
    }

    void Update()
    {
        float currentSpeed = spinCounterClockwise ? -speed : speed;
        transform.RotateAround(centerPoint, Vector3.up, currentSpeed * Time.deltaTime);
    }
}
