
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : Building
{
    public GameObject humanPrefab; // Assign this in the inspector with  Human prefab
    public List<Human> residents;
    private int maxResidents ;
     private bool checkDoneTonight = false;

    private void Awake()
    {
        residents = new List<Human>();
          InitializeMaxResidents();
    }
        private void InitializeMaxResidents()
    {
       
        if (int.TryParse(GameSettings.PHnum, out int parsedValue))
        {
            maxResidents = parsedValue;
        }
        else
        {
            maxResidents = 10; // Установите значение по умолчанию, если парсинг не удался
        }
        Debug.Log("MaxPeoplePerH:");Debug.Log(maxResidents);
    }

    private void Start()
    {
        BuildingsManager.Instance.RegisterBuilding(this);
        InvokeRepeating("CheckForHomelessHumans", 1.0f, 1.0f);
        StartCoroutine(CheckForNewHumanEachNight());
          gameObject.tag = "Building";
    }
private void AddResident(Human human)
    {
        residents.Add(human);
        human.SetHome(this);
    }
    private void CheckForHomelessHumans()
    {
         
        foreach (var human in PeopleManager.Instance.GetAllHumans())
        {
            if (human.home == null && residents.Count < maxResidents)
            {
                AddResident(human);
            }
        }
    }

    private IEnumerator CheckForNewHumanEachNight()
    {
        while (true)
        {
            // Wait until it's night time to perform the check
            yield return new WaitUntil(() => SimpleDayNightCycle.isNight);
            // Ensure we only perform the check once each night
            if (!checkDoneTonight)
            {
                // Perform the check with 10% chance to spawn a new human
                if (HasMaleAndFemale() && Random.Range(0, 100) < 100)
                {
                    SpawnNewHuman();
                }
                checkDoneTonight = true; // Mark that the check has been done
            }

            // Now wait until it's day to reset the check
            yield return new WaitUntil(() => !SimpleDayNightCycle.isNight);
            checkDoneTonight = false; // Reset for the next night
        }
    }

    private bool HasMaleAndFemale()
    {
        bool hasMale = false;
        bool hasFemale = false;
        foreach (var resident in residents)
        {
            if (resident.Gender == Gender.Male) hasMale = true;
            if (resident.Gender == Gender.Female) hasFemale = true;
            if (hasMale && hasFemale) return true;
        }
        return false;
    }

    private void SpawnNewHuman()
    {
       // if (residents.Count >= maxResidents) return; // No room for more residents

        // Randomly decide the gender of the new human
        Gender newHumanGender = (Random.Range(0, 2) == 0) ? Gender.Male : Gender.Female;

        Human newHuman = SpawnHuman(newHumanGender);
        if (newHuman != null)
        {
            // Set the new human's home to this house
            //newHuman.SetHome(this);
            // Add the new human to the residents list
            //residents.Add(newHuman);
            Debug.Log($"A new human has been born! Gender: {newHumanGender}");
        }
    }

    private Human SpawnHuman(Gender gender)
    {
        GameObject instance = Instantiate(humanPrefab, transform.position, Quaternion.identity);
        Human human = instance.GetComponent<Human>();
        if (human != null)
        {
            human.SetGender(gender);
           
        }
        return human;
    }
        public void RemoveResident(Human human)
    {
        if (residents.Contains(human))
        {
            residents.Remove(human);
        }
    }
}
