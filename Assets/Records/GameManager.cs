using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private List<GameRecord> records = new List<GameRecord>();
    private float startTime;
    private string savePath;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            startTime = Time.time;
            savePath = Path.Combine(Application.persistentDataPath, "gameRecords.json");
            LoadRecords();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadRecords()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            GameRecordsWrapper wrapper = JsonUtility.FromJson<GameRecordsWrapper>(json);
            if (wrapper != null)
            {
                records = wrapper.records;
            }
        }
    }

    public void SaveRecord()
    {
        float timePlayed = Time.time - startTime;
        int buildingsCount = BuildingsManager.Instance.ExistingBuildings.Count;
        int peopleCount = PeopleManager.Instance.GetAllHumans().Count;
        int newId = records.Count + 1;

        GameRecord newRecord = new GameRecord(newId, timePlayed, buildingsCount, peopleCount);
        records.Add(newRecord);
        SaveRecordsToFile();
    }

    private void SaveRecordsToFile()
    {
        GameRecordsWrapper wrapper = new GameRecordsWrapper { records = records };
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(savePath, json);
    }

    public List<GameRecord> GetRecords()
    {
        return records;
    }
}
