using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Farm : Building
{
    public Human boundFarmer;
      public ItemSO foodPrefab; // Assign this in the Inspector
    public Farmer assignedFarmer;
    public float productionRadius = 10f;
    public float productionTime;
    public string animation;

private void OnDestroy()
{
    // Удаляем здание из менеджера зданий
    BuildingsManager.Instance.UnregisterBuilding(this);

    // Если здание уничтожено, убираем связанную ферму у фермера
    if (boundFarmer != null && boundFarmer.work is Farmer)
    {
        ((Farmer)boundFarmer.work).UnassignFarm();
    }
}
 private IEnumerator CheckFloodRiskRoutine()
    {
        while (true)
        {
            if (RainManager.IsRaining)
            {
                if (Random.Range(0, 5000) < 1) // 1% шанс затопления
                {
                    Debug.Log($"{this.buildingName} has been flooded and destroyed!");
                    DestroyFarm();
                    break; // Выходим из корутины после уничтожения фермы
                }
            }

            // Проверяем каждые 10 секунд
            yield return new WaitForSeconds(10f);
        }
    }

    private void DestroyFarm()
    {
        // Отписываем ферму от фермера, если он назначен
        if (boundFarmer != null && boundFarmer.work is Farmer farmer)
        {
            farmer.UnassignFarm();
        }

        // Удаляем ферму из списка зданий
        BuildingsManager.Instance.UnregisterBuilding(this);

        // Уничтожаем объект фермы
        Destroy(gameObject);
    }
    

    private void Start()
    {
        BuildingsManager.Instance.RegisterBuilding(this);
        // When the farm is created, attempt to find and assign a farmer
        FindAndAssignFarmer();
 
        StartCoroutine(ProduceFoodRoutine());
          gameObject.tag = "Building";
          StartCoroutine(CheckFloodRiskRoutine());
    
    }

    private void Update()
    {
        // Regularly check if the bound farmer is still available
        if (boundFarmer == null || boundFarmer.Equals(null))
        {
            FindAndAssignFarmer();
        }
    }

    private void FindAndAssignFarmer()
    {
        // Clear the current assignment
        boundFarmer = null;

        // Search for available farmers
        foreach (var human in PeopleManager.Instance.GetAllHumans())
        {
            if (human.work is Farmer && ((Farmer)human.work).farm == null)
            {
                // Assign the first available farmer
                boundFarmer = human;
                ((Farmer)human.work).AssignFarm(this);
                Debug.Log($"{human.name} has been bound to {this.buildingName} as its farmer.");
                return; // Stop searching after assigning one farmer
            }
        }
    }

    private IEnumerator ProduceFoodRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionTime); // Wait for 10 seconds

            // Check if the assigned farmer is within the production radius
            if (boundFarmer != null && IsFarmerCloseEnough())
            {
                //Debug.Log("Produce!");
                // Produce food and add it to the farmer's inventory
                ProduceFood();
            }
        }
    }

    private bool IsFarmerCloseEnough()
    {
        if (boundFarmer == null) return false;

        // Get the Human component from the assigned farmer to access its position
        Human farmerHuman = boundFarmer.GetComponent<Human>();
        if (farmerHuman == null) return false;

        float distance = Vector3.Distance(transform.position, farmerHuman.transform.position);
        return distance <= productionRadius;
    }

private void ProduceFood()
{
    if (boundFarmer == null) return;

    // Предполагаем, что у фермера есть компонент UnitAnimator для управления анимациями
    UnitAnimator farmerAnimator = boundFarmer.GetComponent<UnitAnimator>();
    Human farmerHuman = boundFarmer.GetComponent<Human>();
    // Проверяем наличие инвентаря и prefab'а еды
    if (farmerHuman != null && farmerHuman.Inventory != null && foodPrefab != null)
    {
        // Добавляем prefab еды в инвентарь фермера
        farmerHuman.Inventory.AddItem(foodPrefab);
       // Debug.Log("Food produced and added to the farmer's inventory.");

        // Активируем анимацию получения ресурсов, если компонент UnitAnimator существует
        if (farmerAnimator != null && !string.IsNullOrEmpty(animation))
        {
           // farmerAnimator.PlayAnimation(animation);
           // Debug.Log($"Animation '{animation}' is playing.");
        }
        else
        {
            Debug.LogError("Failed to play animation: UnitAnimator component is missing or animation name is empty.");
        }
    }
}

}
