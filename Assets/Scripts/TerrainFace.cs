using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainFace {
    Mesh mesh;
    int resolution;
    Vector3 localUp;
    Vector3 xAxis;
    Vector3 yAxis;

    public TerrainFace(Mesh mesh, int resolution, Vector3 localUp) 
    {
        this.mesh = mesh;
        this.resolution = resolution;
        this.localUp = localUp;

        xAxis = new Vector3(localUp.y, localUp.z, localUp.x);
        yAxis = Vector3.Cross(localUp, xAxis);
    }

    public void ConstructMesh()
    {
        // Resolution is on one side of the mesh.
        Vector3[] meshVertices = new Vector3[resolution * resolution];

        // Number of squares in mesh = (resolution - 1) * (resolution - 1)
        // Number of triangles per square = 2
        // Number of indices per triangle = 3
        int[] triangleIndices = new int[(resolution - 1) * (resolution - 1) * 6];

        int triangleIndex = 0;

        for (int y = 0; y < resolution; ++y)
        {
            for (int x = 0; x < resolution; ++x)
            {
                int vertexIndex = x + (resolution * y);
                 
                // Gives the percent position of the vertex along the edge.
                Vector2 percent = new Vector2(x, y) / (resolution - 1);

                Vector3 pointOnUnitMesh = localUp + (percent.x - 0.5f) * 2 * xAxis + (percent.y - 0.5f) * 2 * yAxis;
                Vector3 pointOnUnitSphere = pointOnUnitMesh.normalized;
                meshVertices[vertexIndex] = pointOnUnitSphere;

                // 0        1
                // ----------
                // |\       |
                // | \      |
                // |  \     |
                // |   \    |
                // |    \   |
                // |     \  |
                // |      \ |
                // |       \|
                // ----------
                // 2        3

                if (x != resolution - 1 && y != resolution - 1)
                {
                    // First triangle. (0, 3, 2)
                    triangleIndices[triangleIndex] = vertexIndex;
                    triangleIndices[triangleIndex + 1] = vertexIndex + resolution + 1;
                    triangleIndices[triangleIndex + 2] = vertexIndex + resolution;

                    // Second triangle. (0, 1, 3)
                    triangleIndices[triangleIndex + 3] = vertexIndex;
                    triangleIndices[triangleIndex + 4] = vertexIndex + 1;
                    triangleIndices[triangleIndex + 5] = vertexIndex + resolution + 1;

                    triangleIndex += 6;
                }
            }
        }


        // Assign mesh.
        mesh.Clear();
        mesh.vertices = meshVertices;
        mesh.triangles = triangleIndices;
        mesh.RecalculateNormals();
    }
}
