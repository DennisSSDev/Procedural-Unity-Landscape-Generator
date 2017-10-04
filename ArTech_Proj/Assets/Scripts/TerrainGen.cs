using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainGen {
    public static MeshData GenerateTerMesh(float[,] heightmap, float heightMul, AnimationCurve curve,int levelOfDetail)
    {

        int width = heightmap.GetLength(0);
        int height = heightmap.GetLength(1);
        float topLX = (width - 1) / -2f;
        float topLZ = (height - 1) / 2f;
        int simplification = levelOfDetail * 2;

        int vertPerLine = (width - 1) / simplification + 1;
        MeshData meshData = new MeshData(vertPerLine, vertPerLine);

        int vertexIndex = 0;

        for (int y = 0; y < height; y+=simplification)
        {
            for (int x = 0; x < width; x+=simplification)
            {
                meshData.verticies[vertexIndex] = new Vector3(topLX + x, curve.Evaluate(heightmap[x, y])*heightMul,topLZ - y);
                meshData.uv[vertexIndex] = new Vector2(x / (float)width, y / (float)height);
                if (x<width-1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + vertPerLine + 1, vertexIndex + vertPerLine);
                    meshData.AddTriangle(vertexIndex + vertPerLine + 1, vertexIndex , vertexIndex + 1);

                }
                vertexIndex++;

            }
        }

        return meshData;
    }

}
public class MeshData
{
    public Vector3[] verticies;
    public int[] triangles;
    public Vector2[] uv;
    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        verticies = new Vector3[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
        uv = new Vector2[meshWidth*meshHeight]; 
    }
    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex+1] = b;
        triangles[triangleIndex+2] = c;
        triangleIndex += 3;
    }

    public Vector3[] CalculateNormals()
    {
        Vector3[] vertNormals = new Vector3[verticies.Length];
        int triangleCount = triangles.Length/3;
        for (int i = 0; i < triangleCount; i++)
        {
            int normalTriangleIndex = i * 3;
            int vertIndexA = triangles[normalTriangleIndex];
            int vertIndexB = triangles[normalTriangleIndex+1];
            int vertIndexC = triangles[normalTriangleIndex+2];

            Vector3 triangleNormal = SurfaceNormalFromIndacies(vertIndexA, vertIndexB, vertIndexC);
            vertNormals[vertIndexA] += triangleNormal;
            vertNormals[vertIndexB] += triangleNormal;
            vertNormals[vertIndexC] += triangleNormal;
        }
        for (int i = 0; i < vertNormals.Length; i++)
        {
            vertNormals[i].Normalize();
        }

        return vertNormals;
    }
    
    Vector3 SurfaceNormalFromIndacies(int a, int b, int c)
    {
        Vector3 pointA = verticies[a];
        Vector3 pointB = verticies[b];
        Vector3 pointC = verticies[c];

        Vector3 sideAB = pointB - pointA;
        Vector3 sideAC = pointC - pointA;
        return Vector3.Cross(sideAB, sideAC).normalized;
    }

    public void Flatshading()
    {
        Vector3[] flatShadedVerts = new Vector3[triangles.Length];

        Vector2[] flatUV = new Vector2[uv.Length];

        

        for (int i = 0; i < flatShadedVerts.Length; i++)
        {
            flatShadedVerts[i] = verticies[triangles[i]];
            flatUV[i] = uv[triangles[i]];
            triangles[i] = i;
        }
        verticies = flatShadedVerts;
        uv = flatUV;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = verticies;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
        mesh.normals = CalculateNormals();
        return mesh;
    }


}
