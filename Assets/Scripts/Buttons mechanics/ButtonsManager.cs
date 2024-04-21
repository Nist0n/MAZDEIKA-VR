using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonsManager : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] GameObject _settingsButtons;
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
        _settingsButtons.SetActive(true);
    }
    public void QuitSoundSettings()
    {
        _imageSetttingsButton.enabled=true;
        _mainMenu.SetActive(true);
        _settingsButtons.SetActive(false);
    }
}
