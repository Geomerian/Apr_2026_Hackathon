using UnityEngine;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination;


    private void OnTriggerEnter(Collider other)
    {
        if(teleportDestination == null) return;
        other.transform.position = teleportDestination.position;
        Rigidbody rb = GetComponent<Rigidbody>();
        if(rb) rb.linearVelocity = Vector3.zero;
    }
}
