using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour {
    public Renderer texture;
    public MeshFilter filter;
    public MeshRenderer meshRen;
    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D colorA = new Texture2D(width, height);
        colorA.filterMode = FilterMode.Point;
        colorA.wrapMode = TextureWrapMode.Clamp;
        colorA.SetPixels(colorMap);
        colorA.Apply();
        return colorA;
    }
    public void DrawMesh(MeshData data, Texture2D texture)
    {
        filter.sharedMesh = data.CreateMesh();
        meshRen.sharedMaterial.mainTexture = texture;
    }

    public void Draw(Texture2D tex, float[,] map)
    {
        texture.sharedMaterial.mainTexture = tex;
        texture.transform.localScale = new Vector3(map.GetLength(0), 1, map.GetLength(1));
    }

    public void Draw(float[,] map)
    {
        int width = map.GetLength(0);
        int height = map.GetLength(1);

        Texture2D pixels = new Texture2D(width, height);
        Color[] colors = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colors[y * width + x] = Color.Lerp(Color.black, Color.white, map[x, y]);
            }
        }
        pixels.SetPixels(colors);
        pixels.Apply();

        texture.sharedMaterial.mainTexture = pixels;
        texture.transform.localScale = new Vector3(width, 1, height);
    }
}
