using UnityEngine;

public class Triangle3D : MonoBehaviour
{
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    void Start()
    {
        // Crear un nuevo Mesh
        Mesh mesh = new Mesh();

        // Definir los vértices del prisma triangular (ajustados a 1x1x1)
        Vector3[] vertices = new Vector3[]
        {
            // Cara frontal (ajustado al rango -0.5 a 0.5)
            new Vector3(-0.5f, 0.5f, 0.5f),  // Vértice superior frontal
            new Vector3(-0.5f, -0.5f, 0.5f), // Vértice inferior izquierdo frontal
            new Vector3(0.5f, -0.5f, 0.5f),  // Vértice inferior derecho frontal
            
            // Cara trasera (ajustado al rango -0.5 a 0.5)
            new Vector3(-0.5f, 0.5f, -0.5f),  // Vértice superior trasero
            new Vector3(-0.5f, -0.5f, -0.5f), // Vértice inferior izquierdo trasero
            new Vector3(0.5f, -0.5f, -0.5f)   // Vértice inferior derecho trasero
        };

        // Definir las caras del prisma (triángulos)
        int[] triangles = new int[]
        {
            // Cara frontal
            0, 1, 2,

            // Cara trasera
            3, 5, 4,

            // Lado izquierdo
            0, 4, 1,
            0, 3, 4,

            // Lado derecho
            2, 5, 3,
            2, 3, 0,

            // Base
            1, 4, 5,
            1, 5, 2
        };

        // Asignar vértices y triángulos al Mesh
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        // Calcular normales para iluminación correcta
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        // Asignar el Mesh al MeshFilter
        meshFilter.mesh = mesh;

        // Agregar MeshCollider para colisión
        MeshCollider meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
        meshCollider.convex = true;
    }
}
