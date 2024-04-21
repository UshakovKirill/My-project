using UnityEngine;
using System.Collections.Generic;

public class ItemsDatabase : MonoBehaviour
{
    public static ItemsDatabase Instance { get; private set; }

    public List<ItemSO> items = new List<ItemSO>(); // Список всех предметов
    public List<FoodSO> foods = new List<FoodSO>(); // Список всех видов еды

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public ItemSO GetCarrotMarker()
    {
        return foods.Find(item => item.itemName == "Carrot");
    }
    public ItemSO GetWoodMarker()
    {
        return foods.Find(item => item.itemName == "Wood");
    }
}
