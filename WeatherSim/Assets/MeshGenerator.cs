using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uvs;
    Color[] colors;
    public Gradient gradient;
    float minTerrainHeight;
    float maxTerrainHeight;

    public int xSize = 20;
    public int zSize = 20;
    public float detail1 = 0.3f;
    public float altitude1 = 2f;
    public float detail2 = 0.3f;
    public float altitude2 = 2f;
    public float detail3 = 0.3f;
    public float altitude3 = 2f;
    public float textureBlendHeight = 0.5f; // Height where the texture switch happens (0 - 1)
    float[] heights; // Array to store normalized height values for each vertex


    public bool regenerateTerrain = true;
    // public Texture2D t1;
    // public Texture2D t2;
    // public float t1Tiling = 1f; // Tiling factor for texture 1
    // public float t2Tiling = 1f; // Tiling factor for texture 2

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        // CreateShape();
        // UpdateMesh();

    }

    void Update()
    {
        if(regenerateTerrain == true) {
            CreateShape();
            UpdateMesh();
        }
    }

    void CreateShape() {
        vertices = new Vector3[(xSize + 1) * (zSize + 1)];
        heights = new float[vertices.Length]; // Initialize heights array


        int i = 0;
        for(int z = 0; z <= zSize; z++) {
           
            for(int x = 0; x <= xSize; x++) {
                float y = Mathf.PerlinNoise(x * detail1, z * detail1) *  altitude1
                +  Mathf.PerlinNoise(x * detail2, z * detail2) *  altitude2 
                +  Mathf.PerlinNoise(x * detail3, z * detail3) *  altitude3;
                vertices[i] = new Vector3(x, y, z);

                if(y > maxTerrainHeight) {
                    maxTerrainHeight = y;
                }
                if(y < minTerrainHeight) {
                    minTerrainHeight = y;
                }
                heights[i] = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, y); // Normalize height

                i++;
            }
        }

        int vert = 0;
        int tris = 0;
        triangles = new int[xSize * zSize * 6];
        for(int z = 0; z < zSize; z++) {
            for(int x = 0; x < xSize; x++) {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;
                vert++;
                tris += 6;
            }
            vert++; // ca sa nu faca un triunghi in plus intre randuri
        } 

        uvs = new Vector2[vertices.Length];   
        i = 0;
        for(int z = 0; z <= zSize; z++) {
           
            for(int x = 0; x <= xSize; x++) {
               float h = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);

               uvs[i] = new Vector2((float)x / xSize, (float)z / zSize);
               
               // Apply tiling to UVs based on the height of the terrain
                // if (h < textureBlendHeight)
                // {
                //     // Tiling for texture 1
                //     uvs[i] *= t1Tiling;
                // }
                // else
                // {
                //     // Tiling for texture 2
                //     uvs[i] *= t2Tiling;
                // }

                i++;
            }
        }    

         colors = new Color[vertices.Length];   
         
        i = 0;
        for(int z = 0; z <= zSize; z++) {
           
            for(int x = 0; x <= xSize; x++) {
                float h = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);
                colors[i] = gradient.Evaluate(h);
                i++;
            }
        }    

        // colors = new Color[vertices.Length];
        // i = 0;
        // for (int z = 0; z <= zSize; z++)
        // {
        //     for (int x = 0; x <= xSize; x++)
        //     {
        //         float heightNormalized = Mathf.InverseLerp(minTerrainHeight, maxTerrainHeight, vertices[i].y);

        //         // Blend between t1 and t2 based on the height
        //         if (heightNormalized < textureBlendHeight)
        //         {
        //             colors[i] = t1.GetPixelBilinear(uvs[i].x, uvs[i].y);
        //         }
        //         else
        //         {
        //             colors[i] = t2.GetPixelBilinear(uvs[i].x, uvs[i].y);
        //         }

        //         i++;
        //     }
        // }
    }

    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.colors = colors;
        //GetComponent<MeshRenderer>().material.SetFloatArray("_Heights", heights);

        mesh.RecalculateNormals();
    }

}
