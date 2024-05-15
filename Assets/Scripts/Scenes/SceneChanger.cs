using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("LocationMainMenu");
    }
    
    public void LoadPrologScene()
    {
        SceneManager.LoadScene("LocationProlog");
    }
    
    public void LoadVillageScene()
    {
        SceneManager.LoadScene("LocationVillage");
    }
    
    public void LoadCastleScene()
    {
        SceneManager.LoadScene("LocationCastle");
    }
    
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("LocationHome");
    }
}
