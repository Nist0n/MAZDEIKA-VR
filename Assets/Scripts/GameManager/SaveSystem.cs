using Achivments;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    [SerializeField] private GameObject buttons;
    
    public static SaveSystem instance;

    private const string Key = "mainSave";

    public bool isArchimage;
    public bool firstEnemyDefeated;
    public bool secondEnemyDefeated;
    public bool thirdEnemyDefeated;
    public bool fourthEnemyDefeated;
    public List<Conditions> achivmentsConditions;

    private ButtonsManager _buttonsManager;

    private void Start()
    {
        _buttonsManager = buttons.GetComponent<ButtonsManager>();
        AchievementCompleted("gg");
        foreach (var VARIABLE in _buttonsManager.AchievementButton)
        {
            Debug.Log(VARIABLE.name);
        }
    }

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
            achivmentsConditions = this.achivmentsConditions,
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
            achivmentsConditions = data.achivmentsConditions;
        }
    }

    public void AchievementCompleted(string achievementName)
    {
        var a = achivmentsConditions.Find(x => x.name == achievementName);
        {
            if (a.condition == true)
            {
                _buttonsManager.AchievementButton.Find(x => x.name == achievementName).SetActive(true);
            }
        }
    }
}
