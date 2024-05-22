using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseOption : MonoBehaviour
{
    [SerializeField] private GameObject loseCanvas;
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private Animator animator;

    private bool _gameOver = false;

    private PlayerController _player;
    private FirstEnemy _enemy;
    private TrainingManager _trainingManager;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        _enemy = FindObjectOfType<FirstEnemy>();
        _trainingManager = FindObjectOfType<TrainingManager>();
    }

    private void Update()
    {
        if (_trainingManager != null)
        {
            if (_player.CurrentHealth <= 0 && !_gameOver)
            {
                StartCoroutine(OnTrainingOver());
                _gameOver = true;
            }
        }
        else
        {
            if (SceneManager.GetActiveScene().name != "LocationHome" &&
                SceneManager.GetActiveScene().name != "LocationMainMenu")
            {
                if (_player.CurrentHealth <= 0 && !_gameOver)
                {
                    OnLoseCanvas();
                    _gameOver = true;
                }

                if (_enemy.CurrentHealth <= 0 && !_gameOver)
                {
                    OnWinCanvas();
                    _gameOver = true;
                }
            }
        }
    }

    private void OnWinCanvas()
    {
        winCanvas.SetActive(true);
    }
    
    private void OnLoseCanvas()
    {
        loseCanvas.SetActive(true);
    }
    
    private IEnumerator OnTrainingOver()
    {
        FadeIn();
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("LocationHome");
    }

    public void FadeIn()
    {
        animator.SetTrigger("fadeIn");
    }
    
    public void FadeOut()
    {
        animator.SetTrigger("fadeOut");
    }
}
