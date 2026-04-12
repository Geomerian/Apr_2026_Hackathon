using UnityEngine;

public class WaterRiser : MonoBehaviour
{
    public Transform WaterPlane;
    public float RiseSpeed = 0.5f;

    public GameObject Player;
    public Transform Camera;

    private float submersion = 0.0f;
    public float lungCapacity = 3.0f;
    private float breathTimer = 0.0f;


    private void Update()
    {
        float newY = WaterPlane.position.y + RiseSpeed * Time.deltaTime;
        WaterPlane.position = new Vector3(WaterPlane.position.x, newY, WaterPlane.position.z);

        if (Player != null && Camera != null)
        {
            float feetY = Player.transform.position.y;
            float headY = feetY + Camera.localPosition.y;
            float waterY = WaterPlane.position.y;

            // Ensure feetY is less than headY
            if (feetY > headY)
            {
                float temp = feetY;
                feetY = headY;
                headY = temp;
            }

            float bodyHeight = headY - feetY;
            float submergedHeight = waterY - feetY;

            if (submergedHeight <= 0)
                submersion = 0.0f;
            else if (submergedHeight >= bodyHeight)
                submersion = 1.0f;
            else
                submersion = submergedHeight / bodyHeight;

            if (submersion > 0.9f)
            {
                breathTimer += Time.deltaTime;
                if (breathTimer >= lungCapacity)
                {
                    Destroy(Player);
                }
            }
            else
            {
                breathTimer = 0.0f;
            }

            Player.GetComponent<PlayerMovement>().ApplyMultiplier(submersion);
        }
    }

    public void DenialSetup()
    {
        WaterPlane.position = new Vector3(WaterPlane.position.x, -4f, WaterPlane.position.z);
        RiseSpeed = 0.1f;
    }

    public void AcceptanceSetup()
    {
        WaterPlane.position = new Vector3(WaterPlane.position.x, 7f, WaterPlane.position.z);
        RiseSpeed = -0.1f;
    }
}
