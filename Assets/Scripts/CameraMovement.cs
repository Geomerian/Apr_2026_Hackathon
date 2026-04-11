using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTransform;

    public float cameraBobAmount = 0.05f;
    public float bobbingFrequency = 6f;
    public float smoothTime = 0.08f;

    private float canBobTimer = 0f;

    private Vector3 originalPosition;
    private Vector3 currentVelocity;
    private Vector3 targetPosition;

    private PlayerMovement movement;

    private void Start()
    {
        if (cameraTransform == null)
            cameraTransform = transform;

        originalPosition = cameraTransform.localPosition;
        targetPosition = originalPosition;

        movement = GetComponentInParent<PlayerMovement>();
    }

    private void Update()
    {
        if (!cameraTransform || !movement)
            return;

        if (movement.IsCrouching() || movement.InAir())
        {
            targetPosition = originalPosition;
        }
        else if (movement.IsMoving())
        {
            canBobTimer += Time.deltaTime;

            if (canBobTimer > 0.25f)
            {
                float bobbingOffset = Mathf.Sin(Time.time * bobbingFrequency) * cameraBobAmount;

                targetPosition = new Vector3(
                    originalPosition.x,
                    originalPosition.y + bobbingOffset,
                    originalPosition.z
                );
            }
        }
        else
        {
            canBobTimer = 0f;
            targetPosition = originalPosition;
        }

        cameraTransform.localPosition = Vector3.SmoothDamp(
            cameraTransform.localPosition,
            targetPosition,
            ref currentVelocity,
            smoothTime
        );
    }
}