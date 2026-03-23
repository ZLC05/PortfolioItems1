using UnityEngine;
using System.Collections;

public static class MeshGenerator
{

    public static MeshData GenerateTerrainMesh(float[,] heightMap, float heightMutiplier, AnimationCurve heightCurve)
    {
        
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / -2f;
        float topLeftZ = (height - 1) / 2f;

        MeshData meshData = new MeshData(width, height);
        int vertexIndex = 0;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {

                meshData.vertices[vertexIndex] = new Vector3(topLeftX + x, heightCurve.Evaluate(heightMap[x,y]) * heightMutiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1)
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + width + 1, vertexIndex + width);
                    meshData.AddTriangle(vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
                    if (Random.Range(1, 100) == 1)
                    {
                        Vector3[] allVerticies = meshData.vertices;
                        objectGenerator item = GameObject.Find("Objects").GetComponent<objectGenerator>();
                        int randGen = Random.Range(0, 3);
                        if (randGen == 1)
                        {

                            Debug.Log("add a rock at " + allVerticies[vertexIndex]);
                            item.spawnPos = allVerticies[vertexIndex];
                            item.spawnPos = new Vector3(allVerticies[vertexIndex].x * GameObject.Find("Mesh").transform.localScale.x, allVerticies[vertexIndex].y * GameObject.Find("Mesh").transform.localScale.y, allVerticies[vertexIndex].z * GameObject.Find("Mesh").transform.localScale.z);
                            item.GenerateRock();
                        }
                        else if (randGen == 2)
                        {
                            Debug.Log("add a tree at " + allVerticies[vertexIndex]);
                            item.spawnPos = allVerticies[vertexIndex];
                            item.spawnPos = new Vector3(allVerticies[vertexIndex].x * GameObject.Find("Mesh").transform.localScale.x, allVerticies[vertexIndex].y * GameObject.Find("Mesh").transform.localScale.y, allVerticies[vertexIndex].z * GameObject.Find("Mesh").transform.localScale.z);

                            item.GenerateTree();
                        }
                        else
                        {
                            Debug.Log("add a bush at " + allVerticies[vertexIndex]);
                            item.spawnPos = allVerticies[vertexIndex];
                            item.spawnPos = new Vector3(allVerticies[vertexIndex].x * GameObject.Find("Mesh").transform.localScale.x, allVerticies[vertexIndex].y * GameObject.Find("Mesh").transform.localScale.y, allVerticies[vertexIndex].z * GameObject.Find("Mesh").transform.localScale.z);

                            item.GenerateBush();
                        }
                    }
                }

                vertexIndex++;
            }
        }

        return meshData;

    }
}

public class MeshData
{
    public Vector3[] vertices;
    public int[] triangles;
    public Vector2[] uvs;

    int triangleIndex;

    public MeshData(int meshWidth, int meshHeight)
    {
        vertices = new Vector3[meshWidth * meshHeight];
        uvs = new Vector2[meshWidth * meshHeight];
        triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        return mesh;
    }

}

