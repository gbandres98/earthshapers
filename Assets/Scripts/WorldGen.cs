using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public float xSeed;
    public float ySeed;

    public int worldWidth = 500;
    public int worldHeight = 100;
    public int reservedBottom = 20;
    public int reservedTop = 10;

    public float stretchFactor = 0.01f;

    private void Start()
    {
        int[,] worldMatrix = GenerateWorld();
        string[] datos = new string[worldHeight];

        for (int i = 0; i < worldHeight; i++)
        {
            String row = "";
            for (int j = 0; j < worldWidth; j++)
            {
                row += worldMatrix[j, i];
            }
            datos[worldHeight - i - 1] = row;
        }

        File.WriteAllLines("./map.txt", datos, Encoding.UTF8);
    }

    private int[,] GenerateWorld()
    {
        int effectiveMaxHeight = worldHeight - reservedTop - reservedBottom;

        List<int> heightMap = new List<int>();
        int[,] worldMatrix;
        for (int i = 0; i < worldWidth; i++)
        {
            float sample = Mathf.PerlinNoise(xSeed + (i * stretchFactor), ySeed);

            int scaledSample = (int)Math.Round((sample * effectiveMaxHeight) + reservedBottom);

            heightMap.Add(scaledSample);
        }

        worldMatrix = new int[worldWidth, worldHeight];

        for (int i = 0; i < worldWidth; i++)
        {
            for (int j = 0; j < worldHeight; j++)
            {
                worldMatrix[i, j] = j < heightMap[i] ? 1 : 0;
            }
        }
        return worldMatrix;
    }
}