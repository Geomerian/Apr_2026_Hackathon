using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform cameraTransform;

    public float cameraBobAmount = 0.05f;
    public float bobbingFrequency = 1f;
    
    private bool canBob = false;
    private float canBobTimer = 0f;

    private void Update()
    {
        if (cameraTransform == null)
            return;

        CheckMovement();

        if (canBob) canBobTimer += Time.deltaTime;
        if (canBobTimer > 0.25f)
        {
            float bobbingOffset = Mathf.Sin(Time.time * bobbingFrequency) * cameraBobAmount;
            cameraTransform.localPosition = new Vector3(
                cameraTransform.localPosition.x,
                cameraTransform.localPosition.y + bobbingOffset,
                cameraTransform.localPosition.z);
        }
            
    }

    private void CheckMovement()
    {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.LeftControl))
        {
            canBob = false;
            canBobTimer = 0f;
        }
        else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            canBob = true;
        else
        {
            canBob = false;
            canBobTimer = 0f;
        }
    }
}
