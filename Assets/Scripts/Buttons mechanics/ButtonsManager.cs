using Achivments;
using DialogueEditor;
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
    [SerializeField] private GameObject achievmentsMenu;
    [SerializeField] public GameObject continueButton;
    [SerializeField] private Animator animator;
    public List<GameObject> AchievementButton;

    private void Start()
    {
        if (SaveSystem.instance.firstEnemyDefeated == true || SaveSystem.instance.secondEnemyDefeated == true ||
            SaveSystem.instance.thirdEnemyDefeated == true)
        {
            if (continueButton != null) continueButton.SetActive(true);
        }

        foreach (var VARIABLE in AchievementButton)
        {
            Debug.Log(VARIABLE.name);
        }
    }

    public void QuitGame()
    {
        AudioManager.instance.PlaySFX("Click");
        Application.Quit();
    }
    public void SoundSettings()
    {
        AudioManager.instance.PlaySFX("Click");
        mainMenu.SetActive(false);
        settings.SetActive(true);
    }
    public void BackButton()
    {
        AudioManager.instance.PlaySFX("Click");
        mainMenu.SetActive(true);
        settings.SetActive(false);
        chooseGameDifficulty.SetActive(false);
        achievmentsMenu.SetActive(false);
    }
    public void ChooseDifficulty()
    {
        AudioManager.instance.PlaySFX("Click");
        chooseGameDifficulty.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void OpenAchivmentsMenu()
    {
        AudioManager.instance.PlaySFX("Click");
        mainMenu.SetActive(false);
        achievmentsMenu.SetActive(true);
    }

    public void ContinueGame()
    {
        AudioManager.instance.PlaySFX("Click");
        StartCoroutine(LocationHome());
    }
    
    public void LoseGame()
    {
        StartCoroutine(LocationHomeOnLose());
    }

    private IEnumerator LocationHome()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic("LocationHomeMusic");
        SceneManager.LoadScene("LocationHome");
    }
    
    private IEnumerator LocationHomeOnLose()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        if (SaveSystem.instance.isArchimage)
        {
            SaveSystem.instance.secondEnemyDefeated = false;
            SaveSystem.instance.thirdEnemyDefeated = false;
            SaveSystem.instance.fourthEnemyDefeated = false;
            SaveSystem.instance.Save();
        }
        AudioManager.instance.PlayMusic("LocationHomeMusic");
        SceneManager.LoadScene("LocationHome");
    }
    
    private void FadeIn()
    {
        animator.SetTrigger("fadeIn");
    }
    
    private void FadeOut()
    {
        animator.SetTrigger("fadeOut");
    }
}
