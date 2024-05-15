using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settings;
    [SerializeField] private GameObject chooseGameDifficulty;
    [SerializeField] private GameObject achivmentsMenu;
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        if (SaveSystem.instance.firstEnemyDefeated == true || SaveSystem.instance.secondEnemyDefeated == true ||
            SaveSystem.instance.thirdEnemyDefeated == true)
        {
            continueButton.SetActive(true);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void SoundSettings()
    {
        Debug.Log("Settings");
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }
    public void BackButton()
    {
        mainMenu.SetActive(true);
        settings.SetActive(false);
        chooseGameDifficulty.SetActive(false);
        achivmentsMenu.SetActive(false);
    }
    public void ChooseDifficulty()
    {
        chooseGameDifficulty.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void OpenAchivmentsMenu()
    {
        mainMenu.SetActive(false);
        achivmentsMenu.SetActive(true);
    }
}
