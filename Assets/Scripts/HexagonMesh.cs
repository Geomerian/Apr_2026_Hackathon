using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexagonMesh : MonoBehaviour
{
    public float radius = 5f;
    public float height = 1f;

    void Start()
    {
        GenerateMesh();
    }

    private void OnValidate()
    {
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        Vector3[] vertices = new Vector3[14];
        vertices[0] = new Vector3(0, height / 2, 0);
        vertices[7] = new Vector3(0, -height / 2, 0);
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.PI / 3 * i;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            vertices[i + 1] = new Vector3(x, height / 2, z);
            vertices[i + 8] = new Vector3(x, -height / 2, z);
        }

        int[] triangles = new int[6 * 3 + 6 * 3 + 6 * 3 * 2];

        int j = 0;
        for (int i = 0; i < 6; i++)
        {
            triangles[j++] = 0;
            triangles[j++] = (i + 1) % 6 + 1;
            triangles[j++] = i + 1;
        }
        for (int i = 0; i < 6; i++)
        {
            triangles[j++] = 7;
            triangles[j++] = (i + 1) % 6 + 8;
            triangles[j++] = i + 8;
        }
        for (int i = 0; i < 6; i++)
        {
            int topA = i + 1;
            int topB = (i + 1) % 6 + 1;
            int botA = i + 8;
            int botB = (i + 1) % 6 + 8;
            triangles[j++] = topA;
            triangles[j++] = botB;
            triangles[j++] = botA;
            triangles[j++] = topA;
            triangles[j++] = topB;
            triangles[j++] = botB;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        var meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
        {
            meshCollider.sharedMesh = mesh;
        }
    }
}
