using UnityEngine;
using TMPro; // Подключаем пространство имен для работы с TextMeshPro
using System.Collections.Generic;

public class PeopleManager : MonoBehaviour
{
    public static PeopleManager Instance { get; private set; }

    private List<Human> people = new List<Human>();

    public TextMeshProUGUI populationText; 
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Опционально, если вы хотите, чтобы менеджер сохранялся между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Register(Human person)
    {
        if (!people.Contains(person))
        {
            people.Add(person);
            UpdatePopulationText(); // Обновляем текст при добавлении нового человека
        }
    }

    public void Unregister(Human person)
    {
        if (people.Contains(person))
        {
            people.Remove(person);
            UpdatePopulationText(); // Обновляем текст при удалении человека
        }
    }

    // Вот метод, который вам нужен
    public List<Human> GetAllHumans()
    {
        return people;
    }

    // Обновляет текстовый компонент TextMeshProUGUI с текущим количеством людей
    private void UpdatePopulationText()
    {
        if (populationText != null) // Проверяем, что ссылка на компонент не пуста
        {
            populationText.text = $"Population: {people.Count}"; // Формируем строку с количеством людей
        }
    }

    // Для демонстрации: выводим текущее количество персонажей
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) // Нажмите P, чтобы увидеть количество персонажей
        {
            Debug.Log($"Current number of people: {people.Count}");
        }
    }
}
