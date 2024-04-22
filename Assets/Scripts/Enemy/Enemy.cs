using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI currentHpText;
    [SerializeField] private GameObject floatingPoints;
    [SerializeField] private GameObject shieldSkill;

    private PlayerController _player;

    private float _lerpTimer;
    private float _chipSpeed = 2f;
    private int _gottenDamageTimes = 0;
    private bool _gameOver = false;

    private GameObject _shield;

    public bool CanTakeDamage = true;
    public float Damage;
    public float CurrentHealth;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        CurrentHealth = health;
    }

    private void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, health);
        
        UpdateHpBar();

        if (_gottenDamageTimes >= 5 && CanTakeDamage)
        {
            ActivateShield();
        }

        if (_player.CurrentHealth == 0 && !_gameOver)
        {
            _gameOver = true;
            gameObject.GetComponent<EnemySkills>().enabled = false;
        }

        if (CurrentHealth <= 300)
        {
            CanTakeDamage = false;
        }
    }
    
    private void UpdateHpBar()
    {
        float fillFrontBar = frontHealthBar.fillAmount;
        float fillBackBar = backHealthBar.fillAmount;
        float hFraction = CurrentHealth / health;

        currentHpText.text = $"{CurrentHealth}";

        if (fillBackBar > hFraction)
        {
            frontHealthBar.fillAmount = hFraction;
            backHealthBar.color = Color.red;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            percentComplete *= percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillBackBar, hFraction, percentComplete);
        }

        if (fillFrontBar < hFraction)
        {
            backHealthBar.color = Color.green;
            backHealthBar.fillAmount = hFraction;
            _lerpTimer += Time.deltaTime;
            float percentComplete = _lerpTimer / _chipSpeed;
            percentComplete *= percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFrontBar, backHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        GameObject point = Instantiate(floatingPoints, transform.position, new Quaternion(0f, 0f, 0f, 0f), canvas.transform) as GameObject;
        point.GetComponentInChildren<TextMeshProUGUI>().text = $"{damage}";
        point.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        
        CurrentHealth -= damage;
        _lerpTimer = 0f;

        if (CurrentHealth <= 0)
        {
            Debug.Log("СМЭРТЬ");
        }

        _gottenDamageTimes++;
    }

    private void ActivateShield()
    {
        CanTakeDamage = false;
        _shield = Instantiate(shieldSkill, transform) as GameObject;
    }

    public void DeActivateShield()
    {
        CanTakeDamage = true;
        _gottenDamageTimes = 0;
        if (_shield != null)
        {
            Destroy(_shield);
        }
    }
}
