using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Объект интерфейса, который будет показываться или скрываться
    public GameObject menuInterface;

    // Проверяем состояние интерфейса
    private bool isMenuVisible = false;

    private void Update()
    {
        // Отслеживаем нажатие клавиши Esc
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    // Функция для переключения видимости меню
    public void ToggleMenu()
    {
        // Изменяем состояние видимости
        isMenuVisible = !isMenuVisible;

        // Устанавливаем активность объекта интерфейса
        if (menuInterface != null)
        {
            menuInterface.SetActive(isMenuVisible);
        }
        else
        {
            Debug.LogError("Menu interface not assigned.");
        }
    }
}
