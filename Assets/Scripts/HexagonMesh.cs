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
        vertices[0] = new Vector3(0, height / 2, 0);      // Top center
        vertices[7] = new Vector3(0, -height / 2, 0);     // Bottom center
        for (int i = 0; i < 6; i++)
        {
            float angle = Mathf.PI / 3 * i;
            float x = radius * Mathf.Cos(angle);
            float z = radius * Mathf.Sin(angle);
            vertices[i + 1] = new Vector3(x, height / 2, z);
            vertices[i + 8] = new Vector3(x, -height / 2, z);
        }

        // Top face triangles (submesh 0)
        int[] topTriangles = new int[6 * 3];
        int j = 0;
        for (int i = 0; i < 6; i++)
        {
            topTriangles[j++] = 0;
            topTriangles[j++] = (i + 1) % 6 + 1;
            topTriangles[j++] = i + 1;
        }

        // Bottom face triangles (submesh 1)
        int[] bottomTriangles = new int[6 * 3];
        j = 0;
        for (int i = 0; i < 6; i++)
        {
            bottomTriangles[j++] = 7;
            bottomTriangles[j++] = i + 8;
            bottomTriangles[j++] = (i + 1) % 6 + 8;
        }

        // Side face triangles (submesh 2)
        int[] sideTriangles = new int[6 * 6];
        j = 0;
        for (int i = 0; i < 6; i++)
        {
            int topA = i + 1;
            int topB = (i + 1) % 6 + 1;
            int botA = i + 8;
            int botB = (i + 1) % 6 + 8;
            sideTriangles[j++] = topA;
            sideTriangles[j++] = botB;
            sideTriangles[j++] = botA;
            sideTriangles[j++] = topA;
            sideTriangles[j++] = topB;
            sideTriangles[j++] = botB;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.subMeshCount = 3;
        mesh.SetTriangles(topTriangles, 0);
        mesh.SetTriangles(bottomTriangles, 1);
        mesh.SetTriangles(sideTriangles, 2);

        // Simple planar UVs (optional, for demonstration)
        Vector2[] uvs = new Vector2[14];
        for (int i = 0; i < 14; i++)
            uvs[i] = new Vector2(vertices[i].x / (2 * radius) + 0.5f, vertices[i].z / (2 * radius) + 0.5f);
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        GetComponent<MeshFilter>().sharedMesh = mesh;

        var meshCollider = GetComponent<MeshCollider>();
        if (meshCollider != null)
            meshCollider.sharedMesh = mesh;
    }
}
