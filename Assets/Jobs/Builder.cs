using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : Work
{
    public Human human;
    public List<GameObject> buildingsPrefabs;

    public LayerMask buildingLayer; // Слой для обнаружения других зданий
    public float maxDistanceToTry = 5000f; // Максимальное расстояние для поиска нового места строительства
    public float checkRadius = 20f; // Радиус проверки наличия других зданий

    private List<int> ListFarmsIDs =  new List<int> {0,1, 4,5};
     public float buildCheckDelay = 30f;

    private void Start()
    {
        human = GetComponent<Human>();
        buildingsPrefabs = new List<GameObject>();
        AddAllBuildingPrefabsFromResources("Buildings");
        buildingLayer = LayerMask.GetMask("Building"); // Инициализация слоя "Building"

         StartCoroutine(CheckForWorkPlaceRequests());
        StartCoroutine(CheckForHouseRequestsPeriodically());
    }


    

    private IEnumerator CheckForHouseRequestsPeriodically()
{
    // Бесконечный цикл корутины
    while (true)
    {
        // Вызывайте ваш метод проверки здесь
        CheckForHouseRequests();

        // Задержка перед следующим вызовом
        yield return new WaitForSeconds(60); // Пауза в 3 минуты (180 секунд)
    }
}
    private IEnumerator CheckForWorkPlaceRequests() {
        while (true) {
            WorkPlaceRequest workPlaceRequest = MainBuilding.Instance.GetNextWorkPlaceRequest();
            if (workPlaceRequest != null) {
                // Если есть запрос на рабочее место, пытаемся построить здание
                int randomIndex = Random.Range(0, ListFarmsIDs.Count); // Получаем случайный индекс из списка
                int randomBuildingID = ListFarmsIDs[randomIndex]; // Получаем ID фермы по случайному индексу
                DoWork(randomBuildingID); // Вызываем DoWork с ID выбранной фермы
                // Обрабатываем только один запрос за раз
                yield return new WaitForSeconds(buildCheckDelay); // Ждем перед следующей проверкой
            } else {
                // Если запросов нет, ждем перед следующей проверкой
                yield return new WaitForSeconds(buildCheckDelay);
            }
        }
    }


private void CheckForHouseRequests()
{
    // Получаем все активные запросы на жильё
    var houseRequests = MainBuilding.Instance.GetRequestsOfType<HouseRequest>();

    // Проверяем, есть ли активные запросы на жильё
    if (houseRequests.Count > 0)
    {
        // Если есть хотя бы один запрос, начинаем процесс строительства
        InvokeRepeating("DoWorkWithDelay", 0f, 180f); // Запускаем DoWork(1) каждые 3 минуты
    }
    else
    {
        // Если активных запросов на жильё нет, отменяем запланированное повторение метода DoWorkWithDelay
        CancelInvoke("DoWorkWithDelay");
        Debug.Log("Активных запросов на жильё нет. Строительство отложено.");
    }
}

private void DoWorkWithDelay()
{
    // Перед началом строительства снова проверяем наличие запросов
    var houseRequests = MainBuilding.Instance.GetRequestsOfType<HouseRequest>();
    if (houseRequests.Count > 0)
    {
        DoWork(2); // Вызываем метод DoWork(int bID), который строит дом
    }
    else
    {
        Debug.Log("Строительство отменено из-за отсутствия запросов на жильё.");
    }
}

    public override void DoWork()
    {
        /*Vector3 buildLocation = FindBuildLocation();
        if (buildLocation != Vector3.zero)
        {
            BuildAtLocation(buildLocation,0);
        }*/
    }

    /*public override void DoWork(int bID)
    {
        Vector3 buildLocation = FindBuildLocation();
        if (buildLocation != Vector3.zero)
        {
            BuildAtLocation(buildLocation,bID);
        }
    }*/
// Дополнение к классу Builder
bool IsRestrictedLocation(Vector3 location, Building buildingScript) {
    Collider[] hitColliders = Physics.OverlapSphere(location, checkRadius);
    foreach (var hitCollider in hitColliders) {
        if (buildingScript.restrictingTags.Contains(hitCollider.tag)) {
            return true; // Найден объект с ограничивающим тегом
        }
    }
    return false;
}

bool IsNearRequiredTags(Vector3 location, Building buildingScript, float checkRadius = 5f)
{
    if (buildingScript.requiredTags.Count == 0)
    {
        // Если требуемых тегов нет, условие считается выполненным
        return true;
    }

    // Для каждого требуемого тега проверяем наличие касания
    foreach (string tag in buildingScript.requiredTags)
    {
        Collider[] colliders = Physics.OverlapSphere(location, checkRadius);
        foreach (var collider in colliders)
        {
            if (collider.tag == tag)
            {
                // Дополнительно проверяем, есть ли физическое пересечение между зданием и объектом с требуемым тегом
                if (Physics.Raycast(location, collider.transform.position - location, out RaycastHit hit, checkRadius))
                {
                    if (hit.collider.tag == tag)
                    {
                        // Обнаружено физическое касание с объектом требуемого тега
                        return true;
                    }
                }
            }
        }
    }

    // Ни один из объектов с требуемыми тегами не касается места строительства
    return false;
}

// Изменение в методе FindBuildLocation для учета новой логики
Vector3 FindBuildLocation(Building buildingScript) {
    Vector3 averageLocation = BuildingsManager.Instance.GetAverageBuildingLocation();
    float currentDistance = checkRadius;

    Debug.Log($"Начинаем поиск места для строительства {buildingScript.buildingName}");

    while (currentDistance < maxDistanceToTry) {
        for (int attempt = 0; attempt < 10; attempt++) {
            Vector3 randomDirection = GetRandomDirectionOnTerrain();
            Vector3 locationToTry = averageLocation + randomDirection * currentDistance;

            Debug.Log($"Попытка поиска места. Попытка: {attempt}, Расстояние: {currentDistance}");

           /* if (!IsOnTerrain(locationToTry)) {
                Debug.Log($"Место {locationToTry} не на террейне.");
                continue;
            }*/
            if (IsRestrictedLocation(locationToTry, buildingScript)) {
                Debug.Log($"Место {locationToTry} заблокировано ограничивающим тегом.");
                continue;
            }
            if (!IsNearRequiredTags(locationToTry, buildingScript)) {
                Debug.Log($"Место {locationToTry} не соответствует требуемым тегам.");
                continue;
            }

            Debug.Log($"Найдено подходящее место для строительства {buildingScript.buildingName} на {locationToTry}");
            return locationToTry; // Найдено подходящее место для строительства
        }
        currentDistance += checkRadius;
    }

    Debug.LogError($"Не удалось найти подходящее место для строительства {buildingScript.buildingName}.");
    return Vector3.zero;
}







Vector3 GetRandomDirectionOnTerrain() {
    Vector3 randomDirection = Random.insideUnitSphere.normalized;
    randomDirection.y = 0; // Обеспечиваем, чтобы направление было вдоль горизонтальной плоскости
    return randomDirection;
}

bool IsOnTerrain(Vector3 location) {
    // Проверяем, что позиция находится на террейне
    RaycastHit hit;
    if (Physics.Raycast(location + Vector3.up * 50, Vector3.down, out hit, 100, LayerMask.GetMask("Terrain"))) {
        return true; // Точка на террейне
    }
    return false; // Точка не на террейне
}

public override void DoWork(int bID)
{
    GameObject buildingPrefab = buildingsPrefabs[bID];
    Building buildingScript = buildingPrefab.GetComponent<Building>();

    Vector3 buildLocation = FindBuildLocation(buildingScript);
    if (buildLocation == Vector3.zero)
    {
        Debug.LogError("Не найдено подходящее место для строительства.");
        return;
    }

    // Перед началом строительства проверяем, соответствует ли место всем требованиям
    if (!IsNearRequiredTags(buildLocation, buildingScript))
    {
        Debug.LogError($"Место строительства {buildLocation} не соответствует требованиям к требуемым тегам.");
        return;
    }

    GameObject newBuilding = Instantiate(buildingPrefab, buildLocation, Quaternion.identity);
    Building newBuildingScript = newBuilding.GetComponent<Building>();

    if (newBuildingScript != null)
    {
        bool canBuild = true;
        foreach (var requirement in newBuildingScript.requirements)
        {
            if (human.Inventory.GetItemCount(requirement.item) < requirement.quantity)
            {
                MainBuilding.Instance.AddRequest(new ItemRequest(human, requirement.item, requirement.quantity - human.Inventory.GetItemCount(requirement.item)));
                canBuild = false;
                break;
            }
        }

        if (canBuild)
        {
            newBuildingScript.StartConstruction(human.Inventory);
            Debug.Log($"Начинаем строительство: {newBuildingScript.buildingName} на {buildLocation}");
        }
        else
        {
            Destroy(newBuilding);
            Debug.LogError($"Не удалось начать строительство {newBuildingScript.buildingName} из-за недостатка ресурсов.");
        }
    }
}



void BuildAtLocation(Vector3 location, int bID)
{
    GameObject buildingPrefab = buildingsPrefabs[bID];
    GameObject newBuilding = Instantiate(buildingPrefab, location, Quaternion.identity);
    Building newBuildingScript = newBuilding.GetComponent<Building>();
    
    // Проверяем, можно ли построить здание
    if (newBuildingScript != null && newBuildingScript.CanConstruct(human.Inventory))
    {
        // Изъятие ресурсов и начало строительства
        foreach (var requirement in newBuildingScript.requirements)
        {
            human.Inventory.RemoveItems(requirement.item, requirement.quantity);
        }
        newBuildingScript.StartConstruction(human.Inventory);
        Debug.Log($"Начинаем строительство: {newBuildingScript.buildingName}");
    }
    else
    {
        Destroy(newBuilding); // Уничтожаем непостроенное здание, если ресурсов не хватает
        Debug.LogError($"Не удалось начать строительство {newBuildingScript.buildingName} из-за недостатка ресурсов.");
    }
}

    private void AddAllBuildingPrefabsFromResources(string path)
    {
        GameObject[] buildingPrefabs = Resources.LoadAll<GameObject>(path);
        if (buildingPrefabs.Length > 0)
        {
            buildingsPrefabs.AddRange(buildingPrefabs);
            Debug.Log($"Загружено зданий: {buildingPrefabs.Length}");
        }
        else
        {
            Debug.LogError("Не найдены префабы зданий в: " + path);
        }
    }
bool IsBuildingAtLocation(Vector3 location, out GameObject blockingBuilding) {
    blockingBuilding = null;
    foreach (var building in BuildingsManager.Instance.ExistingBuildings) {
        if ((building.transform.position - location).sqrMagnitude <= (checkRadius * checkRadius)) {
            blockingBuilding = building.gameObject; // Устанавливаем объект, который мешает строительству
            return true; // Нашли здание слишком близко к данной точке
        }
    }
    return false; // Зданий поблизости не обнаружено
}
}
