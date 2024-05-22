using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchieveAchievement : MonoBehaviour
{
    private void SetBoolParamToAchievement(string achievementName)
    {
        var a = SaveSystem.instance.achievementsConditions.Find(x => x.name == achievementName);
        {
            a.condition = true;
            SaveSystem.instance.Save();
        }
    }
}
