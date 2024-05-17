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
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI currentHpText;
    [SerializeField] private GameObject floatingPoints;
    [SerializeField] private GameObject canvas;
    [SerializeField] private PostProcessVolume postProcessVolumeDeath;
    [SerializeField] private PostProcessVolume postProcessVolumeShield;
    //[SerializeField] private GameObject - объект картинки и позиции их расстановки

    public bool CanTakeDamage = true;
    public float Damage;
    public float CurrentHealth;
    public float IgniteDamage;
    public int GivenDamageToEnemyTimes = 0;
    public bool IsPoisoned = false;
    public bool IsStunned = false;

    private Vignette _vignetteDeath;
    private Vignette _vignetteShield;

    private float _lerpTimer;
    private float _chipSpeed = 2f;
    private float _health = 1000f;
    private bool _gameOver = false;
    private float _defence;
    private List<GameObject> _effects;

    private void Start()
    {
        CurrentHealth = _health;
        postProcessVolumeDeath.profile.TryGetSettings<Vignette>(out _vignetteDeath);
        postProcessVolumeShield.profile.TryGetSettings<Vignette>(out _vignetteShield);
    }

    private void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _health);

        if (Input.GetKeyDown(KeyCode.D))
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            RestoreHealth(Random.Range(5, 10));
        }

        if (CurrentHealth == 0 && !_gameOver)
        {
            _gameOver = true;
            gameObject.GetComponent<Skills>().enabled = false;
        }

        if (IsStunned)
        {
            gameObject.GetComponent<Skills>().enabled = false;
        }
        else
        {
            if (!_gameOver)
            {
                gameObject.GetComponent<Skills>().enabled = true;
            }
        }
        
        UpdateHpBar();
    }

    private void UpdateHpBar()
    {
        float fillFrontBar = frontHealthBar.fillAmount;
        float fillBackBar = backHealthBar.fillAmount;
        float hFraction = CurrentHealth / _health;

        _vignetteDeath.smoothness.value = 1 - hFraction;

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
        GameObject point = Instantiate(floatingPoints, new Vector3(435f, 617f, 0f), new Quaternion(0f, 0f, 0f, 0f), canvas.transform) as GameObject;
        point.GetComponentInChildren<TextMeshProUGUI>().text = $"{damage}";
        point.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        
        CurrentHealth -= damage;
        _lerpTimer = 0f;
    }

    public void RestoreHealth(float healAmount)
    {
        GameObject point = Instantiate(floatingPoints, new Vector3(435f, 617f, 0f), Quaternion.identity, canvas.transform) as GameObject;
        point.GetComponentInChildren<TextMeshProUGUI>().text = $"{healAmount}";
        point.GetComponentInChildren<TextMeshProUGUI>().color = Color.green;
        
        _lerpTimer = 0f;
        CurrentHealth += healAmount;
    }

    public IEnumerator ActivateShield()
    {
        CanTakeDamage = false;
        postProcessVolumeShield.priority = 2;
        _vignetteShield.smoothness.value = 0.9f;
        yield return new WaitForSeconds(1f);
        CanTakeDamage = true;
        _vignetteShield.smoothness.value = 0f;
        postProcessVolumeShield.priority = 0;
    }

    public void PoisonPlayer(float damage)
    {
        IsPoisoned = true;
        StartCoroutine(ActivateTickDamage(damage));
    }

    public void StunPlayer()
    {
        StartCoroutine(ActivateStun());
    }
    
    private IEnumerator ActivateTickDamage(float damage)
    {
        if (IsPoisoned)
        {
            TakeDamage(damage + _defence);
            yield return new WaitForSeconds(1f);
            StartCoroutine(ActivateTickDamage(damage));
        }
    }

    private IEnumerator ActivateStun()
    {
        IsStunned = true;
        yield return new WaitForSeconds(5f);
        IsStunned = false;
    }

    public IEnumerator IncreaseDamage()
    {
        yield return new WaitForSeconds(1f);
    }

    public void ActivateCleanSkill()
    {
        
    }

    public IEnumerator ActivateDefence()
    {
        yield return new WaitForSeconds(1f);
    }
}
