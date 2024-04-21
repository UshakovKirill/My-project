using UnityEngine;
using TMPro;

public class SimpleDayNightCycle : MonoBehaviour
{
    public Light directionalLight;
    public Color middayColor;
    public Color midnightColor;
    public float dayDuration = 120.0f;
    public static bool isNight = false;

    public TextMeshProUGUI dayNightText;

    private float time;
    private float transitionDuration = 3f; // Продолжительность перехода в секундах
    private float transitionStartTime; // Время начала перехода
    private bool isTransitioning = false; // Находимся ли мы в переходном состоянии
    private Color startColor; // Цвет в начале перехода
    private float startIntensity; // Интенсивность в начале перехода

    void Update()
    {
        time += Time.deltaTime;
        float timeOfDay = Mathf.Sin(time / dayDuration * Mathf.PI * 2) * 0.5f + 0.5f;

        // Проверяем, необходимо ли начать переход
        if ((timeOfDay < 0.25f || timeOfDay > 0.75f) && !isNight && !isTransitioning)
        {
            StartTransition(true);
        }
        else if (timeOfDay >= 0.25f && timeOfDay <= 0.75f && isNight && !isTransitioning)
        {
            StartTransition(false);
        }

        if (isTransitioning)
        {
            // Выполняем переход
            float transitionProgress = (Time.time - transitionStartTime) / transitionDuration;
            if (transitionProgress < 1f)
            {
                // Плавное изменение цвета и интенсивности
                directionalLight.color = Color.Lerp(startColor, isNight ? midnightColor : middayColor, transitionProgress);
                directionalLight.intensity = Mathf.Lerp(startIntensity, isNight ? 0.5f : 1, transitionProgress);
            }
            else
            {
                // Завершение перехода
                isTransitioning = false;
                directionalLight.color = isNight ? midnightColor : middayColor;
                directionalLight.intensity = isNight ? 0.5f : 1;
            }
        }

        // Регулировка окружающего освещения
        RenderSettings.ambientLight = directionalLight.color;
    }

    private void StartTransition(bool toNight)
    {
        isNight = toNight;
        dayNightText.text = isNight ? "Night" : "Day";
        transitionStartTime = Time.time;
        isTransitioning = true;
        startColor = directionalLight.color;
        startIntensity = directionalLight.intensity;
    }
}
