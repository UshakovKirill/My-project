using UnityEngine;

public class TerrainColliderGenerator : MonoBehaviour
{
    public TerrainGenerator terrainGenerator; // Ссылка на ваш TerrainGenerator
    public float colliderHeight = 10f; // Высота коллайдеров
      public bool showColliders = false;

    void Start()
    {
        GenerateTerrainColliders();
    }
    
    void OnDrawGizmos()
{
    if (showColliders)
    {
        // Перебираем все дочерние объекты с коллайдерами
        foreach (Transform child in transform)
        {
            if (child.GetComponent<BoxCollider>() != null)
            {
                Gizmos.color = Color.green; // Установите желаемый цвет
                BoxCollider collider = child.GetComponent<BoxCollider>();
                Gizmos.DrawWireCube(child.position, collider.size);
            }
        }
    }
}

void Update() {
    if (Input.GetKeyDown(KeyCode.I)) {
        showColliders = !showColliders;
        ToggleCollidersVisibility(showColliders);
    }
}

void ToggleCollidersVisibility(bool visible) {
    foreach (Transform child in transform) {
        Renderer renderer = child.GetComponent<Renderer>();
        if (renderer != null) {
            renderer.enabled = visible;
        }
    }
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


 void GenerateTerrainColliders() {
    Debug.Log("Начало генерации коллайдеров");
    var terrainGrid = terrainGenerator.GetTerrainGrid();
    terrainGrid.MirrorGridVertically();
    terrainGrid.RotateGridLeft();
    terrainGrid.RotateGridLeft();
    terrainGrid.RotateGridLeft();
    if (terrainGrid == null) {
        Debug.LogError("TerrainGrid не инициализирован");
        return;
    }

    LogFirstTenGridValues(terrainGrid);

    var terrainData = terrainGenerator.terrain.terrainData;
    var size = terrainData.size;
    var width = terrainGrid.Width;
    var height = terrainGrid.Height;

    float cellSizeX = size.x / width;
    float cellSizeZ = size.z / height;

    // Удалите все предыдущие коллайдеры перед созданием новых
    foreach (Transform child in transform) {
        Destroy(child.gameObject);
    }

    for (int x = 0; x < width; x++) {
        for (int y = 0; y < height; y++) {
            TerrainType type = terrainGrid.GetTerrainType(x, y);
            Vector3 cellCenter = new Vector3(x * cellSizeX + cellSizeX / 2, 0, y * cellSizeZ + cellSizeZ / 2);
            cellCenter += terrainGenerator.terrain.transform.position;
            cellCenter.y = terrainData.GetHeight(x, y) + colliderHeight / 2; // Высота коллайдера

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = cellCenter;
            cube.transform.localScale = new Vector3(cellSizeX, colliderHeight, cellSizeZ); // Установите высоту коллайдера
            cube.transform.parent = this.transform;
            Destroy(cube.GetComponent<Collider>()); // Удалите стандартный коллайдер куба

            // Создаем новый BoxCollider с нужными размерами
            BoxCollider boxCollider = cube.AddComponent<BoxCollider>();
            boxCollider.size = new Vector3(1, 1, 1); // Коллайдер в локальных размерах, т.к. масштаб уже учтен
            boxCollider.isTrigger = true; // Если необходимо, чтобы коллайдеры были триггерами

            // Назначьте тег и цвет в зависимости от типа террейна
            if (type == TerrainType.Grass) {
                cube.tag = "GrassArea";
                cube.GetComponent<Renderer>().material.color = Color.green; // Для визуального представления
            } else if (type == TerrainType.Rock) {
                cube.tag = "RockArea";
                cube.GetComponent<Renderer>().material.color = Color.gray; // Для визуального представления
            }

            // Сделайте коллайдеры невидимыми по умолчанию
            cube.GetComponent<Renderer>().enabled = false;
        }
    }

    Debug.Log("Коллайдеры созданы на основе сетки");
}


}
