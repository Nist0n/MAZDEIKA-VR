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

    private bool _firstEnemyDefeated = false;
    private bool _secondEnemyDefeated = false;
    private bool _thirdEnemyDefeated = false;

    private void Update()
    {
        DisplayChetvertFinals();
        
        DisplaySemiFinals();
        
        DisplayFinals();
    }

    private void DisplayChetvertFinals()
    {
        if (SaveSystem.instance.firstEnemyDefeated && !_firstEnemyDefeated)
        {
            chetvertFinal1.SetActive(true);
            chetvertFinal2.SetActive(true);
            chetvertFinal3.SetActive(true);
            chetvertFinal4.SetActive(true);
            chetvertFinal5.SetActive(true);
            chetvertFinal6.SetActive(true);
            chetvertFinal7.SetActive(true);
            chetvertFinal8.SetActive(true);
            _firstEnemyDefeated = true;
        }
    }
    
    private void DisplaySemiFinals()
    {
        if (SaveSystem.instance.secondEnemyDefeated && !_secondEnemyDefeated)
        {
            semifinal1.SetActive(true);
            semifinal2.SetActive(true);
            semifinal3.SetActive(true);
            semifinal4.SetActive(true);
            _secondEnemyDefeated = true;
        }
    }

    private void DisplayFinals()
    {
        if (SaveSystem.instance.thirdEnemyDefeated && !_thirdEnemyDefeated)
        {
            final1.SetActive(true);
            final2.SetActive(true);
            _thirdEnemyDefeated = true;
        }
    }
}
