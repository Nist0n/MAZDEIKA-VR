using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameCount : MonoBehaviour
{
    private void Start()
    {
        SaveSystem.instance.Load();
        
        if (!SaveSystem.instance.firstStart)
        {
            SaveSystem.instance.firstStart = true;
            SaveSystem.instance.Save();
        }
        SaveSystem.instance.Save();
        SaveSystem.instance.Load();

        if (!SaveSystem.instance.isStarted) SaveSystem.instance.playGameCount++;

        if (AchieveAchievement.instance.CompleteAchievement("FirstPlay") == false && SaveSystem.instance.playGameCount == 1)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("FirstPlay");
        }

        if (AchieveAchievement.instance.CompleteAchievement("SecondPlay") == false && SaveSystem.instance.playGameCount == 2)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("SecondPlay");
        }

        if (AchieveAchievement.instance.CompleteAchievement("ThirdPlay") == false && SaveSystem.instance.playGameCount == 3)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("ThirdPlay");
        }
        
        SaveSystem.instance.Save();
        SaveSystem.instance.isStarted = true;
    }
}
