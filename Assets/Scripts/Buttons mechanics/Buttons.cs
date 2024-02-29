using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    private void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }


}
