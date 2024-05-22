using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
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
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationHome");
    }
    
    private IEnumerator MainMenuScene()
    {
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationMainMenu");
    }
    
    private IEnumerator PrologScene()
    {
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationProlog");
    }
    
    private IEnumerator CastleScene()
    {
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationCastle");
    }
    
    private IEnumerator VillageScene()
    {
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationVillage");
    }
    
    private IEnumerator EndScene()
    {
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("EndGame");
    }
    
    private IEnumerator CastleFinalScene()
    {
        FindObjectOfType<WinLoseOption>().FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationCastleFinal");
    }
}
