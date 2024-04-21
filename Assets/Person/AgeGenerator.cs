using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AgeGenerator : MonoBehaviour
{
    public float mean = 65f; // Среднее значение возраста
    public float stdDev = 20f; // Стандартное отклонение

    // Генерирует возраст с использованием нормального распределения
    public int GenerateAge()
    {
        float u1 = 1.0f - Random.Range(0f, 1f); // равномерное распределение (0,1]
        float u2 = 1.0f - Random.Range(0f, 1f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                              Mathf.Sin(2.0f * Mathf.PI * u2); // случайная величина с нормальным распределением
        float age = mean + stdDev * randStdNormal; // случайная величина со средним и стандартным отклонением
        return Mathf.Clamp((int)age, 30, 100); // Ограничиваем возраст диапазоном от 30 до 100
    }
}
