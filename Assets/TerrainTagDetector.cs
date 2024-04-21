using UnityEngine;

public class TerrainTagDetector : MonoBehaviour
{
    public TerrainColliderGenerator terrainColliderGenerator;
    void Update()
    {
        // Проверяем, была ли нажата правая кнопка мыши
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Проверяем, попал ли луч, исходящий из камеры по направлению к курсору мыши, в коллайдер на сцене
            if (Physics.Raycast(ray, out hit))
            {
                // Выводим тег объекта, в который попал луч
                Debug.Log("Тег территории: " + hit.collider.gameObject.tag);
            }
        }
        // Проверяем, была ли нажата клавиша I
    
    }
}
