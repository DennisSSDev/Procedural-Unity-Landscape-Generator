using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour {

    public enum DrawStates
    {
        NOISE,
        COLORS,
        Map3D
    };
    public DrawStates drawMode;
    const int mapChunkSize = 241;
    [Range(1,6)]
    public int levelOfDetail;
    public float scale;
    public float MeshYMul;
    public int octaves;
    public AnimationCurve curve;
    public float persistence;

    public float lacunarity;

    public bool AutoUpdate;
    public bool CompleteRandomness;

    public int seed;

    public Vector2 offset;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] heightsandValues = Noise.GenerateNoise(mapChunkSize, mapChunkSize, seed,scale,CompleteRandomness,octaves,offset,persistence,lacunarity);

        Color[] mapColor = new Color[mapChunkSize * mapChunkSize];
        


        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float curHeight = heightsandValues[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (curHeight<=regions[i].height)
                    {
                        
                        float save = regions[i].height - curHeight;
                        float r;
                        float g;
                        float b;

                        try
                        {
                             r = Mathf.Lerp(regions[i].Color.r, regions[i].Blended.r, save);
                             g = Mathf.Lerp(regions[i].Color.g, regions[i + 1].Blended.g, save);
                             b = Mathf.Lerp(regions[i].Color.b, regions[i + 1].Blended.b, save);
                        }
                        catch (System.Exception)
                        {
                            r = Mathf.Lerp(regions[i].Color.r, regions[i - 1].Color.r, save);
                            g = Mathf.Lerp(regions[i].Color.g, regions[i - 1].Color.g, save);
                            b = Mathf.Lerp(regions[i].Color.b, regions[i - 1].Color.b, save);

                        }                                                 
                        Color openColor = new Color(r, g, b);
                        mapColor[y * mapChunkSize + x] = openColor;
                        //mapColor[y * mapChunkSize + x] = regions[i].Color;
                        break;
                    }
                }
            }
        }
        MapDisplay mapDisp = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawStates.NOISE)
            mapDisp.Draw(heightsandValues);
        else if (drawMode == DrawStates.COLORS)
            mapDisp.Draw(MapDisplay.TextureFromColorMap(mapColor, mapChunkSize, mapChunkSize), heightsandValues);
        else if (drawMode == DrawStates.Map3D)
            mapDisp.DrawMesh(TerrainGen.GenerateTerMesh(heightsandValues,MeshYMul,curve, levelOfDetail), 
                MapDisplay.TextureFromColorMap(mapColor, mapChunkSize, mapChunkSize));
    }
    public void OnValidate()
    {
        
        if (scale <= 0)
            scale = 1;
        if (octaves <= 0)
            octaves = 1;
        if (octaves > 31)
            octaves = 31;
        if(persistence<0)
        {
            persistence = 0.1f;
        }
        if (persistence>1)
        {
            persistence = 1f;
        }
    }
    [System.Serializable]
    public struct TerrainType
    {

        public float height;
        public Color Color;
        public Color Blended;
        public string name;
    }
}
