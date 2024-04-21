using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitGame : MonoBehaviour
{
    // Вызывается при нажатии кнопки выхода в игре
    public void ExitGame()
    {
        // Вывод сообщения в консоль
        Debug.Log("Exiting game...");

        // Завершение работы игры в зависимости от того, где она выполняется
        #if UNITY_EDITOR
        // Только если игра запущена в редакторе Unity
        EditorApplication.isPlaying = false;
        #else
        // Завершение работы собранной версии игры
        Application.Quit();
        #endif
    }
}
