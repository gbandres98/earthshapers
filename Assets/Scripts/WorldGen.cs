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

    private int[,] worldMatrix;
    private void Start()
    {
        GenerateWorld();
        string[] datos = new string[worldHeight];

        for (int i = 0; i < worldHeight; i++)
        {
            String row = "";
            for (int j = 0; j < worldWidth; j++)
            {
                row += worldMatrix[j, i];
                //aqui hay que unir cada fila en un solo string
            }
            datos[worldHeight - i - 1] = row;
        }

        File.WriteAllLines("./map.txt", datos, Encoding.UTF8);
    }

    private void GenerateWorld()
    {
        int effectiveMaxHeight = worldHeight - reservedTop - reservedBottom;
        /*
     guarda cada posicion en el eje x, emparejada con la altura obtenida con el perlinNoise
     ahora veo que una lista es suficiente, no hace falta un diccionario.
      */
        List<int> heightMap = new List<int>();

        for (int i = 0; i < worldWidth; i++)
        {           /*este sample esta normalizado, va de 0 a 1*/
            float sample = Mathf.PerlinNoise(xSeed + (i * stretchFactor), ySeed);
            /*Lo escalo a la altura permitida para la dirt*/
            int scaledSample = (int)Math.Round((sample * effectiveMaxHeight) + reservedBottom);
            /* se añade al mapa de alturas*/
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
    }
}