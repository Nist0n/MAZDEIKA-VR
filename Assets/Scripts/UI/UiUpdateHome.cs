using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiUpdateHome : MonoBehaviour
{
    [SerializeField] private GameObject chetvertFinal1;
    [SerializeField] private GameObject chetvertFinal2;
    [SerializeField] private GameObject chetvertFinal3;
    [SerializeField] private GameObject chetvertFinal4;
    [SerializeField] private GameObject chetvertFinal5;
    [SerializeField] private GameObject chetvertFinal6;
    [SerializeField] private GameObject chetvertFinal7;
    [SerializeField] private GameObject chetvertFinal8;
    [SerializeField] private GameObject semifinal1;
    [SerializeField] private GameObject semifinal2;
    [SerializeField] private GameObject semifinal3;
    [SerializeField] private GameObject semifinal4;
    [SerializeField] private GameObject final1;
    [SerializeField] private GameObject final2;
    [SerializeField] private GameObject semiFinalsRes1;
    [SerializeField] private GameObject semiFinalsRes2;
    [SerializeField] private GameObject semiFinalsRes3;
    [SerializeField] private GameObject semiFinalsRes4;

    private bool _firstEnemyDefeated = false;
    private bool _secondEnemyDefeated = false;

    private void Update()
    {
        DisplayChetvertFinals();
        
        DisplaySemiFinals();
    }

    private void DisplayChetvertFinals()
    {
        if (SaveSystem.instance.secondEnemyDefeated && !_firstEnemyDefeated)
        {
            chetvertFinal1.SetActive(true);
            chetvertFinal2.SetActive(true);
            chetvertFinal3.SetActive(true);
            chetvertFinal4.SetActive(true);
            chetvertFinal5.SetActive(true);
            chetvertFinal6.SetActive(true);
            chetvertFinal7.SetActive(true);
            chetvertFinal8.SetActive(true);
            semifinal1.SetActive(true);
            semifinal2.SetActive(true);
            semifinal3.SetActive(true);
            semifinal4.SetActive(true);
            _firstEnemyDefeated = true;
        }
    }
    
    private void DisplaySemiFinals()
    {
        if (SaveSystem.instance.thirdEnemyDefeated && !_secondEnemyDefeated)
        {
            final1.SetActive(true);
            final2.SetActive(true);
            semiFinalsRes1.SetActive(true);
            semiFinalsRes2.SetActive(true);
            semiFinalsRes3.SetActive(true);
            semiFinalsRes4.SetActive(true);
            _secondEnemyDefeated = true;
        }
    }
}
