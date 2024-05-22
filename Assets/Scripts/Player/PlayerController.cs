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
    [SerializeField] private List<GameObject> effectPoses;
    [SerializeField] private GameObject defenceSkill;
    [SerializeField] private GameObject increaseSkill;
    [SerializeField] private GameObject stunSkill;
    [SerializeField] private GameObject poisonSkill;

    public bool CanTakeDamage = true;
    public float Damage;
    public float CurrentHealth;
    public float IgniteDamage;
    public int GivenDamageToEnemyTimes = 0;
    public bool IsPoisoned = false;
    public bool IsStunned = false;

    private Vignette _vignetteDeath;
    private Vignette _vignetteShield;
    private TrainingManager _trainingManager;
    private FirstEnemy _enemy;

    private float _lerpTimer;
    private float _chipSpeed = 2f;
    private float _health = 1000f;
    private bool _gameOver = false;
    private float _defence;
    private float _increaseDamage = 35f;
    private List<GameObject> _effects;

    private void Start()
    {
        _enemy = FindObjectOfType<FirstEnemy>();
        _trainingManager = FindObjectOfType<TrainingManager>();
        CurrentHealth = _health;
        postProcessVolumeDeath.profile.TryGetSettings<Vignette>(out _vignetteDeath);
        postProcessVolumeShield.profile.TryGetSettings<Vignette>(out _vignetteShield);
    }

    private void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, _health);

        if (CurrentHealth == 0 && !_gameOver)
        {
            _gameOver = true;
            gameObject.GetComponent<Skills>().enabled = false;
            CanTakeDamage = false;
            ActivateCleanSkill();
        }

        else
        {
            if (!_gameOver)
            {
                if (_trainingManager != null)
                {
                    if (_trainingManager.TrainingIsOver) gameObject.GetComponent<Skills>().enabled = true;
                }
                else
                {
                    gameObject.GetComponent<Skills>().enabled = true;
                }
            }
        }
        
        if (_enemy.CurrentHealth == 0 && !_gameOver)
        {
            _gameOver = true;
            gameObject.GetComponent<Skills>().enabled = false;
            CanTakeDamage = false;
            ActivateCleanSkill();
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
        damage -= _defence;
        
        GameObject point = Instantiate(floatingPoints, new Vector3(0f, 0f, 0f), new Quaternion(0f, 0f, 0f, 0f), canvas.transform) as GameObject;
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
        VisualiseEffects(poisonSkill);
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
        VisualiseEffects(stunSkill);
        yield return new WaitForSeconds(5f);
        DeleteEffect(stunSkill.name);
        IsStunned = false;
    }

    public IEnumerator IncreaseDamage()
    {
        VisualiseEffects(increaseSkill);
        Damage += _increaseDamage;
        yield return new WaitForSeconds(10f);
        Damage -= _increaseDamage;
        DeleteEffect(increaseSkill.name);
    }

    public void ActivateCleanSkill()
    {
        IsPoisoned = false;
        DeleteEffect(poisonSkill.name);
    }

    public IEnumerator ActivateDefence()
    {
        VisualiseEffects(defenceSkill);
        _defence = 25f;
        yield return new WaitForSeconds(7f);
        _defence = 0f;
        DeleteEffect(defenceSkill.name);
    }

    private void VisualiseEffects(GameObject effect)
    {
        for (int i = 0; i < effectPoses.Count; i++)
        {
            if (effectPoses[i].GetComponentInChildren<SkillEffects>() == null)
            {
                Instantiate(effect, effectPoses[i].transform);
                break;
            }
        }
    }

    private void DeleteEffect(string name)
    {
        for (int i = 0; i < effectPoses.Count; i++)
        {
            if (effectPoses[i].GetComponentInChildren<SkillEffects>() != null)
            {
                if (effectPoses[i].GetComponentInChildren<SkillEffects>().gameObject.name.Contains(name))
                {
                    Destroy(effectPoses[i].GetComponentInChildren<SkillEffects>().gameObject);
                }
            }
        }
    }
}
