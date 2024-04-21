using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainType
{
    Grass,
    Rock
}

public class TerrainGrid
{
    private TerrainType[,] grid;
    public int Width { get; private set; }
    public int Height { get; private set; }

       public void MirrorGridVertically()
    {
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height / 2; y++)
            {
                TerrainType temp = grid[x, y];
                grid[x, y] = grid[x, Height - y - 1];
                grid[x, Height - y - 1] = temp;
            }
        }
    }
    public void RotateGridLeft()
    {
        // Создаем временную сетку для хранения новых значений
        TerrainType[,] newGrid = new TerrainType[Width, Height];

        // Заполняем новую сетку отраженными значениями
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                // Поворот на 90 градусов влево равносилен отражению по вертикали и последующему транспонированию
                newGrid[y, x] = grid[Width - x - 1, y];
            }
        }

        // Копируем новую сетку обратно в оригинальную
        for (int x = 0; x < Width; x++)
        {
            for (int y = 0; y < Height; y++)
            {
                grid[x, y] = newGrid[x, y];
            }
        }
    }
    

    public TerrainGrid(int width, int height)
    {
        Width = width;
        Height = height;
        grid = new TerrainType[width, height];
    }

    public void SetTerrainType(int x, int y, TerrainType type)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            grid[x, y] = type;
        }
    }

    public TerrainType GetTerrainType(int x, int y)
    {
        if (x >= 0 && x < Width && y >= 0 && y < Height)
        {
            return grid[x, y];
        }
        return TerrainType.Grass; // Default value
    }
}

