using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class Human : MonoBehaviour
{
     public float thresholdDistance = 5.0f;
    private NavMeshAgent navMeshAgent;
   
    private UnitAnimator unitAnimator;
    public int Age { get; private set; }
    public int Energy { get; private set; }
    public Inventory Inventory { get; private set; }
    public float decreaseEnergyInterval = 0.05f; // Интервал уменьшения энергии в секундах
    public int currentAge { get; private set; }

      public House home;

    public Gender Gender { get; private set; }
    private AgeGenerator ageGenerator;
    public float ageIncreaseInterval = 1f; // Интервал времени в секундах, за который возраст увеличивается на 1 год

    public Work work = null;
    public int Socialization { get; private set; } = 1000; 
    
    private bool isInvisibleDueToBeingInBuilding = false; // Флаг, контролирующий, стал ли персонаж невидимым, потому что он в здании.

    private void Update()
    {
        // Проверка, вышел ли персонаж из здания и должен ли стать видимым.
        if (isInvisibleDueToBeingInBuilding && !IsAtHome())
        {
            MakeVisible();
            isInvisibleDueToBeingInBuilding = false;
        }
        ManageAnimations();
    }
        private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        unitAnimator = GetComponentInChildren<UnitAnimator>(); // Получаем компонент UnitAnimator

        if (navMeshAgent == null) {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }

        if (unitAnimator == null) {
            Debug.LogError("UnitAnimator component not found on " + gameObject.name);
        }
    }
        private bool m = false;
 private void ManageAnimations()
{
    float speedThreshold = 0.1f; // Порог скорости для определения ходьбы и стояния
    bool isMoving = navMeshAgent.velocity.magnitude > speedThreshold;

    if ( isMoving) {
        unitAnimator.Walk(); // Активируем анимацию ходьбы, если скорость выше порога
        m = true;
        
    } else if (!isMoving) {
        unitAnimator.stand(); // Активируем анимацию стояния, если скорость ниже порога
        m = false;
    }
}
   private IEnumerator DecreaseSocialization()
{
    while (true)
    {
        yield return new WaitForSeconds(10); // Ожидаем 10 секунд перед каждым уменьшением социализации
        Socialization -= 1; // Уменьшаем социализацию на 1
        CheckSocialization(); // Проверяем возможность увеличения социализации в зависимости от окружения

        if (Socialization <= 0)
        {
            Die("Lack of social interaction"); // Персонаж умирает из-за недостатка социального взаимодействия
            break; // Выходим из цикла, так как персонаж мертв
        }
    }
}

    private void CheckSocialization()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 10);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject != gameObject && hitCollider.GetComponent<Human>() != null)
            {
                Socialization = Mathf.Min(Socialization + 5, 1000); // Увеличиваем социализацию с ограничением в 1000
            }
        }
    }

        public void TryRequestWork()
    {
        if (work == null) // Проверяем, есть ли у человека работа
        {
            // Создаем запрос на работу, если работы нет
            WorkRequest workRequest = new WorkRequest(this);
            MainBuilding.Instance.AddRequest(workRequest);
            Debug.Log($"{name} has created a work request.");
        }
    }
        public void TryRequestWorkPlace()
    {
        Farmer farmerWork = work as Farmer;
        // Проверяем, есть ли у Human работа и нужно ли место работы
        if (work != null && farmerWork.farm == null)
        {
            // Создаем и добавляем запрос на место работы
            WorkPlaceRequest workPlaceRequest = new WorkPlaceRequest(this, work);
            MainBuilding.Instance.AddRequest(workPlaceRequest);
            Debug.Log($"{name} has made a work place request for {work.GetType().Name}.");
        }
    }

      public void GoToBuilding(Building building)
    {
        // Персонаж отправляется к зданию.
        navMeshAgent.SetDestination(building.transform.position);
    }

    public void EnterBuilding(Building building)
    {
        // Персонаж входит в здание.
        if (home == building)
        {
            MakeInvisible();
            isInvisibleDueToBeingInBuilding = true;
        }
    }

    // Вызывается, когда персонаж покидает здание.
    public void LeaveBuilding(Building building)
    {
        if (home == building)
        {
            // Проверка, действительно ли персонаж находился в данном здании.
            MakeVisible();
            isInvisibleDueToBeingInBuilding = false;
        }
    }

    public void ReceiveFood(ItemSO foodItem, int quantity)
{
    if (foodItem == null || quantity <= 0) return;

    // Assuming Inventory.AddItem can handle adding multiple items at once
    Inventory.AddItem(foodItem, quantity);
    Debug.Log($"{gameObject.name} received {quantity} of {foodItem.itemName}.");
}
public void CheckForHome()
{
    if (home == null && !HasActiveRequest(typeof(HouseRequest)))
    {
        var houseRequest = new HouseRequest(this);
        MainBuilding.Instance.AddRequest(houseRequest);
        //Debug.Log($"{name} создал запрос на дом.");
    }
}


    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        ageGenerator = FindObjectOfType<AgeGenerator>(); // Находим генератор возраста в сцене
        Age = ageGenerator.GenerateAge(); // Генерируем начальный возраст
        currentAge = 10; // Начинаем отсчет с 30 лет
        Inventory = new Inventory();
        Energy = 1000; // Starting energy

        PeopleManager.Instance.Register(this);
       
        ItemSO carrotMarker = ItemsDatabase.Instance.GetCarrotMarker();
         ItemSO woodMarker = ItemsDatabase.Instance.GetWoodMarker();
        if (carrotMarker != null)
        {
            // Добавляем 5 морковок в инвентарь
            Inventory.AddItem(carrotMarker, 1);
            if(work is Farmer){Inventory.AddItem(carrotMarker, 200);}
           /* if(work is Builder){Inventory.AddItem(woodMarker, 100);}*/
        }

        Debug.Log(Inventory.ToString());

        StartCoroutine(DecreaseEnergyOverTime());
        StartCoroutine(IncreaseAgeOverTime());
        StartCoroutine(DecreaseSocialization());

    }

 public void AssignWork<T>() where T : Work
{
    if (work != null)
    {
        Destroy(work);
    }

    work = gameObject.AddComponent<T>();
    Debug.Log($"{this.name} assigned work: {typeof(T).Name}");
}

 private Dictionary<Type, Request> activeRequests = new Dictionary<Type, Request>();

    public void Eat()
    {
        bool foodFound = false;
        foreach (ItemSO item in Inventory.GetItems())
        {
            if (item is FoodSO food && Energy < 100)
            {
                EatFood(food.itemName);
                foodFound = true;
                break;
            }
        }

        if (!foodFound && !HasActiveRequest(typeof(FoodRequest)))
        {
            CreateFoodRequest();
        }
    }

    private bool HasActiveRequest(Type requestType)
    {
        return activeRequests.ContainsKey(requestType);
    }

   

    // Вызывается MainBuilding после обработки запроса
    public void OnRequestProcessed(Request request)
    {
        // Удаление обработанного запроса из активных
        if (activeRequests.ContainsKey(request.GetType()))
        {
            activeRequests.Remove(request.GetType());
        }
    }

    private void CreateFoodRequest()
    {
        FoodRequest foodRequest = new FoodRequest(this);
        MainBuilding.Instance.AddRequest(foodRequest);
       // Debug.Log($"{gameObject.name} создал запрос на еду.");
    }

    public void Wander()
    {
        Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 10; // 10 - максимальное расстояние перемещения
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10, 1);
        Vector3 finalPosition = hit.position;
        
        navMeshAgent.destination = finalPosition;
    }

      private IEnumerator DecreaseEnergyOverTime()
    {
        while(Energy > 0)
        {
            yield return new WaitForSeconds(decreaseEnergyInterval);
            DecreaseEnergy(1); // Уменьшаем энергию на 1 каждые N секунд
            if (Energy <= 0) Die("Energy depleted");
        }
    }

    public void DecreaseEnergy(int amount)
    {
        Energy -= amount;
       // Debug.Log($"Energy now: {Energy}");
    }

    // Method to eat food and restore energy
    public void EatFood(string foodName)
    {
        FoodSO food = Inventory.FindItemByName(foodName) as FoodSO;
        if (food != null)
        {
            Energy += food.energyRestoration;
            Inventory.RemoveItem(food);
        }
    }

        private IEnumerator IncreaseAgeOverTime()
    {
        while(currentAge < Age)
        {
            yield return new WaitForSeconds(ageIncreaseInterval);
           
            currentAge++; // Увеличиваем текущий возраст на 1 каждый год
            if (currentAge >= Age) Die("Reached old age");
        }
    }
       public void LeaveHouse()
    {
        if (home != null)
        {
            home.RemoveResident(this);
            home = null;
        }
    }

     private void Die(string cause)
    {
        Debug.Log($"{gameObject.name} died due to {cause}.");
        
         PeopleManager.Instance.Unregister(this);
         LeaveHouse();
        Destroy(gameObject);
    }

    public void SetGender(Gender gender)
    {
        Gender = gender;
        
    }
        public void PerformWork()
    {
        if (work != null)
        {
            work.DoWork(); // Выполнение работы
        }
        else
        {
            Debug.Log("У этого человека нет работы.");
        }
    }

      public void GoHome()
    {
        // Проверяем, есть ли дом и не слишком ли мы близко уже
        if (home != null && !IsAtHome())
        {
            navMeshAgent.SetDestination(home.transform.position);
        }
        else
        {
            // Если мы уже близко к дому, останавливаемся и становимся невидимыми
            navMeshAgent.ResetPath();
            MakeInvisible();
        }
    }

     public bool IsAtHome()
    {
        if(home != null){
        return Vector3.Distance(transform.position, home.transform.position) < thresholdDistance;}
        else{return false;}
    }

    public void SetHome(House house)
    {
        home = house;
         RemoveAllHomeRequests();
    }
    private void RemoveAllHomeRequests()
    {
        MainBuilding.Instance.RemoveAllRequestsByRequesterAndType<HouseRequest>(this);
    }

      public void MakeInvisible()
    {
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().enabled = false;
        }
    }

    // Сделать персонажа видимым
    public void MakeVisible()
    {
        if (GetComponent<Renderer>() != null)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }

}
