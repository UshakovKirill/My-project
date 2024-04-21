using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable] 
public class BuildingRequirement
{
    public ItemSO item; // Требуемый предмет
    public int quantity; // Требуемое количество
        // Ограничивающие теги указывают на теги объектов, на которых здание построить нельзя
   
}

public class Building : MonoBehaviour
{
    public bool isReady = false;
    public string buildingName; // Название здания
    public float constructionTime; // Время, необходимое для строительства
    public List<BuildingRequirement> requirements; // Список требований для строительства

     public List<string> restrictingTags;

    // Требуемые теги указывают на теги объектов, рядом с которыми должно быть построено здание
    public List<string> requiredTags;

    void Start(){

        BuildingsManager.Instance.RegisterBuilding(this);

        restrictingTags = new List<string>();
        requiredTags = new List<string>();

            // Для самого объекта, к которому прикреплён скрипт
        gameObject.tag = "Building";

       

    }
    
    // Вызывается, когда начинается строительство
 public void StartConstruction(Inventory inventory)
{
    if (CanConstruct(inventory))
    {
        Debug.Log($"Строительство '{buildingName}' началось.");
        
        // Изымаем необходимые ресурсы
        foreach (var requirement in requirements)
        {
            inventory.RemoveItems(requirement.item, requirement.quantity);
        }

        StartCoroutine(ConstructionTimer());
    }
    else
    {
        Debug.Log($"Строительство '{buildingName}' не может быть начато из-за недостатка материалов.");
    }
}

    private IEnumerator ConstructionTimer()
    {
        yield return new WaitForSeconds(constructionTime); // Ожидаем указанное время строительства

        isReady = true; // После завершения таймера здание готово
        Debug.Log($"Строительство '{buildingName}' завершено.");
        
        // Здесь можно вызвать другие методы, например, чтобы сообщить игроку или системе, что здание построено
        OnConstructionComplete();
    }

    // Проверка, достаточно ли материалов для строительства
    public bool CanConstruct(Inventory inventory)
    {
        foreach (var requirement in requirements)
        {
            ItemSO itemInInventory = inventory.FindItemByName(requirement.item.itemName);
            if (itemInInventory == null || inventory.GetItemCount(itemInInventory) < requirement.quantity)
            {
                Debug.Log($"Не хватает предмета: {requirement.item.itemName}");
                return false;
            }
        }
        return true;
    }

      private void OnConstructionComplete()
    {
        
    }
}
