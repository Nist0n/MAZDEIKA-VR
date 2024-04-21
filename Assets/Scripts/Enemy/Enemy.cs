using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    [SerializeField] private float health;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI currentHpText;
    [SerializeField] private GameObject floatingPoints;
    [SerializeField] private GameObject shieldSkill;
    
    private float _lerpTimer;
    private float _chipSpeed = 2f;
    private int _gottenDamageTimes = 0;

    private GameObject _shield;

    public bool CanTakeDamage = true;

    private void Start()
    {
        currentHealth = health;
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, health);
        
        UpdateHpBar();

        if (_gottenDamageTimes >= 5 && CanTakeDamage)
        {
            ActivateShield();
        }
    }
    
    private void UpdateHpBar()
    {
        float fillFrontBar = frontHealthBar.fillAmount;
        float fillBackBar = backHealthBar.fillAmount;
        float hFraction = currentHealth / health;

        currentHpText.text = $"{currentHealth}";

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
        
        currentHealth -= damage;
        _lerpTimer = 0f;

        if (currentHealth <= 0)
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
