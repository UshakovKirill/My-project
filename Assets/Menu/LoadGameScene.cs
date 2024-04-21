using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; 

public class LoadGameScene : MonoBehaviour
{
    
    public TMP_InputField phnumField;
    public TMP_InputField rminField;
    public TMP_InputField rmaxField;

    // Метод для загрузки сцены "Game"
    public void LoadGame()
    {
        SceneManager.LoadScene("Settings");
    }

     public void LoadRec()
    {
        SceneManager.LoadScene("Records");
    }

      public void LoadMenu()
    {
        // Удаление всех неуничтожаемых объектов
        foreach (var go in FindObjectsOfType<GameObject>())
        {
            if (go.scene.buildIndex == -1) // Ищем объекты в DontDestroyOnLoad сцене
            {
                Destroy(go); // Уничтожаем объект
            }
        }

        // Загрузка сцены "MainMenu"
        SceneManager.LoadScene("MainMenu");
    }

    // Метод для загрузки сцены "Game"
    public void LoadGameStart()
    {
        // Сохраняем значения полей ввода в статический класс перед переходом на новую сцену
       
        GameSettings.PHnum = phnumField.text;
        GameSettings.Rmin = rminField.text;
        GameSettings.Rmax = rmaxField.text;

        // Загрузка новой сцены
        SceneManager.LoadScene("Game");
    }
}
