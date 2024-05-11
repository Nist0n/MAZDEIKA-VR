using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstEnemy : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [SerializeField] private TextMeshProUGUI currentHpText;
    [SerializeField] private GameObject floatingPoints;
    [SerializeField] private GameObject shieldSkill;
    [SerializeField] private GameObject stunAura;
    [SerializeField] private GameObject igniteAura;
    [SerializeField] private Animator animator;

    private PlayerController _player;

    private float _lerpTimer;
    private float _chipSpeed = 2f;
    private bool _gameOver = false;
    private float _defence;
    private float _isBurningTime;
    private float _igniteTimer = 5f;
    private bool _isAttacking = false;

    private GameObject _shield;

    public bool CanTakeDamage = true;
    public float Damage;
    public float CurrentHealth;
    public bool IsStunned = false;
    public bool IsBurning = false;
    public float PoisonDamage;

    private void Start()
    {
        _player = FindObjectOfType<PlayerController>();
        CurrentHealth = health;
    }

    private void Update()
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, health);
        
        UpdateHpBar();

        if (_player.GivenDamageToEnemyTimes >= 5 && CanTakeDamage)
        {
            ActivateShield();
        }

        if ((_player.CurrentHealth == 0 && !_gameOver) || (CurrentHealth == 0 && !_gameOver))
        {
            _gameOver = true;
        }

        if (IsBurning)
        {
            _isBurningTime += Time.deltaTime;
            if (_isBurningTime >= _igniteTimer)
            {
                IsBurning = false;
                _isBurningTime = 0;
            }
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
        damage -= _defence;
        //if (!animator.GetBool("isAttacking")) animator.SetTrigger("takeDamage");

        damage = Mathf.Clamp(damage, 0, Single.PositiveInfinity);

        GameObject point = Instantiate(floatingPoints, transform.position, new Quaternion(0f, 0f, 0f, 0f), canvas.transform) as GameObject;
        point.GetComponentInChildren<TextMeshProUGUI>().text = $"{damage}";
        point.GetComponentInChildren<TextMeshProUGUI>().color = Color.red;
        
        CurrentHealth -= damage;
        _lerpTimer = 0f;

        if (CurrentHealth <= 0)
        {
            Debug.Log("СМЭРТЬ");
        }
    }

    private void ActivateShield()
    {
        CanTakeDamage = false;
        _shield = Instantiate(shieldSkill, transform) as GameObject;
    }

    public void DeActivateShield()
    {
        CanTakeDamage = true;
        _player.GivenDamageToEnemyTimes = 0;
        if (_shield != null)
        {
            Destroy(_shield);
        }
    }

    public IEnumerator ActivateStunEffect()
    {
        IsStunned = true;
        GameObject tempStunAura = Instantiate(stunAura, transform) as GameObject;
        yield return new WaitForSeconds(3f);
        Destroy(tempStunAura);
        IsStunned = false;
    }

    public void UpdateDefence(float defence)
    {
        _defence = defence;
    }

    public IEnumerator ActivateTickDamage(float damage)
    {
        if (IsBurning)
        {
            TakeDamage(damage + _defence);
            yield return new WaitForSeconds(1f);
            StartCoroutine(ActivateTickDamage(damage));
        }
    }

    public IEnumerator ActivateIgniteAura()
    {
        GameObject tempIgniteAura = Instantiate(igniteAura, transform) as GameObject;
        Debug.Log("funny");
        yield return new WaitForSeconds(_igniteTimer);
        Debug.Log("Destroyed");
        Destroy(tempIgniteAura);
    }
}
