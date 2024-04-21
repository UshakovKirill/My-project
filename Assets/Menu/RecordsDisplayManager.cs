using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class RecordsDisplayManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI[] recordTexts; 
    [SerializeField] private Button previousButton;
    [SerializeField] private Button nextButton;

    private List<GameRecord> records;
    private int currentPage = 0;
    private int recordsPerPage = 5;

    void Start()
    {
        records = GameManager.Instance.GetRecords(); 
        UpdateDisplay();
        previousButton.onClick.AddListener(PreviousPage);
        nextButton.onClick.AddListener(NextPage);
    }

    void UpdateDisplay()
    {
        int recordStartIndex = currentPage * recordsPerPage;
        for (int i = 0; i < recordTexts.Length; i++)
        {
            if (recordStartIndex + i < records.Count)
            {
                var record = records[recordStartIndex + i];
                recordTexts[i].text = $"ID: {record.id}, Time: {record.timePlayed:N2}, Buildings: {record.buildingsCount}, People: {record.peopleCount}";
                recordTexts[i].gameObject.SetActive(true);
            }
            else
            {
                recordTexts[i].gameObject.SetActive(false);
            }
        }

        previousButton.interactable = currentPage > 0;
        nextButton.interactable = recordStartIndex + recordsPerPage < records.Count;
    }

    void NextPage()
    {
        if (currentPage * recordsPerPage < records.Count)
        {
            currentPage++;
            UpdateDisplay();
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateDisplay();
        }
    }
}
