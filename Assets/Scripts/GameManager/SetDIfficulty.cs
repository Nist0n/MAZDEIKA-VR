using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDIfficulty : MonoBehaviour
{
    public void SetDifficultyArchemage()
    {
        SaveSystem.instance.isArchimage = true;
        SaveSystem.instance.Save();
    }
    
    public void SetDifficultyMage()
    {
        SaveSystem.instance.isArchimage = false;
        SaveSystem.instance.Save();
    }
}
