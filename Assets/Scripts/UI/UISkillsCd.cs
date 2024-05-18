using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillsCd : MonoBehaviour
{
    [SerializeField] private GameObject stunCd;
    [SerializeField] private GameObject defenceCd;
    [SerializeField] private GameObject igniteCd;
    [SerializeField] private GameObject healCd;
    [SerializeField] private GameObject increaseDamageCd;

    private void Start()
    {
        if (SaveSystem.instance.firstEnemyDefeated)
        {
            stunCd.SetActive(true);
            igniteCd.SetActive(true);
        }

        if (SaveSystem.instance.secondEnemyDefeated)
        {
            defenceCd.SetActive(true);
        }

        if (SaveSystem.instance.thirdEnemyDefeated)
        {
            healCd.SetActive(true);
            increaseDamageCd.SetActive(true);
        }
    }
}
