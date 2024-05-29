
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveAchievement : MonoBehaviour
{
    public static AchieveAchievement instance;
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBoolParamToAchievement(string achievementName)
    {
        var a = SaveSystem.instance.achievementsConditions.Find(x => x.name == achievementName);
        {
            a.condition = true;
            SaveSystem.instance.Save();
        }
        
        var listOfAchievements = GameObject.FindGameObjectsWithTag("Achievement");
        foreach (var achievement in listOfAchievements)
        {
            if (achievement.name == achievementName)
            {
                AudioManager.instance.PlaySFX("AchievementCompleted");
                achievement.GetComponent<ShowAchievement>().StartAnimAchievement();
            }
        }
    }

    public void SumBeatOffSpells()
    {
        SaveSystem.instance.sumBeatOffSpells++;
        if (SaveSystem.instance.sumBeatOffSpells >= 100) 
        {
            if (CompleteAchievement("Cast100Spells") == false)
            {
                SetBoolParamToAchievement("Cast100Spells");
            }
        }
        SaveSystem.instance.Save();
    }

    public bool CompleteAchievement(string achievementName)
    {
        var a = SaveSystem.instance.achievementsConditions.Find(x => x.name == achievementName);
        {
            return a.condition;
        }
    }
}
