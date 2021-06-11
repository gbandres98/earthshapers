using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WorldGen : MonoBehaviour
{
    public static WorldGen Instance;
    public int seed = 0;
    public int worldWidth = 500;
    public int worldHeight = 100;
    public int reservedBottom = 40;
    public int reservedTop = 10;

    public float hillStretchFactor = 0.01f;
    public float caveStretchFactor = 0.05f;

    public float dirtStretchFactor = 0.1f;
    private float xHillSeed;
    private float yHillSeed;
    private float xCaveSeed;
    private float yCaveSeed;
    private Tilemap map;
    private List<GameObject> blockArr;
    public const long primeBase = 989999;
    private void SetSeeds()
    {
        xHillSeed = seed * seed % primeBase;
        yHillSeed = xHillSeed * xHillSeed % primeBase;
        xCaveSeed = yHillSeed * yHillSeed % primeBase;
        yCaveSeed = xCaveSeed * xCaveSeed % primeBase;
    }
    public void Awake()
    {
        Instance = this;
        map = GetComponent<Tilemap>();
        SetSeeds();
        int[,] worldMatrix = new int[worldWidth, worldHeight];
        WorldFill(worldMatrix);
        GenerateCaves(worldMatrix);
        GenerateHills(worldMatrix);
        DrawMap(worldMatrix);
    }
    private void WorldFill(int[,] matrix)
    {
        for (int i = 0; i < worldHeight; i++)
        {
            for (int j = 0; j < worldWidth; j++)
            {
                float sample = Mathf.PerlinNoise(xCaveSeed + (i * dirtStretchFactor * 2f), yCaveSeed + (j * dirtStretchFactor * 0.7f));
                matrix[j, i] = sample > .01f * i / worldHeight * 100 ? 5 : 1;
            }
        }
        //System.Random r = new System.Random();
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
                if (sample > 0.6f)
                {
                    worldMatrix[j, i] = 0;
                }
            }
        }
    }
    public void DrawMap(int[,] worldMatrix)
    {
        blockArr = new List<GameObject>();
        for (int i = 0; i < worldWidth; i++)
        {
            for (int j = 0; j < worldHeight; j++)
            {
                int currentCell = worldMatrix[i, j];
                if (currentCell != 0)
                {
                    GameObject block = Resources.Load($"Blocks/{Game.Items[currentCell]}") as GameObject;
                    block.transform.position = map.GetCellCenterWorld(new Vector3Int(i, j, 0));
                    GameObject blockInstance = Instantiate(block);
                    blockInstance.transform.parent = transform;
                    blockArr.Add(blockInstance);
                }
            }
        }
        StaticBatchingUtility.Combine(blockArr.ToArray(), this.gameObject);
    }
    public void DrawSquareTest()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject block = Resources.Load($"Blocks/{Game.Items[1]}") as GameObject;
                block.transform.position = map.GetCellCenterWorld(new Vector3Int(i, j, 0));
                GameObject blockInstance = Instantiate(block);
                blockInstance.transform.parent = transform;
            }
        }
    }
}