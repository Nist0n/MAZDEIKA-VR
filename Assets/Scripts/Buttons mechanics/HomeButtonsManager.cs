using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeButtonsManager : MonoBehaviour
{
    [Header("FirstListOfSpells")]
    [SerializeField] private GameObject stunningAttackSkill;
    [SerializeField] private GameObject igniteSkill;

    [Header("SecondtListOfSpells")]
    [SerializeField] private GameObject defenceSkill;
    [SerializeField] private GameObject cleanSkilll;

    [Header("ThirdtListOfSpells")]
    [SerializeField] private GameObject increaseDamageSkill;
    [SerializeField] private GameObject healSkill;

    private void Start()
    {
        if (SaveSystem.instance.firstEnemyDefeated)
        {
            FirstListOfSpells();
        }

        if (SaveSystem.instance.secondEnemyDefeated)
        {
            SecondtListOfSpells();
        }

        if (SaveSystem.instance.thirdEnemyDefeated)
        {
            ThirdtListOfSpells();
        }
    }

    private void FirstListOfSpells()
    {
        stunningAttackSkill.SetActive(true);
        igniteSkill.SetActive(true);
    }

    private void SecondtListOfSpells()
    {
        defenceSkill.SetActive(true);
        cleanSkilll.SetActive(true);
    }

    private void ThirdtListOfSpells()
    {
        increaseDamageSkill.SetActive(true);
        healSkill.SetActive(true);
    }

    public void PlaySoundClick()
    {
        AudioManager.instance.PlaySFX("Click");
    }
}
