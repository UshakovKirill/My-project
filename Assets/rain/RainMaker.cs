using System.Collections;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    public ParticleSystem rainParticleSystem; // Ссылка на систему частиц дождя на сцене

    // Временные интервалы для включения/выключения дождя
    private float minTime ;
    private float maxTime ;

    // Глобальный параметр состояния дождя
    public static bool IsRaining { get; private set; }

    private void Start()
    {
        // Запускаем корутину управления дождем
        StartCoroutine(ToggleRain());
    }

   private void Awake()
    {
        // Инициализация minTime и maxTime, значения примерно из статического класса
        // Допустим, у нас есть класс TimingSettings с соответствующими полями
        InitializeTiming();
    }

     private void InitializeTiming()
    {
        if (float.TryParse(GameSettings.Rmin, out float parsedMinTime))
        {
            minTime = parsedMinTime;
        }
        else
        {
            minTime = 100f; // Значение по умолчанию, если парсинг не удался
        }

        if (float.TryParse(GameSettings.Rmax, out float parsedMaxTime))
        {
            maxTime = parsedMaxTime;
        }
        else
        {
            maxTime = 500f; // Значение по умолчанию, если парсинг не удался
        }
        Debug.Log("minTime");
        Debug.Log(minTime);
        Debug.Log("maxTime");
        Debug.Log(maxTime);
    }

   

    private IEnumerator ToggleRain()
    {
        while (true)
        {
            // Выключаем дождь
            rainParticleSystem.Stop();
            IsRaining = false; // Обновляем глобальный параметр
            Debug.Log("Rain stopped");

            // Ждем случайный период времени
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));

            // Включаем дождь
            rainParticleSystem.Play();
            IsRaining = true; // Обновляем глобальный параметр
            Debug.Log("Rain started");

            // Опять ждем случайный период времени
            yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        }
    }
}
