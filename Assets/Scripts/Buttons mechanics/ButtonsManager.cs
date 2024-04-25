using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _canvasSettings;
    [SerializeField] GameObject _canvasGameDifficulty;
    public Image _imageSetttingsButton;
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
    public void SoundSettings()
    {
        Debug.Log("Settings");
        _imageSetttingsButton.enabled = false;
        _mainMenu.SetActive(false);
        _canvasSettings.SetActive(true);
    }
    public void BackButton()
    {
        _imageSetttingsButton.enabled=true;
        _mainMenu.SetActive(true);
        _canvasSettings.SetActive(false);
        _canvasGameDifficulty.SetActive(false);
    }
    public void ChooseDifficulty()
    {
        _canvasGameDifficulty.SetActive(true);
        _imageSetttingsButton.enabled = false;
        _mainMenu.SetActive(false);
    }
}
