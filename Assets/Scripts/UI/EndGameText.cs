using System;
using System.Collections;
using System.Collections.Generic;
using DialogueEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameText : MonoBehaviour
{
    [SerializeField] private NPCConversation endText;
    [SerializeField] private Animator animator;

    private void Start()
    {
        AudioManager.instance.PlayMusic("WinFinal");
        StartDialog(endText);
        FadeOut();
    }

    private void StartDialog(NPCConversation dialog)
    {
        ConversationManager.Instance.StartConversation(dialog);
    }
    
    public void FadeOut()
    {
        animator.SetTrigger("fadeOut");
    }
    
    public void FadeIn()
    {
        animator.SetTrigger("fadeIn");
    }
    
    public void LoadEndGame()
    {
        StartCoroutine(EndGame());
        SaveSystem.instance.isArchimage = false;
        SaveSystem.instance.firstEnemyDefeated = false;
        SaveSystem.instance.secondEnemyDefeated = false;
        SaveSystem.instance.fourthEnemyDefeated = false;
        SaveSystem.instance.thirdEnemyDefeated = false;
        SaveSystem.instance.Save();
    }
    
    private IEnumerator EndGame()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationMainMenu");
    }
}
