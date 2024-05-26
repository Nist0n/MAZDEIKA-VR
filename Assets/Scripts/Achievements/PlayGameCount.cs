using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGameCount : MonoBehaviour
{
    private void Start()
    {
        SaveSystem.instance.playGameCount++;

        if (CompleteAchievement("FirstPlay") == false && SaveSystem.instance.playGameCount == 1)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("FirstPlay");
        }

        if (CompleteAchievement("SecondPlay") == false && SaveSystem.instance.playGameCount == 2)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("SecondPlay");
        }

        if (CompleteAchievement("ThirdPlay") == false && SaveSystem.instance.playGameCount == 3)
        {
            AchieveAchievement.instance.SetBoolParamToAchievement("ThirdPlay");
        }

        SaveSystem.instance.Save();
    }

    private bool CompleteAchievement(string achievementName)
    {
        var a = SaveSystem.instance.achievementsConditions.Find(x => x.name == achievementName);
        {
            return a.condition;
        }
    }
}
