using System.Collections;
using UnityEngine;
using TMPro; 

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timerText; 
    
    private float time;

    void Start()
    {
        // Инициализация таймера
        time = 0f;
        
        // Запуск корутины для обновления таймера
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while(true)
        {
            time += Time.deltaTime;
            UpdateTimerText();
            yield return null; // Ожидание до следующего кадра
        }
    }

    void UpdateTimerText()
    {
        // Форматирование времени в минуты:секунды
        int minutes = (int)time / 60;
        int seconds = (int)time % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
