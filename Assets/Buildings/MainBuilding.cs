using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MainBuilding : Building
{
    public static MainBuilding Instance { get; private set; }

    private List<Request> requests = new List<Request>();


        public WorkPlaceRequest GetNextWorkPlaceRequest()
    {
        // Используем LINQ для поиска запроса на рабочее место
        var workPlaceRequest = requests.OfType<WorkPlaceRequest>().FirstOrDefault();
        if (workPlaceRequest != null)
        {
            requests.Remove(workPlaceRequest); // Удаляем найденный запрос из общего списка
        }
        return workPlaceRequest;
    }

     private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            PrintAllRequests();
        }
        RemoveInvalidRequests();
         RemoveDuplicateRequests();
         AssignWorkToRequesters();
    }

        private void PrintAllRequests()
    {
        Debug.Log("Requests: ");
        foreach (var request in requests)
        {
           
            Debug.Log(request.Description);
        }
    }
    public void AssignWorkToRequesters()
{
    var workRequests = GetRequestsOfType<WorkRequest>();

    foreach (var request in workRequests)
    {
        // Решаем, какую работу назначить
     float chance = Random.Range(0f, 1f);
if (chance <= 0.7f)
{
    // 70% chance
    request.Requester.AssignWork<Farmer>();
}
else if (chance <= 0.85f)
{
    // Next 15% chance
    request.Requester.AssignWork<Builder>();
}
else
{
    // Remaining 15% chance
    request.Requester.AssignWork<Loafer>();
}

        // Уведомляем о назначении работы
        Debug.Log($"{request.Requester.name} has been assigned a new work.");

        // Удаляем запрос после обработки
        RemoveRequest(request);
    }
}

public List<T> GetRequestsOfType<T>() where T : Request
{
    return requests.OfType<T>().ToList(); // This should work now with using System.Linq;
}


       private void RemoveInvalidRequests()
    {
        for (int i = requests.Count - 1; i >= 0; i--)
        {
            if (requests[i].Requester == null)
            {
                requests.RemoveAt(i);
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
           // DontDestroyOnLoad(gameObject); // Если MainBuilding должен сохраняться между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }
  public void AddRequest(Request newRequest)
{
    // Проверяем, существует ли уже такой запрос в списке
    bool duplicateExists = requests.Any(r => r.Requester == newRequest.Requester && r.GetType() == newRequest.GetType());

    // Если дубликат не найден, добавляем запрос в список
    if (!duplicateExists)
    {
        requests.Add(newRequest);
    }
}
     public void RemoveRequest(Request request)
    {
        if (requests.Contains(request))
        {
            requests.Remove(request);
        }
    }

public void ProcessRequests()
{
    // Process each request
    foreach (var request in requests)
    {
       
        Debug.Log(request.Requester.name + " made a request.");
        
        // Once the request is processed, notify the requester
        request.Requester.OnRequestProcessed(request);
    }
    
    // Clear requests after processing
    requests.Clear();
}
private void RemoveDuplicateRequests()
{
    // Используем LINQ для группировки запросов по Requester и типу запроса
    var groupedRequests = requests.GroupBy(r => new { r.Requester, RequestType = r.GetType() })
                                  .Select(group => group.First()) // Для каждой группы берем только первый элемент
                                  .ToList(); // Преобразуем результат в список

    requests = groupedRequests; // Обновляем список запросов, убрав дубликаты
}


   public void RemoveAllRequestsByRequesterAndType<T>(Human requester) where T : Request
    {
        requests.RemoveAll(request => request.Requester == requester && request is T);
    }
}

class RequestComparer : IEqualityComparer<Request>
{
    public bool Equals(Request x, Request y)
    {
        if (x == null || y == null)
            return false;

       
        return x.Requester == y.Requester && x.GetType() == y.GetType();
    }

    public int GetHashCode(Request obj)
    {
        
        return obj.Requester.GetInstanceID() ^ obj.GetType().GetHashCode();
    }
}


