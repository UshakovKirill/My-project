using UnityEngine;
using TMPro; 

public class FeatureUnavailable : MonoBehaviour
{
    public TextMeshProUGUI warningText; 

    void Start()
    {
        // Скрываем текст при старте
        if (warningText != null)
            warningText.gameObject.SetActive(false);
    }

    public void ShowWarning()
    {
        // Показываем предупреждение на 3 секунды, затем скрываем
        if (warningText != null)
        {
            warningText.gameObject.SetActive(true);
            Invoke("HideWarning", 3); // Отсчитываем 3 секунды
        }
    }

    void HideWarning()
    {
        warningText.gameObject.SetActive(false);
    }
}

