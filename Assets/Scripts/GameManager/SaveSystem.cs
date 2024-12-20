using Achivments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    private const string Key = "mainSave";

    public bool isStarted = false;
    public bool firstStart;
    public bool isArchimage;
    public bool firstEnemyDefeated;
    public bool secondEnemyDefeated;
    public bool thirdEnemyDefeated;
    public bool fourthEnemyDefeated;
    public int sumBeatOffSpells;
    public int playGameCount;
    public List<Conditions> achievementsConditions;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        DataBase.Save(Key, GetSaveSnapshot());
    }
    
    public GameData GetSaveSnapshot()
    {
        var data = new GameData()
        {
            isArchimage = this.isArchimage,
            firstEnemyDefeated = this.firstEnemyDefeated,
            secondEnemyDefeated = this.secondEnemyDefeated,
            thirdEnemyDefeated = this.thirdEnemyDefeated,
            fourthEnemyDefeated = this.fourthEnemyDefeated,
            achivmentsConditions = this.achievementsConditions,
            sumBeatOffSpells = this.sumBeatOffSpells,
            playGameCount = this.playGameCount,
            firstStart = this.firstStart,
        };

        return data;
    }
    
    public void Load()
    {
        var data = DataBase.Load<GameData>(Key);

        if (data != null)
        {
            isArchimage = data.isArchimage;
            firstEnemyDefeated = data.firstEnemyDefeated;
            secondEnemyDefeated = data.secondEnemyDefeated;
            thirdEnemyDefeated = data.thirdEnemyDefeated;
            fourthEnemyDefeated = data.fourthEnemyDefeated;
            achievementsConditions = data.achivmentsConditions;
            sumBeatOffSpells = data.sumBeatOffSpells;
            playGameCount = data.playGameCount;
            firstStart = data.firstStart;
        }
    }
}
