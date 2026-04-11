using UnityEngine;

public class ScaleCollider : MonoBehaviour
{
    public float scaleFactor = 1.0f;

    private Vector3 originalSize;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            // Scale the collider's size by the specified factor
            if (collider is BoxCollider boxCollider)
            {
                originalSize = boxCollider.size;
            }
            else if (collider is SphereCollider sphereCollider)
            {
                originalSize = Vector3.one * sphereCollider.radius;
            }
            else if (collider is CapsuleCollider capsuleCollider)
            {
                originalSize = new Vector3(capsuleCollider.radius, capsuleCollider.height, capsuleCollider.radius);
            }
            else if (collider is MeshCollider meshCollider)
            {
                // For MeshColliders, you may need to scale the mesh itself
                MeshFilter meshFilter = GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.mesh;
                    Vector3[] vertices = mesh.vertices;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i] *= scaleFactor;
                    }
                    mesh.vertices = vertices;
                    mesh.RecalculateBounds();
                }
            }
        }
        else
        {
            Debug.LogWarning("No Collider component found on this GameObject.");
        }
    }

    void Update()
    { 
        // Get the Collider component attached to this GameObject
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            // Scale the collider's size by the specified factor
            if (collider is BoxCollider boxCollider)
            {
                boxCollider.size *= scaleFactor;
            }
            else if (collider is SphereCollider sphereCollider)
            {
                sphereCollider.radius *= scaleFactor;
            }
            else if (collider is CapsuleCollider capsuleCollider)
            {
                capsuleCollider.radius *= scaleFactor;
                capsuleCollider.height *= scaleFactor;
            }
            else if (collider is MeshCollider meshCollider)
            {
                // For MeshColliders, you may need to scale the mesh itself
                MeshFilter meshFilter = GetComponent<MeshFilter>();
                if (meshFilter != null)
                {
                    Mesh mesh = meshFilter.mesh;
                    Vector3[] vertices = mesh.vertices;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        vertices[i] *= scaleFactor;
                    }
                    mesh.vertices = vertices;
                    mesh.RecalculateBounds();
                }
            }
        }
        else
        {
            Debug.LogWarning("No Collider component found on this GameObject.");
        }
    }
}
