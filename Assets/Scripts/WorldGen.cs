using System.IO;
using System.Text;
using System;
using UnityEngine;

public class WorldGen : MonoBehaviour
{
    public int seed = 0;
    public int worldWidth = 500;
    public int worldHeight = 100;
    public int reservedBottom = 40;
    public int reservedTop = 10;

    public float hillStretchFactor = 0.01f;
    public float caveStretchFactor = 0.05f;
    private float xHillSeed;
    private float yHillSeed;
    private float xCaveSeed;
    private float yCaveSeed;
    public const long primeBase = 989999;
    private void SetSeeds()
    {
        xHillSeed = seed * seed % primeBase;
        yHillSeed = xHillSeed * xHillSeed % primeBase;
        xCaveSeed = yHillSeed * yHillSeed % primeBase;
        yCaveSeed = xCaveSeed * xCaveSeed % primeBase;
    }

    private void Start()
    {
        SetSeeds();
        int[,] worldMatrix = new int[worldWidth, worldHeight];
        Fill(worldMatrix, 1);
        GenerateCaves(worldMatrix);
        WriteFile("./map1.txt", worldMatrix);
        GenerateHills(worldMatrix);
        WriteFile("./map2.txt", worldMatrix);
    }

    private void Fill(int[,] matrix, int value)
    {
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[1, j] = value;
            }
        }
    }
    private void WriteFile(string path, int[,] data)
    {
        string[] datos = new string[worldHeight];

        for (int i = 0; i < worldHeight; i++)
        {
            String row = "";
            for (int j = 0; j < worldWidth; j++)
            {
                row += data[j, i];
            }
            datos[worldHeight - i - 1] = row;
        }

        File.WriteAllLines(path, datos, Encoding.UTF8);
    }

    private void GenerateHills(int[,] worldMatrix)
    {
        int effectiveMaxHeight = worldHeight - reservedTop - reservedBottom;
        for (int i = 0; i < worldWidth; i++)
        {
            double sample = Mathf.PerlinNoise(xHillSeed + (i * hillStretchFactor), yHillSeed);

            int scaledSample = (int)Math.Round((sample * effectiveMaxHeight) + reservedBottom);
            for (int j = scaledSample; j < worldHeight; j++)
            {
                worldMatrix[i, j] = 0;
            }
        }
    }
    private int FindFirstFullRow(int[,] map)
    {
        int firstNoSpaceRow = reservedBottom;
        for (int i = reservedBottom; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                if (map[j, i] == 0)
                {
                    return i - 1;
                }
            }
        }
        return firstNoSpaceRow;
    }
    private void GenerateCaves(int[,] worldMatrix)
    {
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                float sample = Mathf.PerlinNoise(xCaveSeed + (i * caveStretchFactor * 2f), yCaveSeed + (j * caveStretchFactor * 0.7f));
                worldMatrix[j, i] = sample > 0.6f ? 0 : 1;
            }
        }
    }
}