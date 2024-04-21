using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerDamage;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private float currentHealth;
    [SerializeField] private TextMeshProUGUI currentHpText;
    [SerializeField] private GameObject floatingPoints;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PostProcessVolume postProcessVolume;

    private Vignette _vignette;

    private float _lerpTimer;
    private float _chipSpeed = 2f;
    private float _health = 100;

    private void Start()
    {
        currentHealth = _health;
        postProcessVolume.profile.TryGetSettings<Vignette>(out _vignette);
    }

    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, _health);

        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            RestoreHealth(Random.Range(5, 10));
        }
        
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        float fillFrontBar = frontHealthBar.fillAmount;
        float fillBackBar = backHealthBar.fillAmount;
        float hFraction = currentHealth / _health;

        _vignette.smoothness.value = 1 - hFraction;

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
        GameObject point = Instantiate(floatingPoints, new Vector3(435f, 617f, 0f), Quaternion.identity, canvas.transform) as GameObject;
        point.GetComponentInChildren<TextMeshProUGUI>().text = $"{damage}";
        point.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        
        currentHealth -= damage;
        _lerpTimer = 0f;

        if (currentHealth < 0)
        {
            Destroy(gameObject);
        }
    }

    public void RestoreHealth(float healAmount)
    {
        GameObject point = Instantiate(floatingPoints, new Vector3(435f, 617f, 0f), Quaternion.identity, canvas.transform) as GameObject;
        point.GetComponentInChildren<TextMeshProUGUI>().text = $"{healAmount}";
        point.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        
        _lerpTimer = 0f;
        currentHealth += healAmount;
    }
}
