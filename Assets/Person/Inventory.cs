using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Inventory
{
    private List<ItemSO> items = new List<ItemSO>();

    public void AddItem(ItemSO item)
    {
        items.Add(item);
    }

    public bool AddItems(ItemSO item, int quantity)
{
    if (item == null || quantity <= 0)
    {
        Debug.LogError("Некорректные данные для добавления предметов.");
        return false;
    }

    for (int i = 0; i < quantity; i++)
    {
        items.Add(item);
    }

    return true;
}

    public void AddItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < quantity; i++)
        {
            items.Add(item);
        }
    }
    public List<T> GetAllItemsOfType<T>() where T : ItemSO
{
    return items.OfType<T>().ToList();
}

public bool RemoveItems(ItemSO item, int quantity)
{
    if (GetItemCount(item) < quantity)
    {
        Debug.Log($"Недостаточно {item.itemName} для изъятия.");
        return false;
    }

    for (int i = 0; i < quantity; i++)
    {
        items.Remove(item);
    }

    return true;
}

    public bool RemoveItem(ItemSO item)
    {
        return items.Remove(item);
    }

    public ItemSO FindItemByName(string name)
    {
        return items.Find(item => item.itemName == name);
    }

    public List<ItemSO> GetItems()
    {
        return items;
    }

        public void TransferItemTo(ItemSO item, Inventory targetInventory, int quantity)
    {
        int transferred = 0;
        for (int i = items.Count - 1; i >= 0 && transferred < quantity; i--)
        {
            if (items[i].itemName == item.itemName)
            {
                targetInventory.AddItem(items[i]);
                items.RemoveAt(i);
                transferred++;
            }
        }
    }

    // Метод для проверки наличия минимального количества еды в инвентаре
    public bool HasEnoughFood(string foodName, int quantity)
    {
        return GetItemCount(FindItemByName(foodName)) >= quantity;
    }

    public override string ToString()
    {
        Dictionary<string, int> itemCounts = new Dictionary<string, int>();
        
        foreach (var item in items)
        {
            if (itemCounts.ContainsKey(item.itemName))
            {
                itemCounts[item.itemName]++;
            }
            else
            {
                itemCounts[item.itemName] = 1;
            }
        }

        List<string> itemStrings = new List<string>();
        foreach (var pair in itemCounts)
        {
            itemStrings.Add($"{pair.Key}:{pair.Value}");
        }

        return string.Join(", ", itemStrings);
    }

       public int GetItemCount(ItemSO item)
    {
        return items.Count(x => x == item);
    }
     public bool CanConstruct(Building building)
    {
        foreach (var requirement in building.requirements)
        {
            // Подсчитываем, сколько у нас есть предметов каждого типа
            int count = items.Count(item => item.itemName == requirement.item.itemName);

            // Если для какого-то из требований недостаточно предметов, возвращаем false
            if (count < requirement.quantity)
            {
                return false;
            }
        }
        // Если все требования удовлетворены, возвращаем true
        return true;
    }

    public bool RemoveItem(ItemSO item, int quantity)
{
    bool removed = false;
    for (int i = 0; i < quantity && items.Contains(item); i++)
    {
        removed = items.Remove(item);
    }
    return removed;
}
}
