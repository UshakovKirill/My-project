using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingsManager : MonoBehaviour
{
    // Синглтон для легкого доступа к менеджеру
    public static BuildingsManager Instance { get; private set; }

    // Список всех зданий в игре
    public List<Building> ExistingBuildings { get; private set; } = new List<Building>();
public TextMeshProUGUI buildingsCountText;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

      void Start()
    {
        UpdateBuildingsCountText(); // Обновляем текст при запуске
    }
      private void UpdateBuildingsCountText()
    {
        if (buildingsCountText != null)
        {
            buildingsCountText.text = $"Buildings: {ExistingBuildings.Count}";
        }
    }

        public Vector3 GetAverageBuildingLocation()
    {
        if (ExistingBuildings.Count == 0)
            return Vector3.zero; // Возвращаем 0,0,0, если зданий нет

        Vector3 sum = Vector3.zero;
        foreach (var building in ExistingBuildings)
        {
            sum += building.transform.position;
        }
        return sum / ExistingBuildings.Count;
    }

    // Метод для добавления здания в список
   public void RegisterBuilding(Building building)
    {
        if (!ExistingBuildings.Contains(building))
        {
            ExistingBuildings.Add(building);
            UpdateBuildingsCountText(); // Обновляем текст при добавлении здания
        }
    }

    public void UnregisterBuilding(Building building)
    {
        if (ExistingBuildings.Contains(building))
        {
            ExistingBuildings.Remove(building);
            UpdateBuildingsCountText(); // Обновляем текст при удалении здания
        }
    }
}
