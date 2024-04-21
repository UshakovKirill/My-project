using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanSpawner : MonoBehaviour
{
    public GameObject humanPrefab; // Префаб Human, который должен содержать компонент Human
private void Start()
{
    // Спавним людей
    Human male = SpawnHuman(Gender.Male);
    Human female = SpawnHuman(Gender.Female);
    Human female2 = SpawnHuman(Gender.Female);

    // Если male не null, назначаем ему работу Builder
    if (male != null)
    {
        male.AssignWork<Builder>(); // Теперь у male есть работа Builder
    }

     if (female != null)
    {
        //ItemSO carrotMarker = ItemsDatabase.Instance.GetCarrotMarker();
        female.AssignWork<Farmer>(); // Теперь у male есть работа Builder
       
    }
      if (female2 != null)
    {
        //ItemSO carrotMarker = ItemsDatabase.Instance.GetCarrotMarker();
        female2.AssignWork<Farmer>(); // Теперь у male есть работа Builder
       
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
}
