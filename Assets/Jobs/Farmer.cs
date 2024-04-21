using UnityEngine;
using System.Linq;
using System.Collections.Generic;
public class Farmer : Work
{
    public Farm farm;
 private Human human;
    private Inventory inventory ;

    private void Start(){
        inventory =  GetComponent<Human>().Inventory;
    }

    private void Awake()
    {
        human = GetComponent<Human>(); // Get the Human component attached to the same GameObject
        inventory = human.Inventory; // Access the Inventory from the Human component
    }
     private void Update()
    {
        CheckAndFulfillFoodRequests();
        CheckAndFulfillItemRequests();
    }

    public void UnassignFarm()
{
    farm = null;
}
private void CheckAndFulfillItemRequests()
{
    var itemRequests = MainBuilding.Instance.GetRequestsOfType<ItemRequest>();

    foreach (var request in itemRequests)
    {
        // Проверяем, есть ли в инвентаре достаточное количество требуемого предмета
        if (inventory.GetItemCount(request.RequiredItem) >= request.Quantity)
        {
            // Убираем предметы из инвентаря фермера
            bool itemsRemoved = inventory.RemoveItems(request.RequiredItem, request.Quantity);

            // Если удаление прошло успешно, передаем предметы и закрываем запрос
            if (itemsRemoved)
            {
                // Предполагая, что у Requester есть метод для добавления предметов в инвентарь
                request.Requester.Inventory.AddItems(request.RequiredItem, request.Quantity);

                // Закрываем запрос, убрав его из списка запросов в MainBuilding
                MainBuilding.Instance.RemoveRequest(request);

                Debug.Log($"Fulfilled item request for {request.Quantity}x {request.RequiredItem.itemName} for {request.Requester.name}");
            }
        }
    }
}




private void CheckAndFulfillFoodRequests()
{
    // Get the inventory from this component
    Inventory farmerInventory = GetComponent<Human>().Inventory;

    // Retrieve all the food requests
    var foodRequests = MainBuilding.Instance.GetRequestsOfType<FoodRequest>();

    // Assuming you want to process only one request at a time
    if (foodRequests.Count > 0)
    {
        var request = foodRequests[0];
        
        // Get all FoodSO items from the inventory
        List<FoodSO> foodItems = farmerInventory.GetAllItemsOfType<FoodSO>();

        if (foodItems.Count >= 10)
        {
            // Transfer 5 units of food to the requester
            for (int i = 0; i < 5; i++)
            {
                // Transfer the first item (this assumes all food items are equivalent for the purpose of fulfilling requests)
                FoodSO foodItem = foodItems[i];

                // Remove the food item from the farmer's inventory
                farmerInventory.RemoveItem(foodItem);

                // Add the food item to the requester's inventory
                request.Requester.Inventory.AddItem(foodItem);
            }

            // Log the successful transfer
            Debug.Log($"Fulfilled food request for {request.Requester.name}");

            // Remove the request from the MainBuilding's list
            MainBuilding.Instance.RemoveRequest(request);
        }
    }
}


       public override void DoWork(int b)
    {
        // Check if the farmer has been assigned to a farm
        if (farm != null)
        {
            // Use the Human's method to go to the farm's location
            Human human = GetComponent<Human>();
            if (human != null)
            {
                human.GoToBuilding(farm);
            }
        }
        else
        {
            Debug.Log($"{gameObject.name} does not have a farm to work on.");
        }
    }
    public override void DoWork()
    {}

  

    public void AssignFarm(Farm farm)
    {
        this.farm = farm;
       
    }
    

   
}
