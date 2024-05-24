using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private Animator animator;
    
    public void StartBattle()
    {
        if (SaveSystem.instance.firstEnemyDefeated && !SaveSystem.instance.secondEnemyDefeated &&
            !SaveSystem.instance.thirdEnemyDefeated) LoadVillageScene();
        if (SaveSystem.instance.firstEnemyDefeated && SaveSystem.instance.secondEnemyDefeated &&
            !SaveSystem.instance.thirdEnemyDefeated) LoadCastleScene();
        if (SaveSystem.instance.firstEnemyDefeated && SaveSystem.instance.secondEnemyDefeated &&
            SaveSystem.instance.thirdEnemyDefeated) LoadCastleFinal();
    }
    
    public void LoadMainMenuScene()
    {
        StartCoroutine(MainMenuScene());
    }

    public void LoadCastleFinal()
    {
        StartCoroutine(CastleFinalScene());
    }

    public void LoadPrologScene()
    {
        StartCoroutine(PrologScene());
    }
    
    public void LoadVillageScene()
    {
        StartCoroutine(VillageScene());
    }
    
    public void LoadCastleScene()
    {
        StartCoroutine(CastleScene());
    }
    
    public void LoadHomeScene()
    {
        StartCoroutine(HomeScene());
    }

    public void LoadEndGame()
    {
        StartCoroutine(EndScene());
    }

    private IEnumerator HomeScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationHome");
    }
    
    private IEnumerator MainMenuScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic("Music1");
        SceneManager.LoadScene("LocationMainMenu");
    }
    
    private IEnumerator PrologScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic("BirdsSinging");
        SceneManager.LoadScene("LocationProlog");
    }
    
    private IEnumerator CastleScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic("BattleMusicMage3");
        SceneManager.LoadScene("LocationCastle");
    }
    
    private IEnumerator VillageScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic("BattleMusicSkel");
        SceneManager.LoadScene("LocationVillage");
    }
    
    private IEnumerator EndScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndGame");
    }
    
    private IEnumerator CastleFinalScene()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        AudioManager.instance.PlayMusic("BattleMusic");
        SceneManager.LoadScene("LocationCastleFinal");
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
