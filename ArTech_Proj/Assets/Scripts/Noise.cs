using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise{
    // Use this for initialization
    public static float[,] GenerateNoise(int width, int height,int seed, float scale, bool random, int octaves, Vector2 offset, 
        float persistence, float lacunarity)
    {
        System.Random rndm = new System.Random(seed);
        Vector2[] octavesOffsets = new Vector2[octaves];
        for (int i = 0; i < octaves; i++)
        {
            float resultX = rndm.Next(-100000, 100000) + offset.x;
            float resultY = rndm.Next(-100000, 100000) + offset.y;
            octavesOffsets[i] = new Vector2(resultX, resultY);
        }
        float[,] heights = new float[width, height];
        float ranX = Random.Range(0f, 99f);
        float ranY = Random.Range(0f, 99f);

        float halfWidth = width / 2f;
        float halfHeight = height / 2f;
        float maxNoise = float.MinValue;
        float minNoise = float.MaxValue;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float amplitude = 1f;
                float frequency = 1f;
                float noiseHeight = 0f;
                for (int i = 0; i < octaves; i++)
                {
                    float x_Comp = (x-halfWidth) / scale * frequency + octavesOffsets[i].x;
                    float y_Comp = (y-halfHeight) / scale * frequency + octavesOffsets[i].y;
                    
                    float perlVal = Mathf.PerlinNoise(x_Comp, y_Comp)*2 -1;
                    noiseHeight += perlVal * amplitude;
                    amplitude *= persistence;
                    frequency *= lacunarity;
                    /*if (random)
                        heights[x, y] = Mathf.PerlinNoise(x_Comp * ranX, y_Comp * ranY);
                    else
                        heights[x, y] = Mathf.PerlinNoise(x_Comp, y_Comp);
                        */
                }
                if (noiseHeight > maxNoise)
                    maxNoise = noiseHeight;
                else if (noiseHeight < minNoise)
                    minNoise = noiseHeight;
                heights[x, y] = noiseHeight;
            }
        }
        for (int x = 0; x< width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                heights[x, y] = Mathf.InverseLerp(minNoise, maxNoise, heights[x, y]);
            }
        }
        return heights;
    }
    /*
    public  static Color[] GenerateColorBiom(Color[] existing_colors , int size, float[,] heights, int regions)
    {


        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                float curHeight = heights[x, y];
                for (int i = 0; i < regions; i++)
                {
                    
                }
            }
        }

        return existing_colors;
        
    }*/
	
	// Update is called once per frame
}
