using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDIfficulty : MonoBehaviour
{
    public void SetDifficultyArchemage()
    {
        SaveSystem.instance.isArchimage = true;
        SaveSystem.instance.firstEnemyDefeated = false;
        SaveSystem.instance.secondEnemyDefeated = false;
        SaveSystem.instance.fourthEnemyDefeated = false;
        SaveSystem.instance.thirdEnemyDefeated = false;
        SaveSystem.instance.Save();
    }
    
    public void SetDifficultyMage()
    {
        SaveSystem.instance.isArchimage = false;
        SaveSystem.instance.firstEnemyDefeated = false;
        SaveSystem.instance.secondEnemyDefeated = false;
        SaveSystem.instance.fourthEnemyDefeated = false;
        SaveSystem.instance.thirdEnemyDefeated = false;
        SaveSystem.instance.Save();
    }
}
