using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private TextMeshProUGUI infoTextMesh;
    [SerializeField] private TextMeshProUGUI currentAge;
    [SerializeField] private TextMeshProUGUI dAge;
    [SerializeField] private TextMeshProUGUI inventoryText;
    [SerializeField] private TextMeshProUGUI genderText;
    [SerializeField] private TextMeshProUGUI jobText;
    [SerializeField] private GameObject humanInterface;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHumanUI(Human human)
    {
        if (human == null) return;

        infoTextMesh.text = $"Energy: {human.Energy}";
        currentAge.text = $"Age: {human.currentAge}";
        dAge.text = $"Death Age: {human.Age}";
        inventoryText.text = "Inventory: " + human.Inventory.ToString();
        genderText.text = $"Gender: {human.Gender}";
        jobText.text = $"Job: {human.work}";

        humanInterface.SetActive(true);
    }

    public void HideHumanInterface()
    {
        humanInterface.SetActive(false);
    }
}
