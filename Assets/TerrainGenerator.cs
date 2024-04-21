using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    public Terrain terrain;
    public Texture2D grassTexture;
    public Texture2D rockTexture;
    private TerrainGrid terrainGrid; public float noiseScale = 0.5f;

  void Start()
    {
        terrainGrid = new TerrainGrid(terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);
        FillTerrainGrid();
        SetupTerrainLayers();
        ApplyTexturesBasedOnGrid();
    }

void FillTerrainGrid() {
    float grassProbability = 0.1f; // Значение, которое контролирует соотношение травы к камням

    for (int y = 0; y < terrainGrid.Height; y++) {
        for (int x = 0; x < terrainGrid.Width; x++) {
            float noise = Mathf.PerlinNoise(x * noiseScale, y * noiseScale);
            TerrainType type = noise > grassProbability ? TerrainType.Grass : TerrainType.Rock;
            terrainGrid.SetTerrainType(x, y, type);
        }
    }
}


  

     public TerrainGrid GetTerrainGrid()
    {
        return terrainGrid;
    }
 void SetupTerrainLayers()
    {
        // Создаем слои текстур для травы и камней
        TerrainLayer grassLayer = new TerrainLayer();
        grassLayer.diffuseTexture = grassTexture;
        grassLayer.tileSize = new Vector2(15, 15); // Размер текстуры на Terrain

        TerrainLayer rockLayer = new TerrainLayer();
        rockLayer.diffuseTexture = rockTexture;
        rockLayer.tileSize = new Vector2(15, 15); // Размер текстуры на Terrain

        // Применяем эти слои к Terrain
        terrain.terrainData.terrainLayers = new TerrainLayer[] { grassLayer, rockLayer };
    }

    void LogFirstTenGridValues(TerrainGrid terrainGrid) {
    string gridValues = "Первые 10 значений сетки для коллайдеров: ";
    for (int i = 0; i < 100 && i < terrainGrid.Width * terrainGrid.Height; i++) {
        int x = i % terrainGrid.Width;
        int y = i / terrainGrid.Width;
        TerrainType type = terrainGrid.GetTerrainType(x, y);
        gridValues += $"{type} ";
    }
    Debug.Log(gridValues);
}

    
public void ApplyTexturesBasedOnGrid() {
    TerrainData terrainData = terrain.terrainData;

    if (terrainGrid == null) {
        Debug.LogError("TerrainGrid не инициализирована.");
        return;
    }
    LogFirstTenGridValues(terrainGrid);

    int alphaMapWidth = terrainData.alphamapWidth;
    int alphaMapHeight = terrainData.alphamapHeight;
    float[,,] alphaMap = new float[alphaMapWidth, alphaMapHeight, 2]; // Изменено на 2, если у вас только два слоя текстур

    float ratioX = (float)terrainGrid.Width / alphaMapWidth;
    float ratioY = (float)terrainGrid.Height / alphaMapHeight;

    for (int y = 0; y < alphaMapHeight; y++) {
        for (int x = 0; x < alphaMapWidth; x++) {
            int gridX = Mathf.FloorToInt(x * ratioX);
            int gridY = Mathf.FloorToInt(y * ratioY);

            TerrainType terrainType = terrainGrid.GetTerrainType(gridX, gridY);
            float grassValue = terrainType == TerrainType.Grass ? 1 : 0;
            float rockValue = terrainType == TerrainType.Rock ? 1 : 0;

            alphaMap[x, y, 0] = grassValue; // Трава для первого слоя
            alphaMap[x, y, 1] = rockValue; // Камень для второго слоя
        }
    }

    terrainData.SetAlphamaps(0, 0, alphaMap);
}




}

