using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameRecord
{
    public int id;
    public float timePlayed;
    public int buildingsCount;
    public int peopleCount;

    public GameRecord(int id, float timePlayed, int buildingsCount, int peopleCount)
    {
        this.id = id;
        this.timePlayed = timePlayed;
        this.buildingsCount = buildingsCount;
        this.peopleCount = peopleCount;
    }
}
