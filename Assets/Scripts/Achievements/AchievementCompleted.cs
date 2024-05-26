using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCompleted : MonoBehaviour
{
    [SerializeField] private GameObject objectButtonManager;

    private ButtonsManager _buttonsManager;

    private void Start()
    {
        _buttonsManager = objectButtonManager.GetComponent<ButtonsManager>();

        foreach (var achievment in _buttonsManager.AchievementButton)
        {
            CompleteAchievement(achievment.name);
        }
    }

    private void CompleteAchievement(string achievementName)
    {
        var a = SaveSystem.instance.achievementsConditions.Find(x => x.name == achievementName);
        Debug.Log(a);
        {
            if (a.condition == true)
            {
                _buttonsManager.AchievementButton.Find(x => x.name == achievementName).SetActive(true);
            }
        }
    }
}
